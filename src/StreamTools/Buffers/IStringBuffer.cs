using System;

namespace StreamTools.Buffers;

public interface IStringBuffer : IDisposable
{
	int Length { get; }

	void Append(ReadOnlyMemory<byte> buffer);

	string Build();
}