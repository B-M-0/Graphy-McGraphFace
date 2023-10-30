using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Vector : Matrix
    {
        public new Rational[] Values;

        public Rational[] GetValues() { return Values; }    

        public Vector(int Dimensions) : base(Dimensions, 1)
        {
            if (Dimensions != 2 && Dimensions != 3)
                throw new ArgumentException("Error: Stupid Dimensions Exception.");   
            this.Values = new Rational[Dimensions];  
        }

        public Vector(Rational[,] Values) : base(Values.GetLength(0), 1)
        {
            this.Values = new Rational[Values.GetLength(0)];
            for (int i = 0; i < Values.GetLength(0); i++)
            {
                this.Values[i] = Values[i, 0];
            }
        }


        // returns the dot product of two vectors
        public static double operator *(Vector A, Vector B)
        {
            if (A.rows != B.rows)
            {
                throw new ArgumentException("Vectors are not of the same dimension.");
            }

            double sum = 0;
            for (int i = 0; i < A.rows;i++)
            { 
                sum += (double)(A.Values[i] * B.Values[i]);
            }  

            return sum;
        }
        //

        public static Vector operator *(double scalar, Vector A)
        {
            Vector B = new Vector(A.rows);
            for (int i = 0; i < A.rows; i++)
            {
                B.Values[i] = A.Values[i] * scalar;
            }
            return B;
        }
        public static Vector operator *(Vector A, double scalar)
        {
            return scalar * A;
        }
        public static Vector operator *(Rational scalar, Vector A)
        {
            Vector B = new Vector(A.rows);
            for (int i = 0; i < A.rows; i++)
            {
                B.Values[i] = A.Values[i] * scalar;
            }
            return B;
        }
        public static Vector operator *(Vector A, Rational scalar)
        {
            return scalar * A;
        }
        public static Vector operator -(Vector A, Vector B)
        {
            Vector C = new Vector(A.rows);
            for (int i = 0; i < A.rows; i++)
            {
                C.Values[i] = A.Values[i] - B.Values[i]; 
            }
            return C;
        }
        public static Vector operator +(Vector A, Vector B)
        {
            Vector C = new Vector(A.rows);
            for (int i = 0; i < A.rows; i++)
            {
                C.Values[i] = A.Values[i] + B.Values[i];
            }
            return C;
        }

        // returns the cross product of two vectors
        public static Vector cross(Vector A, Vector B)
        {
           
            if (A.rows == 3 && B.rows == 3)
            {
                Vector C = new Vector(3);
                C.Values[0] = A.Values[1] * B.Values[2] - A.Values[2] * B.Values[1];
                C.Values[1] = A.Values[2] * B.Values[0] - A.Values[0] * B.Values[2];
                C.Values[2] = A.Values[0] * B.Values[1] - A.Values[1] * B.Values[0];
                return C;
            }

            return null;
        }

      



        public double Magnitude()
        {
            int sum = 0;
            foreach (int element in Values)
            {
                sum += element * element;
            }
            double root = Math.Sqrt(sum);
            return Math.Round(root);
        }

        public static Vector proj(Vector u, Vector v)
        {
            return ((u * v) / (u * u)) * u;
        }

        public static Vector[] Gram_Schmidt_orthogonalization(Vector[] v)
        {
            Vector[] u = new Vector[v.Length];
            for (int i = 0; i < v.Length; i++)
            {
                u[i] = v[i];
                for (int j = 0; j < i; j++)
                {
                    u[i] = u[i] - proj(u[j], v[i]);
                }
            }
            return u;
        }
        public static Vector[] Gram_Schmidt_orthonormalization(Vector[] v)
        {
            Vector[] u = new Vector[v.Length];
            Vector[] e = new Vector[v.Length];
            for (int i = 0; i < v.Length; i++)
            {
                u[i] = v[i];
                for (int j = 0; j < i; j++)
                {
                    u[i] = u[i] - proj(u[j], v[i]);
                }
                Rational scalar;
                double absU = u[i].Magnitude();
                if (absU % 1 == 0)
                    scalar = new Rational(1, (int)absU);
                else
                    scalar = 1 / absU;
                 
                Vector temp = scalar * u[i];
                e[i] = temp;
            }
            return e;
        }
        //public Vector UnitVector()
        //{ 


        //}

    }
}
