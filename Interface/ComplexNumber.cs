using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public  class ComplexNumber
    {
        public Rational real;
        public Rational imaginary;

        public ComplexNumber(Rational real, Rational imaginary)
        {
            this.real = real;
            this.imaginary = imaginary;
        }

        public static double Abs(ComplexNumber A)
        { 
            return Math.Sqrt(A.real^2 + A.imaginary^2);  
        }

        public static double Arg(ComplexNumber A)
        {
            double angle = Math.Atan(Math.Abs(A.imaginary) / Math.Abs(A.real));
            if (A.real > 0 && A.imaginary == 0)
                return 0;
            else if (A.real > 0 && A.imaginary > 0)
                return angle;
            else if (A.real == 0 && A.imaginary > 0)
                return Math.PI / 2;
            else if (A.real < 0 && A.imaginary > 0)
                return 2 * Math.PI - angle;
            else if (A.real < 0 && A.imaginary ==0)
                return Math.PI;
            else if (A.real > 0 && A.imaginary < 0)
                return (-1) * angle;    
            else if(A.real == 0 && A.imaginary < 0 )
                return (-1) * Math.PI/2;
            else 
                return angle - Math.PI;   
        }

        public ComplexNumber Conjugate()
        { 
            return new ComplexNumber(real, (-1) * imaginary);  
        }

        public static implicit operator ComplexNumber (Rational A)
        {
            return new ComplexNumber(A, 0);
        }
        public static implicit operator ComplexNumber(int A)
        {
            return new ComplexNumber(A, 0);
        }
        public static implicit operator ComplexNumber(double A)
        {
            return new ComplexNumber(A, 0);
        }

        public static ComplexNumber operator +(ComplexNumber A, ComplexNumber B)
        {
            return new ComplexNumber(A.real + B.real, A.imaginary + B.imaginary);
        }
        public static ComplexNumber operator -(ComplexNumber A, ComplexNumber B)
        {
            return new ComplexNumber(A.real - B.real, A.imaginary - B.imaginary);
        }
        public static ComplexNumber operator *(ComplexNumber A, ComplexNumber B)
        {
            return new ComplexNumber(A.real* B.real - A.imaginary * B.imaginary  , A.imaginary * B.real + A.real *B.imaginary);
        }
        public static ComplexNumber operator /(ComplexNumber A, ComplexNumber B)
        {
            ComplexNumber C = A * B.Conjugate();
            return new ComplexNumber(C.real / (B * B.Conjugate()).real, C.imaginary / (B * B.Conjugate()).real);
        }







    }
}
