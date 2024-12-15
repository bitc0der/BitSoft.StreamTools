using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace BitSoft.StreamTools.Buffers;

public class ArrayPoolQueueStringBuffer : IStringBuffer
{
	private readonly Encoding _encoding;
	private readonly ArrayPool<char> _pool;
	private readonly int _bufferSize;

	private QueueItm? _root;
	private QueueItm? _last;

	private bool _disposed;

	public int Length { get; private set; }

	public ArrayPoolQueueStringBuffer(
		Encoding? encoding = null,
		ArrayPool<char>? pool = null,
		int bufferSize = 128 * 1024)
	{
		_pool = pool ?? ArrayPool<char>.Shared;
		_encoding = encoding ?? Encoding.UTF8;
		_bufferSize = bufferSize;
	}

	public void Append(ReadOnlyMemory<byte> buffer)
	{
		if (buffer.Length == 0) return;

		CheckDisposed();

		var item = _last is null
			? QueueItm.Create(_pool, length: _bufferSize)
			: _last;

		if (_last is null)
		{
			_root = item;
			_last = _root;
		}

		var offset = 0;
		while (true)
		{
			Span<char> span;

			while (!item.TryGetEmptySpan(out span))
			{
				var newItem = QueueItm.Create(_pool, length: _bufferSize);
				item.SetNext(newItem);
				item = newItem;
			}

			var maxBytesCount = _encoding.GetByteCount(span);
			var left = Math.Min(buffer.Length - offset, maxBytesCount);

			var bytesSpan = buffer.Slice(start: offset, length: left).Span;

			var length = _encoding.GetChars(bytes: bytesSpan, chars: span);
			item.SetLength(length);

			offset += left;
			Length += length;

			if (offset == buffer.Length)
				break;
		}
	}

	public string Build()
	{
		if (_root is null) return string.Empty;

		CheckDisposed();

		if (_root == _last)
		{
			return new string(_root.Array.AsSpan(start: 0, length: _root.Length));
		}

		return string.Create(length: Length, _root, RenderString);
	}

	private static void RenderString(Span<char> chars, QueueItm root)
	{
		var offset = 0;
		var current = root;

		while (current is not null)
		{
			var sourceSpan = current.Span;
			if (sourceSpan.IsEmpty)
				break;
			var targetSpan = chars.Slice(start: offset, length: sourceSpan.Length);

			sourceSpan.CopyTo(targetSpan);

			offset += current.Length;
			current = current.Next;
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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckDisposed()
	{
		if (_disposed) throw new ObjectDisposedException(GetType().FullName);
	}

	public void Dispose()
	{
		if (_disposed) return;

		Clear();

		_disposed = true;
	}

	private sealed class QueueItm : IDisposable
	{
		private readonly ArrayPool<char> _pool;

		public char[] Array { get; }

		public int Length { get; private set; } = 0;

		public QueueItm? Next { get; private set; }

		private QueueItm(char[] array, ArrayPool<char> pool)
		{
			Array = array ?? throw new ArgumentNullException(nameof(array));
			_pool = pool ?? throw new ArgumentNullException(nameof(pool));
		}

		public static QueueItm Create(char[] buffer, ArrayPool<char> pool)
		{
			return new QueueItm(buffer, pool);
		}

		public static QueueItm Create(ArrayPool<char> pool, int length)
		{
			var array = pool.Rent(minimumLength: length);

			return new QueueItm(array, pool);
		}

		public void SetLength(int length)
		{
			Length = length;
		}

		public Span<char> Span => Length == 0 ? [] : Array.AsSpan(start: 0, length: Length);

		public bool TryGetEmptySpan(out Span<char> span)
		{
			if (Length < Array.Length)
			{
				span = Array.AsSpan(start: Length);
				return true;
			}
			span = [];
			return false;
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