using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using BitSoft.StreamTools.Tests.Utils;

namespace BitSoft.StreamTools.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class StringStreamWriteBenchmark
{
	private string? _source;
	private byte[]? _buffer;

	[GlobalSetup]
	public void GlobalSetup()
	{
		_source = Create.String(length: 300 * 1024 * 1024);
		_buffer = Encoding.UTF8.GetBytes(_source);
	}

	[Benchmark]
	public string StringStream_WriteWithStringBuilder()
	{
		using var stream = StringStream.WriteWithStringBuilder();
		stream.Write(_buffer);
		return stream.GetString();
	}

	[Benchmark]
	public string StringStream_WriteWithArrayPool()
	{
		using var stream = StringStream.WriteWithArrayPool();
		stream.Write(_buffer);
		return stream.GetString();
	}

	[Benchmark]
	public string StringStream_WriteWithArrayPoolQueued()
	{
		using var stream = StringStream.WriteWithArrayPoolQueue();
		stream.Write(_buffer);
		return stream.GetString();
	}

	[Benchmark]
	public string StringStream_WriteWithMemoryPool()
	{
		using var stream = StringStream.WriteWithMemoryPool();
		stream.Write(_buffer);
		return stream.GetString();
	}

	[Benchmark]
	public string MemoryStream()
	{
		using var stream = new MemoryStream();
		stream.Write(_buffer);
		return Encoding.UTF8.GetString(stream.GetBuffer());
	}
}