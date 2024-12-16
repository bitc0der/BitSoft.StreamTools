using System;
using System.Text;

namespace BitSoft.StreamTools.Tests.Utils;

public static class Create
{
	public static string String(int length)
	{
		var builder = new StringBuilder(capacity: length);

		var random = new Random();
		for (var i = 0; i < length; i++)
		{
			builder.Append(random.Next(minValue: 'a', maxValue: 'z'));
		}

		return builder.ToString();
	}

	public static byte[] Buffer(int length)
	{
		var result = new byte[length];

		const int min = 'a';
		const int max = 'z';

		var random = new Random();
		for (var i = 0; i < length; i++)
		{
			result[i] = (byte)random.Next(minValue: min, maxValue: max);
		}
		return result;
	}
}