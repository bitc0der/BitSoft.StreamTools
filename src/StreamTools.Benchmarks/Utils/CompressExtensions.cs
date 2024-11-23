using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace StreamTools.Benchmarks.Utils;

internal static class CompressExtensions
{
	public static byte[] Compress(this string source, Encoding? encoding = null)
	{
		ArgumentNullException.ThrowIfNull(source);

		using var inputStream = StringStream.Read(source: source, encoding);
		using var outputStream = new MemoryStream();
		using var gZipStream = new GZipStream(outputStream, CompressionMode.Compress, leaveOpen: true);

		inputStream.CopyTo(gZipStream);

		return outputStream.GetBuffer();
	}

	public static void DecompressTo(this byte[] data, Stream output)
	{
		ArgumentNullException.ThrowIfNull(data);
		ArgumentNullException.ThrowIfNull(output);

		using var sourceStream = new MemoryStream(data);
		using var gZipStream = new GZipStream(sourceStream, CompressionMode.Decompress, leaveOpen: true);
		gZipStream.CopyTo(output);
	}
}
