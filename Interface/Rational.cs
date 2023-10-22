using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{ 
    public class Rational 
    {
        public bool isInteger = false;
        public bool isFloat = false;
        private int numerator ;
        private int denominator;
        private double Decimalval;

        public static implicit operator double(Rational A)
        {
            if(A == null)
                return 0;
            return A.Decimalval = (double)A.numerator / (double)A.denominator;
           
        }
        //as
        public static implicit operator Rational(double A)
        {
            double B = Math.Round(A,Global.decimalPlaces);
            return new Rational(B).Simplify();
        }
        public static explicit operator Rational(int A)
        {
            return new Rational(A).Simplify();
        }

        public static explicit operator int(Rational A)
        {
            return (int) (double) A;
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
            if(!isInteger)
                return $"\\frac{{{numerator}}}{{{denominator}}}";
            else 
                return numerator.ToString() ;
        }
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
            this.numerator = numerator;
            this.denominator = denominator;

            this.Decimalval = (double)numerator / (double)denominator;
            signHandling();
        }

        public Rational(double Decimal)
        {
            // truncates to (k)dp
            this.Decimalval = Math.Round(Decimal,Global.decimalPlaces + 5);
            if (!Decimal.ToString().Contains('.'))
            {
                this.isInteger = true;
                numerator = (int)Decimal;
                denominator = 1; 
            }
            else if (Decimalval.ToString().Split('.')[1].Length > Global.decimalPlaces)
            {
                isFloat = true;
                numerator = 0;
                denominator = 0;
            }
            else
            {
                Decimal = Math.Round(Decimal, Global.decimalPlaces);

                int denominator = 1;
                while (Decimal % 1 != 0)
                {
                    Decimal = Math.Round(Decimal * 10, Global.decimalPlaces);
                    denominator *= 10;
                }

                this.numerator = (int)Decimal;
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
        public Rational reciprocal()
        {
            return new Rational(denominator, numerator);
        }

        public static Rational operator +(Rational A, Rational B)
        {
            Rational result;
            if (A.isFloat || B.isFloat)
                result = new Rational(A.Decimalval + B.Decimalval);
            else if (A.denominator == B.denominator)
                result = new Rational(A.numerator + B.numerator, A.denominator);
            else
                result = new Rational((A.numerator * B.denominator) + (B.numerator * A.denominator), A.denominator * B.denominator);

            return result.Simplify();
        }
        public static Rational operator -(Rational A, Rational B)
        {
            Rational result;

            if(A.isFloat || B.isFloat)
                result = new Rational(A.Decimalval - B.Decimalval);
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
                return A.Decimalval * B.Decimalval;
            else
            {
                Rational result = new Rational(A.numerator * B.numerator, A.denominator * B.denominator);
                return result.Simplify();
            }
        }
        public static Rational operator /(Rational A, Rational B)
        {
            if(A.isFloat|| B.isFloat)
                return A.Decimalval / B.Decimalval; 
            else
                return A * B.reciprocal();
        }
        public static Rational operator ^(Rational A, double power)
        {
            Rational result = new Rational(Math.Pow(A.numerator, power)/Math.Pow(A.denominator, power));
            return result.Simplify();
        }

        public double Decimal()
        {
            return Math.Round(Decimalval,Global.decimalPlaces);
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
            if (numerator == 0 || denominator == 1)
            {
                denominator = 1;
                isInteger = true;
            }
        }
               
    }
}
