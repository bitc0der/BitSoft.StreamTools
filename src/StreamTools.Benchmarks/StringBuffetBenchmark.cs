﻿using BenchmarkDotNet.Attributes;
using System;
using System.Diagnostics;
using System.Text;

namespace StreamTools.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class StringBuffetBenchmark
{
	private byte[]? _buffer;
	private IStringBuffer? _stringBuilderBuffer;

	[GlobalSetup]
	public void GlobalSetup()
	{
		_buffer = CreateBuffer(length: 50 * 1024 * 1024);
	}

	[IterationSetup]
	public void Init()
	{
		_stringBuilderBuffer = new StringBuilderBuffer(encoding: Encoding.UTF8);
	}

	[Benchmark]
	public string StringBuilder()
	{
		Debug.Assert(_buffer is not null);
		Debug.Assert(_stringBuilderBuffer is not null);

		_stringBuilderBuffer.Append(_buffer, offset: 0, length: _buffer.Length);

		return _stringBuilderBuffer.Build();
	}

	private static byte[] CreateBuffer(int length)
	{
		var result = new byte[length];

		var random = new Random();
		for (var i = 0; i < length; i++)
		{
			result[i] = (byte)random.Next(minValue: (int)'a', maxValue: (int)'z');
		}

		return result;
	}
}