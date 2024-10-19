using System;
using System.Buffers;
using System.Text;

namespace StreamTools;

public class MemoryStringBuffer : IStringBuffer
{
	private readonly Encoding _encoding;
	private readonly MemoryPool<char> _memoryPool;

	private IMemoryOwner<char>? _memoryOwner;
	private int _offset;

	public MemoryStringBuffer(Encoding? encoding = null, MemoryPool<char>? memoryPool = null)
	{
		_encoding = encoding ?? Encoding.UTF8;
		_memoryPool = memoryPool ?? MemoryPool<char>.Shared;
	}

	public int Length => _offset;

	public void Append(byte[] buffer, int offset, int length)
	{
		if (buffer is null) throw new ArgumentNullException(nameof(buffer));

		if (length == 0)
			return;

		if (_memoryOwner is null)
		{
			_memoryOwner = _memoryPool.Rent(minBufferSize: length);
		}
		else
		{
			var requiredLength = _offset + length;
			if (_memoryOwner.Memory.Length < requiredLength)
			{
				var owner = _memoryPool.Rent(requiredLength);
				_memoryOwner.Memory.CopyTo(owner.Memory);
				_memoryOwner.Dispose();
				_memoryOwner = owner;
			}
		}

		var charsCount = _encoding.GetCharCount(buffer, offset, length);
		var bytesSpan = buffer.AsSpan(start: offset, length: length);
		var charsSpan = _memoryOwner.Memory.Span.Slice(start: offset, length: charsCount);

		var result = _encoding.GetChars(bytes: bytesSpan, chars: charsSpan);

		_offset += result;
	}

	public string Build()
	{
		return _memoryOwner is null
			? string.Empty
			: new string(_memoryOwner.Memory.Span[0.._offset]);
	}

	public void Dispose()
	{
		_memoryOwner?.Dispose();
	}
}