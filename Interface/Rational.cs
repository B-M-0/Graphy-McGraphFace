using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Interface
{
    public class Rational
    {
        public readonly bool isInteger = false;
        public readonly bool isFloat = false;
        private int numerator;
        private int denominator;
        private readonly double decimalVal;

        public Rational(int numerator, int denominator = 1)
        {
            if (numerator < 0 && denominator < 0)
            {
                numerator = Math.Abs(numerator);
                denominator = Math.Abs(denominator);
            }
            else if (denominator < 0)
            {
                numerator = -(numerator);
                denominator = Math.Abs(denominator);
            }
            if (numerator == 0 || denominator == 1)
            {
                denominator = 1;
                this.isInteger = true;
            }
            this.numerator = numerator;
            this.denominator = denominator;

            this.decimalVal = (double)numerator / (double)denominator;
            signHandling();
        }
        public Rational(double decimalValue)
        {
            // truncates to (k)dp
            this.decimalVal = Math.Round(decimalValue, Utilities.decimalPlaces + 5);
            if (!decimalValue.ToString().Contains('.'))
            {
                this.isInteger = true;
                numerator = (int)decimalValue;
                denominator = 1;
            }
            else if (decimalVal.ToString().Split('.')[1].Length > Utilities.decimalPlaces || decimalVal > 10000000)
            {
                isFloat = true;
                numerator = 0;
                denominator = 0;
            }
            else
            {
                decimalValue = Math.Round(decimalValue, Utilities.decimalPlaces);

                int denominator = 1;
                while (decimalValue % 1 != 0)
                {
                    decimalValue = Math.Round(decimalValue * 10, Utilities.decimalPlaces);
                    denominator *= 10;
                }

                this.numerator = (int)decimalValue;
                this.denominator = denominator;


                Rational X = this.Simplify();
                this.numerator = X.numerator;
                this.denominator = X.denominator;
            }

        }

        public Rational Simplify()
        {
            Rational result = this;
            if (isFloat)
            {
                return result;
            }
            int end = Math.Abs(Math.Min(numerator, denominator));
            // the condition is the truncated root of the smaller number.
            for (int i = 2; i <= end; i++)
            {
                if (Math.Abs(numerator) % i == 0 && Math.Abs(denominator) % i == 0)
                {
                    result = new Rational(numerator / i, denominator / i);
                    i = end + 1;
                    result = result.Simplify();
                }
            }

            signHandling();
            return result;
        }
        public Rational Reciprocal()
        {
            if (isFloat)
                return 1 / this;
            else if (this == 0)
                return this;
            else
                return new Rational(denominator, numerator);
        }



        public static implicit operator double(Rational A)
        {
            if (A == null)
                return 0;
            else
                return A.decimalVal;
        }
        public static implicit operator Rational(double A)
        {
            return new Rational(A).Simplify();
        }
        public static implicit operator Rational(int A)
        {
            return new Rational(A).Simplify();
        }
        public static explicit operator int(Rational A)
        {
            return (int)(double)A;
        }

        public static implicit operator string(Rational A)
        {

            if (A.isInteger)
                return $"{A.numerator}";
            else
                return $"{A.numerator}/{A.denominator}";
        }
        public string LatexFormat()
        {
            Simplify();
            string sign = "";
            if (numerator < 0) sign = "-";

            if (isInteger)
                return numerator.ToString();
            else if (isFloat)
                return this.Decimal().ToString();
            else
                return $"{sign}\\frac{{{Math.Abs(numerator)}}}{{{denominator}}}";
        }
      

        public static Rational operator +(Rational A, Rational B)
        {
            Rational result;
            if (A.isFloat || B.isFloat)
                result = new Rational(A.decimalVal + B.decimalVal);
            else if (A.denominator == B.denominator)
                result = new Rational(A.numerator + B.numerator, A.denominator);
            else
                result = new Rational((A.numerator * B.denominator) + (B.numerator * A.denominator), A.denominator * B.denominator);

            return result.Simplify();
        }
        public static Rational? operator -(Rational A, Rational B)
        {
            Rational result;

            if (A.isFloat || B.isFloat)
                result = new Rational(A.decimalVal - B.decimalVal);
            else if (A.denominator == B.denominator)
                result = new Rational(A.numerator - B.numerator, A.denominator);
            else
                result = new Rational((A.numerator * B.denominator) - (B.numerator * A.denominator), A.denominator * B.denominator);

            return result.Simplify();


        }
        public static Rational operator *(Rational A, Rational B)
        {
            if (A == null || B == null)
                return null;
            if (A.isFloat || B.isFloat)
                return A.decimalVal * B.decimalVal;
            else
            {
                Rational result = new Rational(A.numerator * B.numerator, A.denominator * B.denominator);
                return result.Simplify();
            }
        }
        public static Rational operator /(Rational A, Rational B)
        {
            if (A.isFloat || B.isFloat)
                return A.decimalVal / B.decimalVal;
            else
                return A * B.Reciprocal();
        }
        public static Rational operator ^(Rational A, double power)
        {
            Rational result = new Rational((int)Math.Pow(A.numerator, power), (int)Math.Pow(A.denominator, power));
            return result.Simplify();
        }

        public double Decimal()
        {
            return Math.Round(decimalVal, Utilities.decimalPlaces);
        }

        public int Numerator()
        {
            return this.numerator;
        }
        public int Denominator()
        {
            return this.denominator;
        }

        private void signHandling()
        {
            if (this.numerator < 0 && this.denominator < 0)
            {
                this.numerator = Math.Abs(this.numerator);
                this.denominator = Math.Abs(this.denominator);
            }
            else if (this.denominator < 0)
            {
                this.numerator = -(this.numerator);
                this.denominator = Math.Abs(this.denominator);
            }
            
        }

    }
}