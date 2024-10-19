using BenchmarkDotNet.Attributes;
using StreamTools.Benchmarks.Utils;
using System.IO;
using System.IO.Compression;
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
		_buffer = Compress(Create.String(length: 64 * 1024 * 1024));
	}

	[Benchmark]
	public string StrinbStream()
	{
		using var stream = new StringStream();
		Decompress(output: stream, data: _buffer!);
		return stream.GetString();
	}

	[Benchmark]
	public string StrinbStream_ArrayPool()
	{
		using var stream = new StringStream(() => new ArrayStringBuffer());
		Decompress(output: stream, data: _buffer!);
		return stream.GetString();
	}

	[Benchmark]
	public string StrinbStream_MemoeryPool()
	{
		using var stream = new StringStream(() => new MemoryStringBuffer());
		Decompress(output: stream, data: _buffer!);
		return stream.GetString();
	}

	[Benchmark]
	public string MemoryStream()
	{
		using var stream = new MemoryStream();
		Decompress(output: stream, data: _buffer!);
		return Encoding.UTF8.GetString(stream.GetBuffer());
	}

	private static byte[] Compress(string source)
	{
		using var inputStream = new StringStream(source: source);
		using var outputStream = new MemoryStream();
		using var gZipStream = new GZipStream(outputStream, CompressionMode.Compress, leaveOpen: true);

		inputStream.CopyTo(gZipStream);

		return outputStream.GetBuffer();
	}

	private static void Decompress(Stream output, byte[] data)
	{
		using var sourceStream = new MemoryStream(data);
		using var gZipStream = new GZipStream(sourceStream, CompressionMode.Decompress, leaveOpen: true);
		gZipStream.CopyTo(output);
	}	
}