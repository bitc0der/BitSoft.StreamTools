using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace BitSoft.StreamTools.Buffers;

public class ArrayPoolStringBuffer : IStringBuffer
{
	private readonly Encoding _encoding;
	private readonly ArrayPool<char> _pool;

	private char[]? _array;
	private int _offset;

	private bool _disposed;

	public ArrayPoolStringBuffer(Encoding? encoding = null, ArrayPool<char>? pool = null)
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

		var maxCharsCount = _encoding.GetMaxCharCount(buffer.Length);

		if (_array is null)
		{
			_array = _pool.Rent(minimumLength: maxCharsCount);
		}
		else
		{
			var leftChars = _array.Length - _offset;
			if (leftChars < maxCharsCount)
			{
				var newArray = _pool.Rent(minimumLength: _offset + maxCharsCount);
				Array.Copy(sourceArray: _array, destinationArray: newArray, length: _offset);
				_pool.Return(_array);
				_array = newArray;
			}
		}
		var charsSpan = _array.AsSpan(start: _offset);
		_offset += _encoding.GetChars(bytes: buffer.Span, chars: charsSpan);
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