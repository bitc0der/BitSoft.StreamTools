using System.IO;
using System;
using System.Text;

namespace StreamTools.Tests;

public class StringStreamTests
{
	[Test]
	public void Should_CreateEmpryStream()
	{
		// Act
		var stream = new StringStream();

		// Assert
		Assert.That(stream, Is.Not.Null);
		Assert.That(stream.Length, Is.EqualTo(0));
		Assert.That(stream.Position, Is.EqualTo(0));
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
	}

	private static string CreateString() => Guid.NewGuid().ToString();
}