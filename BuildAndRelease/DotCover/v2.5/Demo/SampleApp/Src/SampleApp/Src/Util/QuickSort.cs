using System;
using System.Collections.Generic;

namespace SampleApp.Util
{
  /// <summary>
  /// Sample quick sort implementation. 
  /// </summary>
  public static class QuickSort
  {
    public static void Sort<T>(IList<T> items) where T : IComparable<T>
    {
      if (items == null)
        throw new ArgumentNullException("items");

      if (items.Count < 2)
        return;

      Sort(items, 0, items.Count - 1);
    }

    private static void Sort<T>(IList<T> items, int left, int right) where T : IComparable<T>
    {
      int i = left;
      int j = right;
      T x = items[(left + right) / 2];

      do
      {
        while (items[i].CompareTo(x) < 0 && (i < right)) i++;
        while (x.CompareTo(items[j]) < 0 && (j > left)) j--;

        if (i > j)
          continue;

        T y = items[i];
        items[i] = items[j];
        items[j] = y;
        i++;
        j--;
      } while (i <= j);

      if (left < j)
        Sort(items, left, j);
      if (i < right)
        Sort(items, i, right);
    }
  }
}