using System;
using System.Buffers;
using System.Text;

namespace BitSoft.StreamTools.Buffers;

public sealed class StringBuilderBuffer : IStringBuffer
{
	private readonly Encoding _encoding;
	private readonly ArrayPool<char> _pool;

	private StringBuilder? _stringBuilder;

	public StringBuilderBuffer(
		Encoding? encoding = null,
		StringBuilder? stringBuilder = null,
		ArrayPool<char>? pool = null)
	{
		_encoding = encoding ?? Encoding.UTF8;
		_pool = pool ?? ArrayPool<char>.Shared;
		_stringBuilder = stringBuilder;
	}

	public int Length => _stringBuilder is null
		? 0
		: _stringBuilder.Length;

	public void Append(ReadOnlyMemory<byte> buffer)
	{
		if (buffer.Length == 0) return;

		var bufferSpan = buffer.Span;
		var charsCount = _encoding.GetCharCount(bufferSpan);

		if (_stringBuilder is null)
			_stringBuilder = new StringBuilder(capacity: charsCount);

		var array = _pool.Rent(minimumLength: charsCount);

		try
		{
			var result = _encoding.GetChars(bytes: bufferSpan, chars: array);

			var span = array.AsSpan(start: 0, length: result);

			_stringBuilder.Append(span);
		}
		finally
		{
			_pool.Return(array);
		}
	}

	public string Build() => _stringBuilder is null
		? string.Empty
		: _stringBuilder.ToString();

	public void Dispose()
	{
		// do nothing
	}
}