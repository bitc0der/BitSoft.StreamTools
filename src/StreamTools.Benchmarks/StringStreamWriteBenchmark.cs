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
	public string StrinbStream()
	{
		using var stream = new StringStream();
		_buffer!.DecompressTo(stream);
		return stream.GetString();
	}

	[Benchmark]
	public string StrinbStream_ArrayPool()
	{
		using var stream = new StringStream(() => new ArrayStringBuffer());
		_buffer!.DecompressTo(stream);
		return stream.GetString();
	}

	[Benchmark]
	public string StrinbStream_MemoeryPool()
	{
		using var stream = new StringStream(() => new MemoryStringBuffer());
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