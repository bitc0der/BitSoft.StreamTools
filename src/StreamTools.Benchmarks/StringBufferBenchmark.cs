using BenchmarkDotNet.Attributes;
using StreamTools.Benchmarks.Utils;

namespace StreamTools.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class StringBufferBenchmark
{
	private byte[]? _buffer;

	[GlobalSetup]
	public void GlobalSetup()
	{
		_buffer = Create.Buffer(length: 64 * 1024 * 1024);
	}

	[Benchmark]
	public string StringBuilderBuffer()
	{
		using var buffer = new StringBuilderBuffer();
		buffer.Append(_buffer!, offset: 0, length: _buffer!.Length);
		return buffer.Build();
	}

	[Benchmark]
	public string ArrayStringBuffer()
	{
		using var buffer = new ArrayStringBuffer();
		buffer.Append(_buffer!, offset: 0, length: _buffer!.Length);
		return buffer.Build();
	}

	[Benchmark]
	public string MemoryStringBuffer()
	{
		using var buffer = new MemoryStringBuffer();
		buffer.Append(_buffer!, offset: 0, length: _buffer!.Length);
		return buffer.Build();
	}
}