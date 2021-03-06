﻿using NUnit.Framework;
using SupaCharge.Core.Converter;
using SupaCharge.Testing;

namespace SupaCharge.UnitTests.Core.Converter {
  [TestFixture]
  public class ValueConverterTest : BaseTestCase {
    [Test]
    public void TestBasicConversion() {
      var converter = new ValueConverter();

      Assert.That(converter.Get<int>("3"), Is.EqualTo(3));
      Assert.That(converter.Get<string>(3), Is.EqualTo("3"));
      Assert.That(converter.Get<string>(true), Is.EqualTo("True"));
      Assert.That(converter.Get<int>(true), Is.EqualTo(1));
      Assert.That(converter.Get<int>(false), Is.EqualTo(0));
      Assert.That(converter.Get<bool>("false"), Is.EqualTo(false));
    }

    [Test]
    public void TestDefValues() {
      var converter = new ValueConverter();

      Assert.That(converter.Get(null, 3), Is.EqualTo(3));
    }
  }
}