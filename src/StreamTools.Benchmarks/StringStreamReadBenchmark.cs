using System.IO.Compression;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using StreamTools.Benchmarks.Utils;

namespace StreamTools.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class StringStreamReadBenchmark
{
	private string? _string;

	private Stream? _internalStream;
	private Stream? _output;

	[GlobalSetup]
	public void GlobalSetup()
	{
		_string = Create.String(length: 64 * 1024 * 1024);
	}

	[IterationSetup]
	public void IterationSetup()
	{
		_output = new MemoryStream();
		_internalStream = new GZipStream(_output, CompressionMode.Compress, leaveOpen: true);
	}

	[IterationCleanup]
	public void IterationCleanup()
	{
		_internalStream!.Dispose();
		_output!.Dispose();
	}

	[Benchmark]
	public void StringStream()
	{
		using var stream = new StringStream(source: _string!);
		Compress(stream);
	}

	[Benchmark]
	public void MemoryStream()
	{
		var buffer = Encoding.UTF8.GetBytes(_string!);
		using var stream = new MemoryStream(buffer: buffer);
		Compress(stream);
	}

	private void Compress(Stream stream) => stream.CopyTo(_internalStream!);
}