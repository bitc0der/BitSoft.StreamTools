namespace BitSoft.StreamTools.Buffers;

public sealed class ArrayPoolQueueOptions
{
	public static ArrayPoolQueueOptions Default { get; } = new();

	public int BufferSize { get; set; } = 1024 * 1024;

	public int MaxBufferSize { get; set; } = 128 * 1024 * 1024;

	public int Multipler { get; set; } = 4;

	public int Step { get; set; } = 10;
}