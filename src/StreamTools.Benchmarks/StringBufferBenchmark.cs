using BenchmarkDotNet.Attributes;
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
		_buffer = CreateBuffer(length: 64 * 1024 * 1024);
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

	private static byte[] CreateBuffer(int length)
	{
		var result = new byte[length];

		const int min = (int)'a';
		const int max = (int)'z';

		var random = new Random();
		for (var i = 0; i < length; i++)
		{
			result[i] = (byte)random.Next(minValue: min, maxValue: max);
		}
		return result;
	}
}