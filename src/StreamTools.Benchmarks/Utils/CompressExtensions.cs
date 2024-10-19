using System;
using System.IO.Compression;
using System.IO;

namespace StreamTools.Benchmarks.Utils;

internal static class CompressExtensions
{
	public static byte[] Compress(this string source)
	{
		ArgumentNullException.ThrowIfNull(source);

		using var inputStream = new StringStream(source: source);
		using var outputStream = new MemoryStream();
		using var gZipStream = new GZipStream(outputStream, CompressionMode.Compress, leaveOpen: true);

		inputStream.CopyTo(gZipStream);

		return outputStream.GetBuffer();
	}

	public static void DecompressTo(this byte[] data, Stream output)
	{
		ArgumentNullException.ThrowIfNull(data);

		using var sourceStream = new MemoryStream(data);
		using var gZipStream = new GZipStream(sourceStream, CompressionMode.Decompress, leaveOpen: true);
		gZipStream.CopyTo(output);
	}
}
