using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Projekt2;

namespace Tests
{
    [TestClass]
    public class FractionTests
    {
        [TestMethod]
        public void AddFractionTest()
        {
            Fraction firstFraction = new Fraction(1, 5);
            Fraction secondFraction = new Fraction(2, 10);
            Fraction addResult = firstFraction + secondFraction;

            Assert.AreEqual(new BigInteger(2), addResult.Numerator);
            Assert.AreEqual(new BigInteger(5), addResult.Denominator);
        }

        [TestMethod]
        public void SubstractFractionTest()
        {
            Fraction firstFraction = new Fraction(4, 5);
            Fraction secondFraction = new Fraction(2, 10);
            Fraction substractResult = firstFraction - secondFraction;

            Assert.AreEqual(new BigInteger(3), substractResult.Numerator);
            Assert.AreEqual(new BigInteger(5), substractResult.Denominator);
        }

        [TestMethod]
        public void MultiplyFractionTest()
        {
            Fraction firstFraction = new Fraction(4, 5);
            Fraction secondFraction = new Fraction(2, 10);
            Fraction multiplyResult = firstFraction * secondFraction;

            Assert.AreEqual(new BigInteger(4), multiplyResult.Numerator);
            Assert.AreEqual(new BigInteger(25), multiplyResult.Denominator);
        }

        [TestMethod]
        public void DivideFractionTest()
        {
            Fraction firstFraction = new Fraction(4, 5);
            Fraction secondFraction = new Fraction(2, 10);
            Fraction divideResult = firstFraction / secondFraction;

            Assert.AreEqual(new BigInteger(4), divideResult.Numerator);
            Assert.AreEqual(new BigInteger(1), divideResult.Denominator);
        }
    }
}
