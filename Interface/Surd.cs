using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Surd //: INumber
    {
        public Rational Coeficient;
        public int Root;

        public static implicit operator string(Surd A)
        {
            if (A.Root == 1)
            {
                return (string)A.Coeficient;
            }
            else if (A.Coeficient == 1)
                return "√" + A.Root;
            else
                return (string)A.Coeficient + "√" + A.Root;
        }
        public static explicit operator double(Surd A)
        {
            return A.Coeficient * Math.Sqrt(A.Root);
        }
        public Surd(Rational coef, int rootVal)
        {
            Coeficient = coef;
            Root = rootVal;
        }
        public Surd(int rootVal)
        {
            Coeficient = 1;
            Root = rootVal;
            this.Simplify();
        }

        public static Surd Sqrt(Rational x)
        {
            Surd output;
            
            
            if (x < 0) ;
            //complex number shit

            if (x.isInteger)
                output = new Surd((int)x);
            else
            {
                Surd A = Sqrt(x.Numerator());
                Surd B = Sqrt(x.Denominator());

                output = new Surd(A.Coeficient/(B.Coeficient*B.Root),A.Root*B.Root);
            }

            output.Simplify();
            return output;
        }
       

        public static Surd operator *(Surd A, Surd B)
        {
            return new Surd(A.Coeficient * B.Coeficient, A.Root * B.Root);
        }
        public static Surd operator /(Surd A, Surd B)
        {
            return new Surd(A.Coeficient / B.Coeficient, A.Root / B.Root);
        }

        public static Surd operator *(Rational A, Surd B)
        {
            return new Surd(A * B.Coeficient,B.Root);
        }

        public static Surd operator *(Surd A, Rational B)
        {
            return B * A;
        }

        public static Surd operator +(Surd A, Surd B)
        {
            if (A.Root == B.Root)
                return new Surd(A.Coeficient + B.Coeficient, A.Root);
            else
                throw new Exception("Root values are not equal");
        }
        public static Surd operator -(Surd A, Surd B)
        {
            if (A.Root == B.Root)
                return new Surd(A.Coeficient - B.Coeficient, A.Root);
            else
                throw new Exception("Root values are not equal");
        }

        public double Decimal()
        {
            return Math.Sqrt(Root) * Coeficient.Decimal();
        }
        private Surd Simplify()
        {
            Surd A = this;
            int count = 0;
            
            
            for (int i = 2; i*i <= A.Root; i++)
            { 
                if ((long)A.Root % ((long)i * (long)i) == 0)
                {
                    A.Root /= (i * i);
                    A.Coeficient *= i;
                    count++;
                    A.Simplify();
                }
            }
            return A;   
        }
    }
}
