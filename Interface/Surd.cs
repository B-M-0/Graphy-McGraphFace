using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;

namespace Interface
{
    public class Surd
    {
        public Rational Surplus = 0;
        public Rational Coefficient;
        public int Root;
        public bool isIrrational = true;

        public static implicit operator Surd(Rational A)
        {
            return new Surd(0, 1, A);
        }

        public static implicit operator Surd(double A)
        {
            return new Surd(0, 1, A);
        }
        public static implicit operator Surd(int A)
        {
            return new Surd(0, 1, A);
        }
        public static implicit operator string(Surd A)
        {
            if (A.Root == 1)
            {
                return (string)A.Coefficient;
            }
            else if (A.Coefficient == 1)
                return "√" + A.Root;
            else
                return (string)A.Coefficient + "√" + A.Root;
        }
        public static implicit operator double(Surd A)
        {
            return A.Surplus + A.Coefficient * Math.Sqrt(A.Root);
        }
        public static explicit operator Rational(Surd A)
        {
            return new Rational(A.Surplus + A.Coefficient * Math.Sqrt(A.Root));
        }

      
        public Surd(Rational Coefficient, int Root, Rational Surplus)
        {
            this.Coefficient = Coefficient;
            this.Root = Root;
            this.Surplus = Surplus;
            if (Root == 1)
                isIrrational = false;
        }
        public Surd(Rational Coefficient, int Root)
        {
            this.Coefficient = Coefficient;
            this.Root = Root;
            this.Surplus = 0;
            if (Root == 1)
                isIrrational = false;
        }

        public Surd(int rootVal)
        {
            Coefficient = 1;
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

                output = new Surd(A.Coefficient / (B.Coefficient * B.Root), A.Root * B.Root);
            }

            output.Simplify();
            return output;
        }


        public static Surd operator *(Surd A, Surd B)
        {
            var a = (A.Surplus * B.Surplus + new Surd(A.Coefficient * B.Surplus, A.Root));
            var b = new Surd(A.Surplus * B.Coefficient, B.Root);
            var c = new Surd(B.Coefficient * A.Coefficient, A.Root * B.Root);
            try
            {
                return a + b + c;
            }
            catch 
            {
                return a.Decimal() + b.Decimal() + c.Decimal();   
            }
        }
        public static Surd operator *(Rational A, Surd B)
        {
            return (new Surd(A * B.Coefficient, B.Root) + A * B.Surplus).Simplify();
        }
        public static Surd operator *(Surd A, Rational B)
        {
            return B * A;
        }


        public static List<Surd> Expand(List<Surd> A, List<Surd> B)
        {
            Dictionary<int, Rational> C = new Dictionary<int, Rational>();
            //         <Root, Coefficient> (its backwards)
            for (int i = 0; i < B.Count(); i++)
            {
                for (int j = 0; j < A.Count(); j++)
                {
                    Surd temp = A[j] * B[i];
                    if (C.ContainsKey(temp.Root))
                    {
                        C[temp.Root] += temp.Coefficient;
                    }
                    else
                    {
                        C.Add(temp.Root, temp.Coefficient);
                    }
                }
            }
            return DictToList(C);
        } 
      
        public static Surd operator +(Rational A, Surd B)
        {
            return new Surd(B.Coefficient, B.Root, A+B.Surplus).Simplify();
        }

        public static Surd operator +(Surd A, Rational B)
        {
            return B + A;
        }

        
        public static Surd operator +(Surd A, Surd B)
        {
            if (A.Root == B.Root || A.Root == 1 || B.Root == 1)
                return (new Surd(A.Coefficient + B.Coefficient, Math.Max(A.Root,B.Root)) + A.Surplus + B.Surplus).Simplify();
            else
                throw new Exception("Root values are not equal");
        }
        public static Surd operator -(Surd A, Surd B)
        {
            if (A.Root == B.Root|| A.Root == 1|| B.Root == 1)
                return new Surd(A.Coefficient - B.Coefficient, Math.Max(A.Root, B.Root)) + (A.Surplus - B.Surplus).Simplify();
            else
                throw new Exception("Root values are not equal");
        }
        public static Surd operator -(Rational A, Surd B)
        {
            return new Surd(-1 * B.Coefficient, B.Root, A - B.Surplus).Simplify();
        }

        public static Surd operator -(Surd A, Rational B)
        {
            return new Surd(A.Coefficient, A.Root, A.Surplus - B).Simplify();
        }


        public double Decimal()
        {
            return Math.Sqrt(Root) * Coefficient.Decimal();
        }
        private Surd Simplify()
        {
            Surd A = this;
            int count = 0;


            for (int i = 2; i * i <= A.Root; i++)
            {
                if ((long)A.Root % ((long)i * (long)i) == 0)
                {
                    A.Root /= (i * i);
                    A.Coefficient *= i;
                    count++;
                    A.Simplify();
                }
            }
            if (Root == 1)
                isIrrational = false;
            return A;
        }


        public string ToLatex()
        {
            if (Surplus == 0 && Coefficient == 0)
                return "0";
            string sign = "+";
            string temp = "";
            if (Coefficient < 0 || Surplus == 0)
                sign = "";

            if (Surplus != 0)
                temp += Surplus.LatexFormat();

            if(Coefficient == -1)
                sign = "-"; 

            if (Coefficient == 0)
                return temp;
            else if (Coefficient == 1|| Coefficient == -1)
                return temp += sign + $" \\sqrt{{{Root}}}";
            else
                return temp += sign + Coefficient.LatexFormat() + $"\\sqrt{{{Root}}}";
        }

        private static List<Surd> DictToList(Dictionary<int, Rational> A)
        {
            List<Surd> output = new List<Surd>();
            foreach (KeyValuePair<int, Rational> pair in A)
            {
                output.Add(new Surd(pair.Value, pair.Key));
            }

            return output;
        }
    }
}
