using System;

namespace StreamTools.Buffers;

public interface IStringBuffer : IDisposable
{
	int Length { get; }

	void Append(byte[] buffer, int offset, int length);

	string Build();
}