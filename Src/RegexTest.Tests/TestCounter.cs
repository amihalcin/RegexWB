using System;
using NUnit.Framework;
using Timing;

namespace RegexWorkbench.Tests
{
	[TestFixture]
	public class TestCounter
	{
		[Test]
		public void TestCounter_Construction_DoesNotResultInAnyElapsedTime()
		{
			Counter c = new Counter(new MockPerformanceCounter(100, 1));
			Assert.AreEqual(0.0f, c.Seconds);
		}

		[Test]
		public void TestCounter_StartOnly_DoesNotResultInAnyElapsedTime()
		{
			Counter c = new Counter(new MockPerformanceCounter(100, 1));
			c.Start();
			Assert.AreEqual(0.0f, c.Seconds);
		}

		[Test]
		public void TestCounter_StartAndStopAtSameTime_DoesNotResultInAnyElapsedTime()
		{
			Counter c = new Counter(new MockPerformanceCounter(100, 1));
			c.Start();
			c.Stop();
			Assert.AreEqual(0.0f, c.Seconds);
		}

		[Test]
		public void TestCounter_StartAndStop_ElapsedTimeCorrect()
		{
			const long initialValue = 100;
			const long frequency = 1;

			var perfCounter = new MockPerformanceCounter(initialValue, frequency);
			Counter c = new Counter(perfCounter);
			c.Start();
			perfCounter.Value = initialValue + 2;
			c.Stop();

			Assert.AreEqual(2.0f, c.Seconds);
		}

		[Test]
		public void TestCounter_StartAndStopTwice_ElapsedTimeCorrect()
		{
			const long initialValue = 100;
			const long frequency = 1;

			var perfCounter = new MockPerformanceCounter(initialValue, frequency);
			Counter c = new Counter(perfCounter);
			c.Start();
			perfCounter.Value = initialValue + 2;
			c.Stop();

			perfCounter.Value = initialValue + 5;
			c.Start();
			perfCounter.Value = initialValue + 6;
			c.Stop();

			Assert.AreEqual(3.0f, c.Seconds);
		}

		private class MockPerformanceCounter : IPerformanceCounter
		{
			public long Value { get; set; }

			private long frequency;

			public MockPerformanceCounter(long currentValue, long frequency)
			{
				Value = currentValue;
				this.frequency = frequency;
			}

			public long GetValue()
			{
				return Value;
			}

			public long GetFrequency()
			{
				return frequency;
			}
		}
	}
}
