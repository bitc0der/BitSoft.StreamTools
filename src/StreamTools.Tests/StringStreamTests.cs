using System.IO;
using System;

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

	private static string CreateString() => Guid.NewGuid().ToString();
}