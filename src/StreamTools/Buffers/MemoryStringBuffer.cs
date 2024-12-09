﻿using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace StreamTools.Buffers;

public class MemoryStringBuffer : IStringBuffer
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

	public MemoryStringBuffer(Encoding? encoding = null, MemoryPool<char>? memoryPool = null)
	{
		_encoding = encoding ?? Encoding.UTF8;
		_memoryPool = memoryPool ?? MemoryPool<char>.Shared;
	}

	public void Append(ReadOnlyMemory<byte> buffer)
	{
		CheckDisposed();

		if (buffer.Length == 0) return;

		if (_memoryOwner is null)
		{
			_memoryOwner = _memoryPool.Rent(minBufferSize: buffer.Length);
		}
		else
		{
			var requiredLength = _offset + buffer.Length;
			if (_memoryOwner.Memory.Length < requiredLength)
			{
				var owner = _memoryPool.Rent(requiredLength);
				_memoryOwner.Memory.CopyTo(owner.Memory);
				_memoryOwner.Dispose();
				_memoryOwner = owner;
			}
		}

		var bytesSpan = buffer.Span;
		var charsCount = _encoding.GetCharCount(bytesSpan);
		var charsSpan = _memoryOwner.Memory.Span.Slice(start: _offset, length: charsCount);

		var result = _encoding.GetChars(bytes: bytesSpan, chars: charsSpan);

		_offset += result;
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