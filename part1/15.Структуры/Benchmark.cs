using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
    {
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            var watch = new Stopwatch();
            task.Run();
            GC.Collect();                   // Эти две строчки нужны, чтобы уменьшить вероятность того,
            GC.WaitForPendingFinalizers();  // что Garbadge Collector вызовется в середине измерений
                                            // и как-то повлияет на них.
            watch.Start();
            for (int i = 0; i < repetitionCount; i++)
            {
                task.Run();
            }
            watch.Stop();
            return watch.Elapsed.TotalMilliseconds / repetitionCount;
        }

        public class BuildStringByBuilder : ITask
        {
            public int StringLength { get; }

            public BuildStringByBuilder(int stringLength)
            {
                this.StringLength = stringLength;
            }

            public void Run()
            {
                BuildString(this.StringLength);
            }

            public string BuildString(int countOfSymbols)
            {
                var builder = new StringBuilder();
                for (int i = 0; i < countOfSymbols; i++)
                {
                    builder.Append('a');
                }
                return builder.ToString();
            }
        }


        public class BuildStringByFunction : ITask
        {
            public int StringLength { get; }

            public void Run()
            {
                BuildString(StringLength);
            }

            public string BuildString(int countOfSymbols)
            {
                return new string('a', countOfSymbols);
            }

            public BuildStringByFunction(int stringLength)
            {
                this.StringLength = stringLength;
            }
        }
    }


    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var benchmark = new Benchmark();
            var stringLength = 10000;
            var countOfRepeats = 200;
            var builderTask = new Benchmark.BuildStringByBuilder(stringLength);
            var functionTask = new Benchmark.BuildStringByFunction(stringLength);

            var builderTime = benchmark.MeasureDurationInMs(builderTask, countOfRepeats);
            var functionTime = benchmark.MeasureDurationInMs(functionTask, countOfRepeats);

            Assert.Less(functionTime, builderTime);
        }
    }
}