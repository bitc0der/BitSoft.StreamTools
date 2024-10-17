using System;
using System.Buffers;
using System.Text;

namespace StreamTools;

public sealed class StringBuilderBuffer : IStringBuffer
{
	private readonly Encoding _encoding;
	private readonly ArrayPool<char> _pool;

	private readonly StringBuilder _stringBuilder = new();

	public StringBuilderBuffer(Encoding? encoding = null, ArrayPool<char>? pool = null)
	{
		_encoding = encoding ?? Encoding.UTF8;
		_pool = pool ?? ArrayPool<char>.Shared;
	}

	public void Append(byte[] buffer, int offset, int length)
	{
		if (buffer is null) throw new ArgumentNullException(nameof(buffer));

		if (length == 0) return;

		var maxCharsCount = _encoding.GetCharCount(buffer);

		var array = _pool.Rent(minimumLength: maxCharsCount);

		try
		{
			var result = _encoding.GetChars(bytes: buffer, chars: array);

			var span = array.AsSpan(start: 0, length: result);

			_stringBuilder.Append(span);
		}
		finally
		{
			_pool.Return(array);
		}
	}

	public string Build() => _stringBuilder.ToString();
	public void Dispose()
	{
		// do nothing
	}
}