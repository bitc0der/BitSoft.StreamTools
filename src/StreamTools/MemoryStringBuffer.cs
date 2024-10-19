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
			if (_memoryOwner.Memory.Length < _offset + length)
			{
				var owner = _memoryPool.Rent(_offset + length);
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
		if (_memoryOwner is null)
			return string.Empty;
		return new string(_memoryOwner.Memory.Span);
	}

	public void Dispose()
	{
		_memoryOwner?.Dispose();
	}
}