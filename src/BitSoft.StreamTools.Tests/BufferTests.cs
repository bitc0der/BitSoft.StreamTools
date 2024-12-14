using System;
using System.Collections.Generic;
using System.Text;
using BitSoft.StreamTools.Buffers;

namespace BitSoft.StreamTools.Tests;

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

		buffer.Append(buffer: bytes.AsMemory());

		// Assert
		var result = buffer.Build();

		Assert.That(result, Is.Not.Null);
		Assert.That(result, Is.EqualTo(str));
	}

	private static IEnumerable<TestCaseData> GetBuffers()
	{
		yield return new TestCaseData(() => new StringBuilderBuffer());
		yield return new TestCaseData(() => new ArrayPoolStringBuffer());
		yield return new TestCaseData(() => new MemoryPoolStringBuffer());
	}

	private enum Buffer
	{
		StringBuilder,
		ArrayPool,
		MemoryPool
	}
}