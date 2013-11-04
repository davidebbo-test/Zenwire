using NUnit.Framework;

namespace SampleApp.Data.Tests
{
  [TestFixture]
  public class TemperatureTests
  {
    [TestCase(0, 32)]
    [TestCase(40, 104)]
    [TestCase(100, 212)]
    [TestCase(36.6, 97.88)]
    public void TestCelsiusToFahrenheitConversion(double celsius, double fahrenheit)
    {
      var temperature = new Temperature {Celsius = celsius};
      Assert.AreEqual(fahrenheit, temperature.Fahrenheit, 0.001);

      temperature.Fahrenheit = fahrenheit;
      Assert.AreEqual(celsius, temperature.Celsius, 0.001);
    }

    [TestCase(-273.15, 0)]
    [TestCase(642, 915.15)]
    public void TestCelsiusToKelvinConversion(double celsius, double kelvin)
    {
      var temperature = new Temperature {Celsius = celsius};
      Assert.AreEqual(kelvin, temperature.Kelvin, 0.001);

      temperature.Kelvin = kelvin;
      Assert.AreEqual(celsius, temperature.Celsius, 0.001);
    }

    [TestCase(123.4, -237.55)]
    [TestCase(310.95, 100.04)]
    [TestCase(0, -459.67)]
    public void TestKelvinToFahrenheitConversion(double kelvin, double fahrenheit)
    {
      var temperature = new Temperature {Kelvin = kelvin};
      Assert.AreEqual(fahrenheit, temperature.Fahrenheit, 0.001);

      temperature.Fahrenheit = fahrenheit;
      Assert.AreEqual(kelvin, temperature.Kelvin, 0.001);
    }
  }
}