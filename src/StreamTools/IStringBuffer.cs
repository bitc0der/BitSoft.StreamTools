using System;

namespace StreamTools;

public interface IStringBuffer : IDisposable
{
	int Length { get; }

	void Append(byte[] buffer, int offset, int length);

	string Build();
}