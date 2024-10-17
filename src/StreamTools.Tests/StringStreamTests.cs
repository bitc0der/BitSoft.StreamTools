using System.IO;
using System;
using System.Text;

namespace StreamTools.Tests;

public class StringStreamTests
{
	[Test]
	public void Should_ReadStringStream()
	{
		// Arrange
		var str = CreateString();

		using var sr = new StringStream(str);
		using var reader = new StreamReader(sr);

		// Act
		var result = reader.ReadToEnd();

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result, Is.EqualTo(str));
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
	}

	private static string CreateString() => Guid.NewGuid().ToString();
}