using System;

namespace SampleApp.Data
{
  /// <summary>
  /// Class allows to work with temperature in multiple temperature scales
  /// </summary>
  public class Temperature : IComparable<Temperature>, IEquatable<Temperature>
  {
    public Temperature()
    {
      Celsius = 36.6;
    }

    // Auto-properties are inlined so they will have 0 statements in the resulting coverage report
    public double Celsius { get; set; }

    public double Fahrenheit
    {
      get { return Celsius * 9 / 5 + 32; }
      set { Celsius = (value - 32) * 5 / 9; }
    }

    public double Kelvin
    {
      get { return Celsius + 273.15; }
      set { Celsius = value - 273.15; }
    }

    #region IComparable<Temperature> Members

    public int CompareTo(Temperature other)
    {
      return Celsius.CompareTo(other.Celsius);
    }

    #endregion

    #region IEquatable<Temperature> Members

    public bool Equals(Temperature other)
    {
      if (other == null)
        return false;

      return Math.Abs(Celsius - other.Celsius) < Double.Epsilon;
    }

    #endregion
  }
}