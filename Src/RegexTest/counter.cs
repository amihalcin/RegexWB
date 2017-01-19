using System;

namespace Timing
{
	public class Counter
	{
		private readonly IPerformanceCounter counter;
		private long elapsedCount;
		private long startCount;

		public Counter()
			: this(new KernelPerformanceCounterValue())
		{ }

		internal Counter(IPerformanceCounter counter)
		{
			this.counter = counter;
			this.elapsedCount = 0;
			this.startCount = 0;
		}

		public void Start()
		{
			startCount = counter.GetValue();
		}

		public void Stop()
		{
			elapsedCount += (counter.GetValue() - startCount);
		}

		public void Clear()
		{
			elapsedCount = 0;
		}

		public float Seconds
		{
			get
			{
				return ((float)elapsedCount / (float)counter.GetFrequency());
			}
		}

		public override string ToString()
		{
			return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} seconds", Seconds);
		}
	}

	internal interface IPerformanceCounter
	{
		long GetValue();
		long GetFrequency();
	}

	internal sealed class KernelPerformanceCounterValue : IPerformanceCounter
	{
		public long GetValue()
		{
			long value = 0;
			if (!QueryPerformanceCounter(ref value))
			{
				throw new NotSupportedException("Unable to call QueryPerformanceCounter");
			}

			return value;
		}

		public long GetFrequency()
		{
			long freq = 0;
			if (!QueryPerformanceFrequency(ref freq))
			{
				throw new NotSupportedException("Unable to call QueryPerformanceFrequency");
			}

			return freq;
		}


		[System.Runtime.InteropServices.DllImport("KERNEL32")]
		private static extern bool QueryPerformanceCounter(ref long lpPerformanceCount);

		[System.Runtime.InteropServices.DllImport("KERNEL32")]
		private static extern bool QueryPerformanceFrequency(ref long lpFrequency);
	}
}
