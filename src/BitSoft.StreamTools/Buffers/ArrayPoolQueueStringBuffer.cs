using System;
using System.Buffers;
using System.Text;

namespace BitSoft.StreamTools.Buffers;

public class ArrayPoolQueueStringBuffer : IStringBuffer
{
	private readonly Encoding _encoding;
	private readonly ArrayPool<char> _pool;

	private QueueItm? _root;
	private QueueItm? _last;

	public int Length => GetLength();

	public ArrayPoolQueueStringBuffer(Encoding? encoding = null, ArrayPool<char>? pool = null)
	{
		_pool = pool ?? ArrayPool<char>.Shared;
		_encoding = encoding ?? Encoding.UTF8;
	}

	public void Append(ReadOnlyMemory<byte> buffer)
	{
		if (buffer.Length == 0) return;

		var maxCharsCount = _encoding.GetMaxByteCount(buffer.Length);

		char[]? array = null;

		try
		{
			array = _pool.Rent(maxCharsCount);
			var length = _encoding.GetChars(bytes: buffer.Span, chars: array.AsSpan());
			var item = new QueueItm(array, _pool, length: length);

			if (_last is null)
			{
				_root ??= item;
				_last = _root;
			}
			else
			{
				_last.SetNext(item);
			}
		}
		catch
		{
			if (array is not null)
				_pool.Return(array);
		}
	}

	private int GetLength()
	{
		if (_root is null) return 0;

		var length = 0;
		var current = _root;
		while (current is not null)
		{
			length += current.Length;
			current = current.Next;
		}
		return length;
	}

	public string Build()
	{
		var length = GetLength();

		if (length == 0) return string.Empty;

		char[]? array = null;
		try
		{
			array = _pool.Rent(length);

			var offset = 0;
			var current = _root;

			while (current is not null)
			{
				Array.Copy(
					sourceArray: current.Array,
					sourceIndex: 0,
					destinationArray: array,
					destinationIndex: offset,
					length: current.Length
				);
				offset += current.Length;
				current = current.Next;
			}

			Clear();

			_root = new(array, _pool, length);
			_last = _root;

			return new string(array.AsSpan(start: 0, length: length));
		}
		finally
		{
			if (array is not null)
				_pool.Return(array);
		}
	}

	private void Clear()
	{
		var current = _root;
		while (current is not null)
		{
			current.Dispose();
			current = current.Next;
		}

		_root = null;
		_last = null;
	}

	public void Dispose()
	{
		Clear();
	}

	private sealed class QueueItm : IDisposable
	{
		private readonly ArrayPool<char> _pool;
		
		public char[] Array { get; }
		public int Length { get; }

		public QueueItm? Next { get; private set; }

		public QueueItm(char[] array, ArrayPool<char> pool, int length)
		{
			Array = array ?? throw new ArgumentNullException(nameof(array));
			_pool = pool ?? throw new ArgumentNullException(nameof(pool));
			Length = length;
		}

		public void SetNext(QueueItm item)
		{
			Next = item ?? throw new ArgumentNullException(nameof(item));
		}

		public void Dispose()
		{
			_pool.Return(Array);
		}
	}
}