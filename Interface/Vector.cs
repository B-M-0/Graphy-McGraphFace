using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Vector : Matrix
    {
        public Vector(int Dimensions) : base(Dimensions, 1)
        {
            if (Dimensions != 2 || Dimensions != 3)
                throw new ArgumentException("Error: Stupid Dimensions Exception.");    
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
                sum += (double)(A.Values[i,0] * B.Values[i, 0]);
            }  

            return sum;
        }

        // returns the cross product of two vectors
        public static double operator ^(Vector A, Vector B)
        {
            double sum = 0;
            return sum;
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
