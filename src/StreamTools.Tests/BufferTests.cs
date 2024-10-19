using StreamTools.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamTools.Tests;

[TestFixture]
public sealed class BufferTests
{
	[TestCaseSource(nameof(GetBuffers))]
	public void Should_CreateBuffer(Func<IStringBuffer> func)
	{
		// Act
		using var buffer = func();

		// Assert
		Assert.That(buffer, Is.Not.Null);
		Assert.That(buffer.Length, Is.EqualTo(0));
	}

	[TestCaseSource(nameof(GetBuffers))]
	public void Should_WriteData(Func<IStringBuffer> func)
	{
		// Arrange
		var str = Guid.NewGuid().ToString();
		var bytes = Encoding.UTF8.GetBytes(str);

		// Act
		using var buffer = func();

		buffer.Append(buffer: bytes, offset: 0, length: bytes.Length);

		// Assert
		var result = buffer.Build();

		Assert.That(result, Is.Not.Null);
		Assert.That(result, Is.EqualTo(str));
	}

	private static IEnumerable<TestCaseData> GetBuffers()
	{
		yield return new TestCaseData(() => new StringBuilderBuffer());
		yield return new TestCaseData(() => new ArrayStringBuffer());
		yield return new TestCaseData(() => new MemoryStringBuffer());
	}

	private enum Buffer
	{
		StringBuilder,
		ArrayPool,
		MemoryPool
	}
}