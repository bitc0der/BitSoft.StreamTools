# StreamTools
[![stable](https://img.shields.io/nuget/v/BitSoft.StreamTools.svg?label=nuget)](https://www.nuget.org/packages/BitSoft.StreamTools/)
[![build](https://github.com/bitc0der/BitSoft.StreamTools/actions/workflows/build.yml/badge.svg)](https://github.com/bitc0der/BitSoft.StreamTools/actions/workflows/build.yml)

Yet another one library with .NET streaming helpers.

## StringStream
Simple proxy stream to obrain ability process string data as a stream:
```csharp
using Stream inputStream = new StringStream(source: "some string");
```
Or, you can write to a string:
```csharp
using Stream outputStream = new StringStream();
```

### Key features
* [High prefomance](src/StreamTools.Benchmarks/README.md) string based stream.
* Low memory allocation
* Full support of a .NET stream functionality
* Multiple buffer allocation drivers:
	* ArrayPool
	* MemoryPool
	* StringBuilder

### Example

#### Read

For example, generate stream from a string value to compress with gZip:
```csharp
using StreamTools;
using System.IO.Compression;

public static byte[] Compress(string source)
{
	ArgumentNullException.ThrowIfNull(source);

	using var inputStream = new StringStream(source);
	using var outputStream = new MemoryStream();
	using var gzipStream = new GZipStream(outputStream, CompressionMode.Compress);

	inputStream.CopyTo(gzipStream);

	return outputStream.GetBuffer();
}
```

#### Write

Decompress an input stream with gZip and convert it to a string value:

```csharp
using StreamTools;
using System.IO.Compression;

public static string Decompress(Stream sourceStream)
{
	ArgumentNullException.ThrowIfNull(source);

	using var outputStream = new StringStream();
	using var gZipStream = new GZipStream(sourceStream, CompressionMode.Decompress, leaveOpen: true);

	gZipStream.CopyTo(outputStream);

	return outputStream.GetString();
}
```

# Benchmarks

You can find [here](src/StreamTools.Benchmarks/README.md).