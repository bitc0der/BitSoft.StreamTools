using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Text;
using StreamTools.Buffers;

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
	public override bool CanSeek => _mode == StringStreamMode.Read;
	public override bool CanWrite => _mode == StringStreamMode.Write;

	public override long Length => GetLength();

	public override long Position
	{
		get => GetPosition();
		set => SetPosition(value);
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
		Func<Encoding, IStringBuffer>? stringBuffer = null,
		Encoding? encoding = null)
	{
		_mode = StringStreamMode.Write;
		_encoding = encoding ?? DefaultEncoding;
		_buffer = stringBuffer is null
			? new MemoryStringBuffer(_encoding)
			: stringBuffer(_encoding);
	}

	public static StringStream WithStringBuilder(Encoding? encoding = null) => new(e => new StringBuilderBuffer(e), encoding);

	public static StringStream WithArrayPool(
		Encoding? encoding = null,
		ArrayPool<char>? arrayPool = null
	) => new(e => new ArrayStringBuffer(e, arrayPool), encoding);

	public static StringStream WithMemoryPool(
		Encoding? encoding = null,
		MemoryPool<char>? memoryPool = null
	) => new(e => new MemoryStringBuffer(e, memoryPool), encoding);

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

	private long GetLength()
	{
		return _mode switch
		{
			StringStreamMode.Read => _source.Length,
			StringStreamMode.Write => GetInternalBufferLength(),
			_ => throw new InvalidOperationException("Invalid mode"),
		};
	}

	private long GetPosition()
	{
		return _mode switch
		{
			StringStreamMode.Read => _offset,
			StringStreamMode.Write => GetInternalBufferLength(),
			_ => throw new InvalidOperationException("Invalid mode")
		};
	}

	private void SetPosition(long position)
	{
		CheckMode(StringStreamMode.Read);

		if (position < 0 || position > int.MaxValue)
			throw new ArgumentOutOfRangeException(nameof(position));

		_offset = (int)position;
	}

	private int GetInternalBufferLength() => _buffer is null ? 0 : _buffer.Length;

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