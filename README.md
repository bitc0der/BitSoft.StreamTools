# StreamTools
Yet another one library with .NET streaming helpers.

## StringStream
Simple proxy stream to obrain ability process string data as a stream:
```csharp
using Stream stringStream = new StringStream(source: "some string");
```

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

public static string Decompress(Stream source)
{
	ArgumentNullException.ThrowIfNull(source);

	using var outputStream = new StringStream();
	using var gZipStream = new GZipStream(sourceStream, CompressionMode.Decompress, leaveOpen: true);

	gZipStream.CopyTo(outputStream);

	return outputStream.GetString();
}
```