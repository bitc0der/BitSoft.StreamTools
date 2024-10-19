using BenchmarkDotNet.Attributes;
using StreamTools.Benchmarks.Utils;
using System.IO;
using System.Text;

namespace StreamTools.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class StringStreamWriteBenchmark
{
	private byte[]? _buffer;

	[GlobalSetup]
	public void GlobalSetup()
	{
		_buffer = Create.String(length: 64 * 1024 * 1024).Compress();
	}

	[Benchmark]
	public string StringStream_WithStringBuilder()
	{
		using var stream = StringStream.WithStringBuilder();
		_buffer!.DecompressTo(stream);
		return stream.GetString();
	}

	[Benchmark]
	public string StringStream_WithArrayPool()
	{
		using var stream = StringStream.WithArrayPool();
		_buffer!.DecompressTo(stream);
		return stream.GetString();
	}

	[Benchmark]
	public string StringStream_WithMemoryPool()
	{
		using var stream = StringStream.WithMemoryPool();
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