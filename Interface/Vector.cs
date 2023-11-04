using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Vector : Matrix
    {
        private new Rational[] values;
        public Rational this[int index]
        { 
            get => values[index];   
            set => values[index] = value;
        }

        public new Rational[] GetValues()
        {
            return values;
        }
        

        public Vector(int dimensions) : base(dimensions, 1)
        {
            if (dimensions != 2 && dimensions != 3)
                throw new ArgumentException("Error: Stupid Dimensions Exception.");   
            this.values = new Rational[dimensions];  
        }

        public Vector(Rational[,] values) : base(values.GetLength(0), 1)
        {
            this.values = new Rational[values.GetLength(0)];
            for (int i = 0; i < values.GetLength(0); i++)
            {
                this.values[i] = values[i, 0];
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
                sum += (double)(A.values[i] * B.values[i]);
            }  

            return sum;
        }
        //

        public static Vector operator *(double scalar, Vector A)
        {
            Vector B = new Vector(A.rows);
            for (int i = 0; i < A.rows; i++)
            {
                B[i] = A[i] * scalar;
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
                B[i] = A[i] * scalar;
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
                C[i] = A[i] - B[i]; 
            }
            return C;
        }
        public static Vector operator +(Vector A, Vector B)
        {
            Vector C = new Vector(A.rows);
            for (int i = 0; i < A.rows; i++)
            {
                C[i] = A[i] + B[i];
            }
            return C;
        }

        // returns the cross product of two vectors
        public static Vector? cross(Vector A, Vector B)
        {
           
            if (A.rows == 3 && B.rows == 3)
            {
                Vector C = new Vector(3);
                C[0] = A[1] * B[2] - A[2] * B[1];
                C[1] = A[2] * B[0] - A[0] * B[2];
                C[2] = A[0] * B[1] - A[1] * B[0];
                return C;
            }
            return null;
        }

      



        public double Magnitude()
        {
            int sum = 0;
            foreach (int element in values)
            {
                sum += element * element;
            }
            double root = Math.Sqrt(sum);
            return Math.Round(root);
        }

        public static Vector Proj(Vector u, Vector v)
        {
            return ((u * v) / (u * u)) * u;
        }

        public static Vector[] GramSchmidtOrthogonalization(Vector[] v)
        {
            Vector[] u = new Vector[v.Length];
            for (int i = 0; i < v.Length; i++)
            {
                u[i] = v[i];
                for (int j = 0; j < i; j++)
                {
                    u[i] = u[i] - Proj(u[j], v[i]);
                }
            }
            return u;
        }
        public static Vector[] GramSchmidtOrthonormalization(Vector[] v)
        {
            Vector[] u = new Vector[v.Length];
            Vector[] e = new Vector[v.Length];
            for (int i = 0; i < v.Length; i++)
            {
                u[i] = v[i];
                for (int j = 0; j < i; j++)
                {
                    u[i] = u[i] - Proj(u[j], v[i]);
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
