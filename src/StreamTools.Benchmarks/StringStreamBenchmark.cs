using System.IO.Compression;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace StreamTools.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class StringStreamBenchmark
{
	private string? _string;

	private Stream? _internalStream;
	private Stream? _output;

	[GlobalSetup]
	public void GlobalSetup()
	{
		_string = CreateString(length: 64 * 1024 * 1024);
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

	private static string CreateString(int length)
	{
		var builder = new StringBuilder(capacity: length);

		for (var i = 0; i < length; i++)
		{
			builder.Append('A');
		}

		return builder.ToString();
	}
}