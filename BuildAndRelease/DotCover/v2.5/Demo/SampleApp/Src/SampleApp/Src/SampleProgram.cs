using System;
using System.Collections.Generic;
using System.Linq;
using SampleApp.Data;
using SampleApp.Util;

namespace SampleApp
{
  public static class SampleProgram
  {
    public static void Main(string[] args)
    {
      var data = new Temperature[10];
      var random = new Random(0);
      for (int i = 0; i < data.Length; i++)
        data[i] = new Temperature {Celsius = random.NextDouble() * 100};

      Console.WriteLine(ConverDataToString(data));
      QuickSort.Sort(data);
      Console.WriteLine(ConverDataToString(data));
    }

    private static string ConverDataToString(IEnumerable<Temperature> data)
    {
      string[] celsiusStrings = data.Select(t => string.Format("{0:0.0}", t.Celsius)).ToArray();
      return string.Format("[{0}]", string.Join(", ", celsiusStrings));
    }
  }
}