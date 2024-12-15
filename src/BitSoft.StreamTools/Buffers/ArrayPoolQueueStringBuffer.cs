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
		int bufferSize = 1024 * 1024)
	{
		_pool = pool ?? ArrayPool<char>.Shared;
		_encoding = encoding ?? Encoding.UTF8;
		_bufferSize = bufferSize;
	}

	public void Append(ReadOnlyMemory<byte> buffer)
	{
		if (buffer.Length == 0) return;

		CheckDisposed();

		var item = _last is null ? CreateItem() : _last;

		if (_last is null)
		{
			_root = item;
			_last = _root;
		}

		var offset = 0;
		while (offset < buffer.Length)
		{
			Span<char> span;

			while (!item.TryGetEmptySpan(out span))
			{
				var newItem = CreateItem();
				item.Next = newItem;
				item = newItem;
			}

			var maxBytesCount = _encoding.GetByteCount(span);
			var left = Math.Min(buffer.Length - offset, maxBytesCount);

			var bytesSpan = buffer.Slice(start: offset, length: left).Span;

			var length = _encoding.GetChars(bytes: bytesSpan, chars: span);
			item.Length = length;

			offset += left;
			Length += length;

			if (offset == buffer.Length)
				break;
		}
	}

	private QueueItm CreateItem()
	{
		var array = _pool.Rent(minimumLength: _bufferSize);

		return new QueueItm(array);
	}

	public string Build()
	{
		if (_root is null) return string.Empty;

		CheckDisposed();

		return _root == _last
			? new string(_root.Span)
			: string.Create(length: Length, _root, RenderString);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckDisposed()
	{
		if (_disposed) throw new ObjectDisposedException(GetType().FullName);
	}

	public void Dispose()
	{
		if (_disposed) return;

		var current = _root;
		while (current is not null)
		{
			_pool.Return(current.Array);
			current = current.Next;
		}

		_root = null;
		_last = null;

		_disposed = true;
	}

	private sealed class QueueItm
	{
		public char[] Array { get; }

		public int Length { get; set; }

		public QueueItm? Next { get; set; }

		public QueueItm(char[] array)
		{
			Array = array ?? throw new ArgumentNullException(nameof(array));
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
	}
}