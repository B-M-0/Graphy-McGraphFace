using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Vector : Matrix
    {
        Rational[] Values;

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

        // returns the cross product of two vectors
        public static Vector operator +(Vector A, Vector B)
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
            return Math.Round(Math.Sqrt(sum));
        }
        //public Vector UnitVector()
        //{ 
            
        
        //}

    }
}
