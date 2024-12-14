using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace BitSoft.StreamTools.Buffers;

public class MemoryPoolStringBuffer : IStringBuffer
{
	private readonly Encoding _encoding;
	private readonly MemoryPool<char> _memoryPool;

	private IMemoryOwner<char>? _memoryOwner;
	private int _offset;

	private bool _disposed;

	public int Length
	{
		get
		{
			CheckDisposed();
			return _offset;
		}
	}

	public MemoryPoolStringBuffer(Encoding? encoding = null, MemoryPool<char>? memoryPool = null)
	{
		_encoding = encoding ?? Encoding.UTF8;
		_memoryPool = memoryPool ?? MemoryPool<char>.Shared;
	}

	public void Append(ReadOnlyMemory<byte> buffer)
	{
		CheckDisposed();

		if (buffer.Length == 0) return;

		var maxCharsCount = _encoding.GetMaxCharCount(buffer.Length);

		if (_memoryOwner is null)
		{
			_memoryOwner = _memoryPool.Rent(minBufferSize: maxCharsCount);
		}
		else
		{
			var requiredLength = _offset + maxCharsCount;
			if (_memoryOwner.Memory.Length < requiredLength)
			{
				var owner = _memoryPool.Rent(requiredLength);
				_memoryOwner.Memory.CopyTo(owner.Memory);
				_memoryOwner.Dispose();
				_memoryOwner = owner;
			}
		}

		var actualCharsCount = _encoding.GetChars(bytes: buffer.Span, chars: _memoryOwner.Memory.Span[_offset..]);

		_offset += actualCharsCount;
	}

	public string Build()
	{
		CheckDisposed();

		return _memoryOwner is null
			? string.Empty
			: new string(_memoryOwner.Memory.Span[0.._offset]);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckDisposed() => ObjectDisposedException.ThrowIf(_disposed, instance: this);

	public void Dispose()
	{
		if (_disposed) return;

		if (_memoryOwner is not null)
		{
			_memoryOwner?.Dispose();
			_memoryOwner = null;
		}

		_disposed = true;
	}
}