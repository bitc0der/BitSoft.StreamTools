using System;
using System.IO;
using System.Text;

namespace StreamTools;

public class StringStream : Stream
{
	public static Encoding DefaultEncoding { get; } = Encoding.UTF8;

	private readonly ReadOnlyMemory<char> _source;
	private readonly Encoding _encoding;

	private int _offset;

	public override bool CanRead => _offset < _source.Length - 1;
	public override bool CanSeek => false;
	public override bool CanWrite => false;

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
		_source = source;
		_encoding = encoding ?? DefaultEncoding;
	}

	public override void Flush()
	{
		// do nothing
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (buffer is null) throw new ArgumentNullException(nameof(buffer));

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
	public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
}