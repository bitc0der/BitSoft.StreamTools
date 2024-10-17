using BenchmarkDotNet.Attributes;
using StreamTools.Benchmarks.Utils;
using System;
using System.Text;

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
	public string StringBuilder()
	{
		using var buffer = new StringBuilderBuffer(encoding: Encoding.UTF8);
		buffer.Append(_buffer!, offset: 0, length: _buffer!.Length);
		return buffer.Build();
	}

	[Benchmark]
	public string ArrayBuilder()
	{
		using var buffer = new ArrayStringBuffer();
		buffer.Append(_buffer!, offset: 0, length: _buffer!.Length);
		return buffer.Build();
	}
}