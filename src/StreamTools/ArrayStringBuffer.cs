using System;
using System.Buffers;
using System.Text;

namespace StreamTools;

public class ArrayStringBuffer : IStringBuffer
{
	private readonly Encoding _encoding;
	private readonly ArrayPool<char> _pool;

	private char[]? _array;
	private int _offset;

	public ArrayStringBuffer(Encoding? encoding = null, ArrayPool<char>? pool = null)
	{
		_pool = pool ?? ArrayPool<char>.Shared;
		_encoding = encoding ?? Encoding.UTF8;
	}

	public void Append(byte[] buffer, int offset, int length)
	{
		if (buffer is null) throw new ArgumentNullException(nameof(buffer));

		if (length == 0) return;

		var charsCount = _encoding.GetCharCount(buffer, index: offset, count: length);

		if (_array is null)
		{
			_array = _pool.Rent(minimumLength: charsCount);
		}
		else
		{
			var left = _array.Length - _offset;
			if (left < length)
			{
				var newArray = _pool.Rent(minimumLength: _offset + length);
				Array.Copy(sourceArray: _array, destinationArray: newArray, length: _offset);
				_pool.Return(_array);
				_array = newArray;
			}
		}
		var bytesSpan = buffer.AsSpan(start: offset, length: length);
		var charsSpan = _array.AsSpan(start: _offset, length: charsCount);
		var result = _encoding.GetChars(bytes: bytesSpan, chars: charsSpan);
		_offset += result;
	}

	public string Build()
	{
		return _array is null
			? string.Empty
			: new(_array.AsSpan(start: 0, length: _offset));
	}

	public void Dispose()
	{
		if (_array is null)
			return;

		_pool.Return(_array);
		_array = null;
	}
}