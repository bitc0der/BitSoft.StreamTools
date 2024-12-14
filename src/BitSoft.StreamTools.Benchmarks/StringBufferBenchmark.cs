using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using BitSoft.StreamTools.Benchmarks.Utils;
using BitSoft.StreamTools.Buffers;

namespace BitSoft.StreamTools.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class StringBufferWriteBenchmark
{
	private byte[]? _bytes;

	[GlobalSetup]
	public void GlobalSetup() => _bytes = Create.Buffer(length: 64 * 1024 * 1024);

	[Benchmark]
	public string StringBuilderBuffer()
	{
		using var buffer = new StringBuilderBuffer();
		return Write(buffer, _bytes!);
	}

	[Benchmark]
	public string ArrayStringBuffer()
	{
		using var buffer = new ArrayStringBuffer();
		return Write(buffer, _bytes!);
	}

	[Benchmark]
	public string MemoryStringBuffer()
	{
		using var buffer = new MemoryStringBuffer();
		return Write(buffer, _bytes!);
	}

	[Benchmark]
	public string Encoding_UTF8_GetString() => Encoding.UTF8.GetString(_bytes!);

	private static string Write(IStringBuffer buffer, byte[] bytes)
	{
		buffer.Append(bytes.AsMemory());
		return buffer.Build();
	}
}