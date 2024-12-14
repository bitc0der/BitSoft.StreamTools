using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace StreamTools.Buffers;

public class ArrayStringBuffer : IStringBuffer
{
	private readonly Encoding _encoding;
	private readonly ArrayPool<char> _pool;

	private char[]? _array;
	private int _offset;

	private bool _disposed;

	public ArrayStringBuffer(Encoding? encoding = null, ArrayPool<char>? pool = null)
	{
		_pool = pool ?? ArrayPool<char>.Shared;
		_encoding = encoding ?? Encoding.UTF8;
	}

	public int Length
	{
		get
		{
			CheckDisposed();
			return _offset;
		}
	}

	public void Append(ReadOnlyMemory<byte> buffer)
	{
		CheckDisposed();

		if (buffer.Length == 0)
			return;

		var charsCount = _encoding.GetCharCount(buffer.Span);

		if (_array is null)
		{
			_array = _pool.Rent(minimumLength: charsCount);
		}
		else
		{
			var left = _array.Length - _offset;
			if (left < buffer.Length)
			{
				var newArray = _pool.Rent(minimumLength: _offset + buffer.Length);
				Array.Copy(sourceArray: _array, destinationArray: newArray, length: _offset);
				_pool.Return(_array);
				_array = newArray;
			}
		}
		var bytesSpan = buffer.Span;
		var charsSpan = _array.AsSpan(start: _offset, length: charsCount);
		var result = _encoding.GetChars(bytes: bytesSpan, chars: charsSpan);
		_offset += result;
	}

	public string Build()
	{
		CheckDisposed();

		return _array is null
			? string.Empty
			: new(_array.AsSpan(start: 0, length: _offset));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckDisposed()
	{
		if (_disposed) throw new ObjectDisposedException(GetType().FullName);
	}

	public void Dispose()
	{
		if (_disposed) return;

		if (_array is not null)
		{
			_pool.Return(_array);
			_array = null;
		}

		_disposed = true;
	}
}