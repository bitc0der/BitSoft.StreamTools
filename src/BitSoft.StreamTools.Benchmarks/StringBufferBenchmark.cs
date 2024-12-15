using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using BitSoft.StreamTools.Buffers;
using BitSoft.StreamTools.Tests.Utils;

namespace BitSoft.StreamTools.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class StringBufferWriteBenchmark
{
	private byte[]? _bytes;

	[GlobalSetup]
	public void GlobalSetup() => _bytes = Create.Buffer(length: 300 * 1024 * 1024);

	[Benchmark]
	public string StringBuilderBuffer()
	{
		using var buffer = new StringBuilderBuffer();
		return Write(buffer, _bytes!);
	}

	[Benchmark]
	public string ArrayStringBuffer()
	{
		using var buffer = new ArrayPoolStringBuffer();
		return Write(buffer, _bytes!);
	}

	[Benchmark]
	public string MemoryStringBuffer()
	{
		using var buffer = new MemoryPoolStringBuffer();
		return Write(buffer, _bytes!);
	}

	[Benchmark]
	public string ArrayPoolQueueStringBuffer()
	{
		using var buffer = new ArrayPoolQueueStringBuffer();
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