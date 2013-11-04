using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SampleApp.Data;

namespace SampleApp.Util.Tests
{
  public class QuickSortTests
  {
    [Test]
    public void TestOnEmptyDataSet()
    {
      var data = new List<int>();
      QuickSort.Sort(data);
      Assert.AreEqual(0, data.Count);
    }

    [Test]
    [ExpectedException(typeof (ArgumentNullException))]
    public void TestOnNullDataSet()
    {
      QuickSort.Sort<int>(null);
    }

    [Test]
    public void TestOnMeaningfulDataSet()
    {
      var data = new Temperature[1000];
      var random = new Random(0);
      for (int i = 0; i < data.Length; i++)
        data[i] = new Temperature {Celsius = random.NextDouble() * 100};

      var actualResult = new List<Temperature>(data);
      QuickSort.Sort(actualResult);

      var expectedResult = new List<Temperature>(data);
      expectedResult.Sort();

      Assert.That(expectedResult.SequenceEqual(actualResult));
    }
  }
}