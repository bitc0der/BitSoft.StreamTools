using System;

namespace StreamTools;

public interface IStringBuffer : IDisposable
{
	void Append(byte[] buffer, int offset, int length);

	string Build();
}