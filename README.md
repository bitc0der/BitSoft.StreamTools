# StreamTools
Yet another one library with .NET streaming helpers.

## StringStream
Simple proxy stream to obrain ability process string data as a stream:
```csharp
using Stream stringStream = new StringStream(source: "some string");
```

### Example

For example, generate stream from a string value to compress with GZip:
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