using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BitSoft.StreamTools.Buffers;

public class ArrayPoolQueueStringBuffer : IStringBuffer
{
	private readonly Encoding _encoding;
	private readonly ArrayPool<char> _pool;

	private List<QueueItm>? _queue;

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

		_queue ??= new List<QueueItm>();

		try
		{
			array = _pool.Rent(maxCharsCount);
			var length = _encoding.GetChars(bytes: buffer.Span, chars: array.AsSpan());
			var item = new QueueItm(array, _pool, length: length);
			_queue.Add(item);
		}
		catch
		{
			if (array is not null)
				_pool.Return(array);
		}
	}

	private int GetLength()
	{
		if (_queue?.Count == 0 ) return 0;

		Debug.Assert(_queue is not null);

		var length = 0;
		for (var i = 0; i < _queue.Count; i++)
		{
			var item = _queue[i];
			length += item.Length;
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
			for (var i = 0; i < _queue!.Count; i++)
			{
				var item = _queue[i];

				Array.Copy(
					sourceArray: item.Array,
					sourceIndex: 0,
					destinationArray: array,
					destinationIndex: offset,
					length: item.Length
				);
			}

			var queue = new List<QueueItm>();
			queue.Add(new QueueItm(array, _pool, length));

			Clear();

			_queue = queue;

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
		if (_queue is null) return;

		for (var i = 0; i < _queue.Count; i++)
		{
			var item = _queue[i];
			item.Dispose();
		}

		_queue = null;
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

		public QueueItm(char[] array, ArrayPool<char> pool, int length)
		{
			Array = array ?? throw new ArgumentNullException(nameof(array));
			_pool = pool ?? throw new ArgumentNullException(nameof(pool));
			Length = length;
		}

		public void Dispose()
		{
			_pool.Return(Array);
		}
	}
}