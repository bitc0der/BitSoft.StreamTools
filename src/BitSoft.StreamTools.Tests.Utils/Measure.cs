using System;
using System.Diagnostics;

namespace BitSoft.StreamTools.Tests.Utils;

public static class Measure
{
	public static IDisposable OnComplete(Action<TimeSpan> predicate)
	{
		ArgumentNullException.ThrowIfNull(predicate);

		return new MeasureAction(predicate);
	}

	private sealed class MeasureAction : IDisposable
	{
		private readonly Stopwatch _stopwatch;
		private readonly Action<TimeSpan> _predicate;

		public MeasureAction(Action<TimeSpan> predicate)
		{
			_stopwatch = Stopwatch.StartNew();
			_predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
		}

		public void Dispose()
		{
			_stopwatch.Stop();

			_predicate(_stopwatch.Elapsed);
		}
	}
}