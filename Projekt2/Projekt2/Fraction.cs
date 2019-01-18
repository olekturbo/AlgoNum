using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Projekt2
{
    public class Fraction
    {

        public Fraction(BigInteger numerator, BigInteger denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public static Fraction operator +(Fraction frac1, Fraction frac2)
        {
            return (Add(frac1, frac2));
        }

        public static Fraction operator -(Fraction frac1, Fraction frac2)
        {
            return (Substract(frac1, frac2));
        }

        public static Fraction operator *(Fraction frac1, Fraction frac2)
        {
            return (Multiply(frac1, frac2));
        }

        public static Fraction operator /(Fraction frac1, Fraction frac2)
        {
            return (Multiply(frac1, Inverse(frac2)));
        }

        public static bool operator >(Fraction frac1, Fraction frac2)
        {
            return frac1.Numerator * frac2.Denominator > frac2.Numerator * frac1.Denominator;
        }

        public static bool operator <(Fraction frac1, Fraction frac2)
        {
            return frac1.Numerator * frac2.Denominator < frac2.Numerator * frac1.Denominator;
        }

        private static Fraction Add(Fraction firstFraction, Fraction secondFraction)
        {
            BigInteger numerator = firstFraction.Numerator * secondFraction.Denominator + secondFraction.Numerator * firstFraction.Denominator;
            BigInteger denominator = firstFraction.Denominator * secondFraction.Denominator;
            return ReduceFraction(new Fraction(numerator, denominator));
        }

        private static Fraction Substract(Fraction firstFraction, Fraction secondFraction)
        {
            BigInteger numerator = firstFraction.Numerator * secondFraction.Denominator - secondFraction.Numerator * firstFraction.Denominator;
            BigInteger denominator = firstFraction.Denominator * secondFraction.Denominator;
            return ReduceFraction(new Fraction(numerator, denominator));
        }

        private static Fraction Multiply(Fraction firstFraction, Fraction secondFraction)
        {
            BigInteger numerator = firstFraction.Numerator * secondFraction.Numerator;
            BigInteger denominator = firstFraction.Denominator * secondFraction.Denominator;
            return ReduceFraction(new Fraction(numerator, denominator));
        }

        public static Fraction Sqrt(Fraction fraction)
        {
            BigInteger numerator = fraction.Numerator;
            BigInteger denominator = fraction.Denominator;

            return ReduceFraction((new Fraction(Sqrt(numerator), Sqrt(denominator))));
        }

        public static Fraction Abs(Fraction fraction)
        {
            BigInteger nominator = BigInteger.Abs(fraction.Numerator);
            BigInteger denominator = BigInteger.Abs(fraction.Denominator);

            return ReduceFraction((new Fraction(nominator, denominator)));
        }

        public static BigInteger Sqrt(BigInteger n)
        {
            if (n == 0) return 0;
            if (n > 0)
            {
                int bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(n, 2)));
                BigInteger root = BigInteger.One << (bitLength / 2);

                while (!isSqrt(n, root))
                {
                    root += n / root;
                    root /= 2;
                }

                return root;
            }

            throw new ArithmeticException("NaN");
        }

        private static Boolean isSqrt(BigInteger n, BigInteger root)
        {
            BigInteger lowerBound = root * root;
            BigInteger upperBound = (root + 1) * (root + 1);

            return (n >= lowerBound && n < upperBound);
        }

        public static Fraction ReduceFraction(Fraction fraction)
        {

            if (fraction.Numerator == 0)
            {
                fraction.Denominator = 1;
                return fraction;
            }

            BigInteger iGCD = BigInteger.GreatestCommonDivisor(fraction.Numerator, fraction.Denominator);
            fraction.Numerator /= iGCD;
            fraction.Denominator /= iGCD;

            if (fraction.Denominator < 0)
            {
                fraction.Numerator *= -1;
                fraction.Denominator *= -1;
            }
            return fraction;

        }

        public static Fraction Inverse(Fraction fraction)
        {
            if (fraction.Numerator == 0)
                throw new Exception("Nominator is ZERO, so inversion is not possible.");

            BigInteger numerator = fraction.Denominator;
            BigInteger denominator = fraction.Numerator;
            return ReduceFraction(new Fraction(numerator, denominator));
        }

        public override string ToString()
        {
            string str;
            if (this.Denominator == 1)
                str = this.Numerator.ToString();
            else
                str = this.Numerator + " / " + this.Denominator;
            return str;
        }

        public BigInteger Denominator
        {
            get => _denominator;
            set
            {
                if (value != 0)
                    _denominator = value;
                else
                    throw new Exception("Denominator cannot have ZERO value.");
            }
        }

        public BigInteger Numerator
        {
            get => _numerator;
            set => _numerator = value;
        }

        private BigInteger _numerator;
        private BigInteger _denominator;
    }
}
