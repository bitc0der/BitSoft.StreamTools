﻿using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using BitSoft.StreamTools.Benchmarks.Utils;

namespace BitSoft.StreamTools.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class StringStreamWriteBenchmark
{
	private byte[]? _buffer;

	[GlobalSetup]
	public void GlobalSetup()
	{
		_buffer = Create.String(length: 300 * 1024 * 1024).Compress();
	}

	[Benchmark]
	public string StringStream_WriteWithStringBuilder()
	{
		using var stream = StringStream.WriteWithStringBuilder();
		_buffer!.DecompressTo(stream);
		return stream.GetString();
	}

	[Benchmark]
	public string StringStream_WriteWithArrayPool()
	{
		using var stream = StringStream.WriteWithArrayPool();
		_buffer!.DecompressTo(stream);
		return stream.GetString();
	}

	[Benchmark]
	public string StringStream_WriteWithMemoryPool()
	{
		using var stream = StringStream.WriteWithMemoryPool();
		_buffer!.DecompressTo(stream);
		return stream.GetString();
	}

	[Benchmark]
	public string MemoryStream()
	{
		using var stream = new MemoryStream();
		_buffer!.DecompressTo(stream);
		return Encoding.UTF8.GetString(stream.GetBuffer());
	}
}