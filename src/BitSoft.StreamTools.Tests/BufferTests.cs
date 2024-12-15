using System;
using System.Collections.Generic;
using System.Text;
using BitSoft.StreamTools.Buffers;
using BitSoft.StreamTools.Tests.Utils;

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

	[TestCaseSource(nameof(GetBuffers)), Ignore("Skip")]
	public void Should(Func<IStringBuffer> func)
	{
		// Arrange
		var data = Create.Buffer(length: 300 * 1024 * 1024);
		using var buffer = func();

		// Act
		using (Measure.OnComplete(t => Console.WriteLine($"APD: {t}")))
		{
			buffer.Append(data.AsMemory());
		}
		using (Measure.OnComplete(t => Console.WriteLine($"BLD: {t}")))
		{
			var result = buffer.Build();
		}

		// Assert
		// Assert.That(result, Is.Not.Null);
	}

	private static IEnumerable<TestCaseData> GetBuffers()
	{
		yield return new TestCaseData(() => new StringBuilderBuffer());
		yield return new TestCaseData(() => new ArrayPoolStringBuffer());
		yield return new TestCaseData(() => new MemoryPoolStringBuffer());
		yield return new TestCaseData(() => new ArrayPoolQueueStringBuffer());
	}

	private enum Buffer
	{
		StringBuilder,
		ArrayPool,
		MemoryPool
	}
}