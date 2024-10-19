using System.IO;
using System;
using System.Text;

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
		Assert.That(stream, Is.Not.Null);
		Assert.That(stream.Length, Is.EqualTo(value.Length));
		Assert.That(stream.Position, Is.EqualTo(0));
		Assert.That(stream.CanRead, Is.EqualTo(true));
		Assert.That(stream.CanSeek, Is.EqualTo(true));
		Assert.That(stream.CanWrite, Is.EqualTo(false));

		Assert.Throws<InvalidOperationException>(() => stream.Write([1, 2, 3]));
		Assert.Throws<NotSupportedException>(() => stream.Seek(offset: 1, SeekOrigin.Begin));

		var result = stream.GetString();

		Assert.That(result, Is.EqualTo(value));
	}

	[Test]
	public void Should_CreateEmpryStreamToWrite()
	{
		// Act
		using var stream = new StringStream();

		// Assert
		Assert.That(stream, Is.Not.Null);
		Assert.That(stream.Length, Is.EqualTo(0));
		Assert.That(stream.Position, Is.EqualTo(0));
		Assert.That(stream.CanRead, Is.EqualTo(false));
		Assert.That(stream.CanSeek, Is.EqualTo(false));
		Assert.That(stream.CanWrite, Is.EqualTo(true));

		var buffer = new byte[8];

		Assert.Throws<InvalidOperationException>(() => stream.Read(buffer.AsSpan()));
		Assert.Throws<InvalidOperationException>(() => stream.Position = 1);

		Assert.Throws<NotSupportedException>(() => stream.Seek(offset: 1, SeekOrigin.Begin));
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
		Assert.That(result, Is.Not.Null);
		Assert.That(result, Is.EqualTo(str));
		Assert.That(stream.Length, Is.EqualTo(str.Length));
		Assert.That(stream.Position, Is.EqualTo(str.Length));
		Assert.That(stream.CanRead, Is.EqualTo(false));
		Assert.That(stream.CanSeek, Is.EqualTo(true));
		Assert.That(stream.CanWrite, Is.EqualTo(false));

		Assert.Throws<NotSupportedException>(() => stream.Seek(offset: 1, SeekOrigin.Begin));
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
		Assert.That(result1, Is.EqualTo(str));
		Assert.That(result2, Is.EqualTo(str));
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

		Assert.That(result, Is.EqualTo(sourceString));
		Assert.That(stream.Length, Is.EqualTo(result.Length));
		Assert.That(stream.Position, Is.EqualTo(result.Length));
		Assert.That(stream.CanRead, Is.EqualTo(false));
		Assert.That(stream.CanSeek, Is.EqualTo(false));
		Assert.That(stream.CanWrite, Is.EqualTo(true));

		Assert.Throws<InvalidOperationException>(() => stream.Position = 1);
		Assert.Throws<NotSupportedException>(() => stream.Seek(offset: 1, SeekOrigin.Begin));
	}

	private static string CreateString() => Guid.NewGuid().ToString();
}