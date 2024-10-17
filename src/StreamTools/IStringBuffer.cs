namespace StreamTools;

public interface IStringBuffer
{
	void Append(byte[] chars, int offset, int length);

	string Build();
}