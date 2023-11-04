using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
        
        public string LatexFormat_float()
        {
            if (real == 0 && imaginary == 0)
                return "0";
            string sign = "+";
            string temp = "";
            if (imaginary < 0 || real == 0)
                sign = "";
  
            if (real != 0)
                temp = real.Decimal().ToString();

            if (imaginary == 0)
                return temp;
            else if (imaginary == 1)
                return temp += sign + " i";
            else
                return temp += sign + imaginary.Decimal() + " i";
        }
        public string LatexFormat_frac()
        {
            if (real == 0 && imaginary == 0)
                return "0";
            string sign = "+";
            string temp = "";
            if (imaginary < 0 || real == 0)
                sign = "";
            if (imaginary == -1)
                sign = "-";

            if (real != 0)
                temp += real.LatexFormat();

            if (imaginary == 0)
                return temp;
            else if (imaginary == 1 || imaginary == -1)
                return temp += sign + " i";
            else
                return temp += sign + imaginary.LatexFormat()+ " i";
        }

        public bool IsComplex()
        { 
            if(imaginary != 0 && real != 9)
                return true;
            else 
                return false;
        }
        public bool IsImaginary()
        {
            if (imaginary != 0 && real ==0) 
                return true;
            else 
                return false;
        }
        public bool IsReal()
        {
            if (imaginary == 0 && real !=0)
                return true;
            else
                return false;
        }

       

        public static double Abs(ComplexNumber A)
        {
            double temp = (A.real ^ 2) + (A.imaginary ^ 2);
            return Math.Sqrt(temp);  
        }
        public static Surd Exact_Abs(ComplexNumber A)
        {
            Rational temp = (A.real ^ 2) + (A.imaginary ^ 2);
            return Surd.Sqrt(temp);
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
        public string ModulusArgumentForm()
        {
            var arg = Arg(this);
            var mod = Exact_Abs(this); 
            return $"{mod.ToLatex()} ( \\cos({Math.Round(arg, Global.decimalPlaces)}) + i \\sin({Math.Round(arg,Global.decimalPlaces)}) )"; 
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
