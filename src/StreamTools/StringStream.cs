using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace StreamTools;

public class StringStream : Stream
{
	public static Encoding DefaultEncoding { get; } = Encoding.UTF8;

	private readonly StringStreamMode _mode;

	private readonly ReadOnlyMemory<char> _source;
	private readonly Encoding _encoding;

	private readonly IStringBuffer? _buffer;

	private int _offset;

	public override bool CanRead => _mode == StringStreamMode.Read && _offset < _source.Length - 1;
	public override bool CanSeek => false;
	public override bool CanWrite => _mode == StringStreamMode.Write;

	public override long Length => throw new NotSupportedException();

	public override long Position
	{
		get => throw new NotSupportedException();
		set => throw new NotSupportedException();
	}

	public StringStream(string source, Encoding? encoding = null)
		: this(source: source.AsMemory(), encoding: encoding)
	{
		if (source is null) throw new ArgumentNullException(nameof(source));
	}

	public StringStream(ReadOnlyMemory<char> source, Encoding? encoding = null)
	{
		_mode = StringStreamMode.Read;
		_source = source;
		_encoding = encoding ?? DefaultEncoding;
	}

	public StringStream(
		Func<IStringBuffer>? stringBuffer = null,
		Encoding? encoding = null)
	{
		_mode = StringStreamMode.Write;
		_encoding = encoding ?? DefaultEncoding;
		_buffer = stringBuffer is null
			? new MemoryStringBuffer(_encoding)
			: stringBuffer();
	}

	public override void Flush()
	{
		// do nothing
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (buffer is null) throw new ArgumentNullException(nameof(buffer));

		CheckMode(StringStreamMode.Read);

		if (count == 0)
			return 0;

		var maxChars = _encoding.GetMaxCharCount(byteCount: count) - 1;
		var charsToRead = Math.Min(_source.Length - _offset, maxChars);

		if (charsToRead == 0)
			return 0;

		var charsSpan = _source.Slice(start: _offset, length: charsToRead).Span;
		var bytesSpan = buffer.AsSpan(start: offset, length: count);

		var result = _encoding.GetBytes(chars: charsSpan, bytes: bytesSpan);

		_offset += _encoding.GetCharCount(bytes: buffer, index: offset, count: result);

		return result;
	}

	public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
	public override void SetLength(long value) => throw new NotSupportedException();

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (buffer is null) throw new ArgumentNullException(nameof(buffer));

		CheckMode(StringStreamMode.Write);

		if (count == 0) return;

		Debug.Assert(_buffer is not null);

		_buffer.Append(buffer, offset, length: count);
	}

	public string GetString()
	{
		if (_mode == StringStreamMode.Read)
			return new string(_source.Span);
		if (_mode == StringStreamMode.Write)
		{
			Debug.Assert(_buffer is not null);
			return _buffer.Build();
		}
		throw new InvalidOperationException("Unknown mode");
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		if (_mode == StringStreamMode.Write)
		{
			_buffer?.Dispose();
		}
	}

	private void CheckMode(StringStreamMode mode)
	{
		if (_mode == mode) return;

		throw new InvalidOperationException("Invalid mode");
	}

	private enum StringStreamMode
	{
		Read,
		Write
	}
}