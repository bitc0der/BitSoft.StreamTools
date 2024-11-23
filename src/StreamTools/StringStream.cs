using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
		set => Seek(offset: value, SeekOrigin.Begin);
	}

	public StringStream(string source, Encoding? encoding = null)
		: this(source: source.AsMemory(), encoding: encoding)
	{
		ArgumentNullException.ThrowIfNull(source);
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

	public static StringStream WithStringBuilder(
		Encoding? encoding = null,
		StringBuilder? stringBuilder = null,
		ArrayPool<char>? arrayPool = null
	) => new(e => new StringBuilderBuffer(e, stringBuilder, arrayPool), encoding);

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

	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		var result = Read(buffer, offset, count);

		return Task.FromResult(result);
	}

	public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
	{
		CheckMode(StringStreamMode.Read);

		if (buffer.Length == 0)
			return new ValueTask<int>(0);

		var maxChars = _encoding.GetMaxCharCount(byteCount: buffer.Length) - 1;
		var charsToRead = Math.Min(_source.Length - _offset, maxChars);

		if (charsToRead == 0)
			return new ValueTask<int>(0);

		var charsSpan = _source.Slice(start: _offset, length: charsToRead).Span;
		var bytesSpan = buffer.Span;

		var result = _encoding.GetBytes(chars: charsSpan, bytes: bytesSpan);

		_offset += _encoding.GetCharCount(bytes: buffer[..result].Span);

		return new ValueTask<int>(result);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		CheckMode(StringStreamMode.Read);

		if (offset is < 0 or > int.MaxValue)
			throw new ArgumentOutOfRangeException(nameof(offset));

		var newOffset = origin switch
		{
			SeekOrigin.Begin => offset,
			SeekOrigin.Current => _offset + offset,
			SeekOrigin.End => _source.Length - offset,
			_ => throw new ArgumentOutOfRangeException(paramName: nameof(origin))
		};

		if (newOffset < 0 || newOffset > _source.Length)
			throw new ArgumentOutOfRangeException(nameof(offset));

		_offset = (int)newOffset;

		return _offset;
	}

	public override void SetLength(long value) => throw new NotSupportedException();

	public override void Write(byte[] buffer, int offset, int count)
	{
		ArgumentNullException.ThrowIfNull(buffer);

		CheckMode(StringStreamMode.Write);

		if (count == 0) return;

		Debug.Assert(_buffer is not null);

		var memory = buffer.AsMemory(start: offset, length: count);

		_buffer.Append(memory);
	}

	public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(buffer);

		var memory = buffer.AsMemory(start: offset, length: count);

		await WriteAsync(memory, cancellationToken);
	}

	public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
	{
		CheckMode(StringStreamMode.Write);

		Debug.Assert(_buffer is not null);

		_buffer.Append(buffer);

		return ValueTask.CompletedTask;
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