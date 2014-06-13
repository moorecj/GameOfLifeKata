using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using StatsdClient;
using GameOfLifeKata;

namespace GameOfLifeKataTests
{
    public class FakeStatsdClient : IStatsd
    {
        private List<String> metrics = new List<String>();

        public void LogCount(String name, Int32 count = 1)
        {
            metrics.Add(name);
        }

        public void LogGauge(String name, Int32 value)
        {
            throw new NotImplementedException();
        }

        public void LogRaw(String name, Int32 value, Int64? epoch = null)
        {
            throw new NotImplementedException();
        }

        public void LogSet(String name, Int32 value)
        {
            throw new NotImplementedException();
        }

        public void LogTiming(String name, Int64 milliseconds)
        {
            throw new NotImplementedException();
        }

        public void LogTiming(String name, Int32 milliseconds)
        {
            throw new NotImplementedException();
        }

        public void AssertNothingLogged()
        { }

        public void AssertDeathsLogged(Int32 p)
        {
            Assert.That(metrics.Count(m => m == GameOfLife.DEATH_METRIC), Is.EqualTo(p));
        }

        internal void AssertBirthsLogged(Int32 p)
        {
            Assert.That(metrics.Count(m => m == GameOfLife.BIRTH_METRIC), Is.EqualTo(p));
        }
    }
}
