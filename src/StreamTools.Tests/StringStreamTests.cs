using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StreamTools.Tests;

public class StringStreamTests
{
	[Test]
	public void Should_CreateEmpryStreamToRead()
	{
		// Arrange
		const string value = "string";

		// Act
		using var stream = new StringStream(source: value);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(stream, Is.Not.Null);
			Assert.That(stream.Length, Is.EqualTo(value.Length));
			Assert.That(stream.Position, Is.EqualTo(0));
			Assert.That(stream.CanRead, Is.EqualTo(true));
			Assert.That(stream.CanSeek, Is.EqualTo(true));
			Assert.That(stream.CanWrite, Is.EqualTo(false));
		});

		var result = stream.GetString();

		Assert.That(result, Is.EqualTo(value));
	}

	[Test]
	public void Should_Throw_When_ReadMode()
	{
		// Arrange
		const string value = "string";

		// Act
		using var stream = new StringStream(source: value);

		// Assert
		Assert.Throws<InvalidOperationException>(() => stream.Write([1, 2, 3]));
	}

	[Test]
	public void Should_CreateEmpryStreamToWrite()
	{
		// Act
		using var stream = new StringStream();

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(stream, Is.Not.Null);
			Assert.That(stream.Length, Is.EqualTo(0));
			Assert.That(stream.Position, Is.EqualTo(0));
			Assert.That(stream.CanRead, Is.EqualTo(false));
			Assert.That(stream.CanSeek, Is.EqualTo(false));
			Assert.That(stream.CanWrite, Is.EqualTo(true));
		});

		var buffer = new byte[8];

		Assert.Throws<InvalidOperationException>(() => stream.ReadExactly(buffer.AsSpan()));
		Assert.Throws<InvalidOperationException>(() => stream.Position = 1);
	}

	[Test]
	public void Should_ReadStringStream()
	{
		// Arrange
		var str = CreateString();

		using var stream = new StringStream(str);
		using var reader = new StreamReader(stream);

		// Act
		var result = reader.ReadToEnd();

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.EqualTo(str));
			Assert.That(stream.Length, Is.EqualTo(str.Length));
			Assert.That(stream.Position, Is.EqualTo(str.Length));
			Assert.That(stream.CanRead, Is.EqualTo(false));
			Assert.That(stream.CanSeek, Is.EqualTo(true));
			Assert.That(stream.CanWrite, Is.EqualTo(false));
		});
	}

	[Test]
	public async Task Should_ReadString_When_Async()
	{
		// Arrange
		var str = CreateString();

		using var stream = new StringStream(str);
		using var reader = new StreamReader(stream);

		// Act
		var result = await reader.ReadToEndAsync();

		// Assert
		Assert.That(result, Is.EqualTo(str));
	}

	[Test]
	public void Should_DoubleReadStringStream()
	{
		// Arrange
		var str = CreateString();

		using var stream = new StringStream(str);
		using var reader = new StreamReader(stream);

		// Act
		var result1 = reader.ReadToEnd();
		stream.Position = 0;
		var result2 = reader.ReadToEnd();

		// Assert
		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result1, Is.EqualTo(str));
			Assert.That(result2, Is.EqualTo(str));
		});
	}

	[Test]
	public void Should_DoubleReadStringStream_When_SeekFromBegin()
	{
		// Arrange
		var str = CreateString();

		using var stream = new StringStream(str);
		using var reader = new StreamReader(stream);

		// Act
		var result1 = reader.ReadToEnd();
		stream.Seek(0, SeekOrigin.Begin);
		var result2 = reader.ReadToEnd();

		// Assert
		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result1, Is.EqualTo(str));
			Assert.That(result2, Is.EqualTo(str));
		});
	}

	[Test]
	public void Should_DoubleReadStringStream_When_SeekFromEnd()
	{
		// Arrange
		var str = CreateString();

		using var stream = new StringStream(str);
		using var reader = new StreamReader(stream);

		// Act
		var result1 = reader.ReadToEnd();
		stream.Seek(stream.Length, SeekOrigin.End);
		var result2 = reader.ReadToEnd();

		// Assert
		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result1, Is.EqualTo(str));
			Assert.That(result2, Is.EqualTo(str));
		});
	}

	[Test]
	public void Should_DoubleReadHalfStringStream()
	{
		// Arrange
		var str = CreateString();

		using var stream = new StringStream(str);
		using var reader = new StreamReader(stream);

		// Act
		var result1 = reader.ReadToEnd();
		stream.Seek(str.Length / 2, SeekOrigin.Begin);
		var result2 = reader.ReadToEnd();

		// Assert
		Assert.Multiple(() =>
		{
			// Assert
			Assert.That(result1, Is.EqualTo(str));
			Assert.That(result2, Is.EqualTo(str.Substring(startIndex: str.Length / 2)));
		});
	}

	[Test]
	public void Should_WriteStringStream()
	{
		// Arrange
		var sourceString = CreateString();
		var buffer = Encoding.UTF8.GetBytes(sourceString);

		using var stream = new StringStream();

		// Act
		stream.Write(buffer, offset: 0, count: buffer.Length);

		// Assert
		var result = stream.GetString();

		Assert.Multiple(() =>
		{
			Assert.That(result, Is.EqualTo(sourceString));
			Assert.That(stream.Length, Is.EqualTo(result.Length));
			Assert.That(stream.Position, Is.EqualTo(result.Length));
			Assert.That(stream.CanRead, Is.EqualTo(false));
			Assert.That(stream.CanSeek, Is.EqualTo(false));
			Assert.That(stream.CanWrite, Is.EqualTo(true));
		});
	}

	[Test]
	public void Should_Throw_When_WriteMode()
	{
		// Arrange
		using var stream = StringStream.WithArrayPool();

		// Act & Assert
		Assert.Throws<InvalidOperationException>(() => stream.Position = 1);
	}

	private static string CreateString() => Guid.NewGuid().ToString();
}