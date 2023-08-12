using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Diagnostics;

namespace NEA26
{

    public class Matrix
    {
        // NEEDS A CLEANING UP
        public static T[,] Convert2DListToArray<T>(List<List<T>> list)
        {
            T[,] array = new T[list.Count(), list[0].Count()];

            for (int row = 0; row < list.Count(); row++)
            {
                for (int column = 0; column < list[0].Count(); column++)
                {
                    array[row,column] = list[row][column];    
                }
            }
            return array;
        }

        public int rows, columns;
        public Rational[,] Values;
       

      
        public Matrix(Rational[,] MatrixElements):this(MatrixElements.GetLength(0), MatrixElements.GetLength(1))
        {
            _2DArrayPopMtrx(MatrixElements);
        }
        public Matrix(double[,] MatrixElements) : this(MatrixElements.GetLength(0), MatrixElements.GetLength(1))
        {
            _2DArrayPopMtrx(MatrixElements);
        }

        public Matrix(int[,] MatrixElements) : this(MatrixElements.GetLength(0), MatrixElements.GetLength(1))
        {
            _2DArrayPopMtrx(MatrixElements);
        }


        public Matrix(Rational MatrixElement) : this(1,1)
        {
            _2DArrayPopMtrx(new Rational[1, 1] { { MatrixElement } });
        }
        public Matrix(double MatrixElement) : this(1, 1)
        {
            _2DArrayPopMtrx(new double[1, 1] { { MatrixElement } });
        }

        public Matrix(int MatrixElement) : this(1, 1)
        {
            _2DArrayPopMtrx(new int[1, 1] { { MatrixElement } });
        }


        public Matrix(int a = 2, int b = 2)
        {
            rows = a;
            columns = b;
            Values = new Rational[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Values[i, j] = 0;
                }
            }
        }





        //Allows matrix to be poplated from console
        public void CmdPopMtrx()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Values[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }
        }
        protected void _2DArrayPopMtrx(Rational[,] MatrixElements)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Values[i, j] = MatrixElements[i,j];
                }
            }
        }
        protected void _2DArrayPopMtrx(int[,] MatrixElements)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Values[i, j] = MatrixElements[i,j];
                }
            }
        }
        protected void _2DArrayPopMtrx(double[,] MatrixElements)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Values[i, j] = Math.Round(MatrixElements[i,j],Global.decimalPlaces);
                }
            }
        }

        //Arithmetic operations.

        //Adds two Matrices together and returns the result
        public static Matrix operator +(Matrix A, Matrix B)
        {
            Matrix C;

            if ((A.columns == B.columns) && (A.rows == B.rows))
            {
                C = new Matrix(A.rows, A.columns);

                for (int i = 0; i < C.rows; i++)
                {
                    for (int j = 0; j < C.columns; j++)
                    {
                        C.Values[i, j] = A.Values[i, j] + (B.Values[i, j]);
                    }
                }
            }
            else
            {
                throw new ArgumentException();
            }
            return C;
        }

        //Substracts two Matrices together and returns the result
        public static Matrix operator -(Matrix A, Matrix B)
        {
            Matrix C;

            if ((A.columns == B.columns) && (A.rows == B.rows))
            {
                C = new Matrix(A.rows, A.columns);

                for (int i = 0; i < C.rows; i++)
                {
                    for (int j = 0; j < C.columns; j++)
                    {
                        C.Values[i, j] = A.Values[i, j] - (B.Values[i, j]);
                    }
                }
            }
            else
            {
                throw new ArgumentException();
            }
            return C;
        }

        //Multiplies two Matrices together and returns the result
        public static Matrix operator *(Matrix A, Matrix B)
        {
            Matrix C;
            if (A.columns == B.rows)
            {
                C = new Matrix(A.rows, B.columns);

                for (int row = 0; row < C.rows; row++)
                {
                    for (int column = 0; column < C.columns; column++)
                    {
                        double sum = 0;
                        for (int i = 0; i < A.columns; i++)
                        {
                            sum +=(double) (A.Values[row, i] * B.Values[i, column]);
                        }
                        C.Values[row, column] = sum;
                    }
                }
            }
            else
            {
                throw new ArgumentException();
            }

            return C;
        }

        //Multipy  by  scalar
        public static Matrix operator *(Rational Scalar, Matrix A)
        {
            for (int i = 0; i < A.rows; i++)
            {
                for (int j = 0; j < A.columns; j++)
                {
                    A.Values[i, j] = A.Values[i, j] * Scalar;
                }
            }
            return A;
        }

        public static Matrix operator *(Matrix A, Rational Scalar)
        {
            return Scalar * A;
        }



        //parses a set of sets in the form M:{{a,b,c},{d,e,f},{g,h,i}}
        public static Matrix StringToMatrix(string input)
        {
            //Valiation
            int openBracketCount = 0;
            int closedBracketCount = 0;
            foreach (char letter in input)
            {
                if (letter == '{')
                    openBracketCount++;
                else if (letter == '}')
                    closedBracketCount++;
            }
            if (openBracketCount != closedBracketCount)
                throw new ArgumentException("Invalid Format: Not all brackets are closed.");
            //M:
            //({ },{ },{ })

            List<List<int>> MatrixElements = new List<List<int>>();
            input = input.Substring(1, input.Length - 2);
            int pivot = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '}')
                {
                    string sublistStr = input.Substring(pivot, (i - pivot) + 1);
                    MatrixElements.Add(InputHandling.ListParse(sublistStr));
                    if (sublistStr.Length != input.Length)
                    {
                        input = input.Substring(i + 2);
                        i = 0;
                    }
                }
            }
            int dimension = MatrixElements[0].Count();
            foreach (List<int> sets in MatrixElements)
            {
                if (sets.Count() != dimension) 
                    throw new Exception("Invalid Format: Not all rows are the same length.");
            }

            if (MatrixElements[0].Count() == MatrixElements.Count())
                return new SquareMatrix(Convert2DListToArray(MatrixElements));
            else
                return new Matrix(Convert2DListToArray(MatrixElements));
        }


        static public Matrix LatexToMatrix(string input = @"a&b\\c&d\\")
        {
           
            int columns = 0;
            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input.Substring(i, 2) == @"\\")
                {
                    input = input.Substring(0, i) + "%" + input.Substring(i+2);
                    i = 0;
                    columns++;
                }
            }
            string[] rows = input.Split('%');


            Rational[,] M = new Rational[rows.Length,columns]; 
            for (int i =0;i< rows.Length;i++)
            {
                string[] row = rows[i].Split('&');
                for (int j = 0; j < row.Length; j++)
                {
                    M[i, j] = (Rational)(double.Parse(row[j]));
                }
            }

            
            return new Matrix(M); 
        }
        public string MatrixToLatex(string ID)
        {
            string Latex = "";
            for (int row = 0; row < this.rows; row++)
            {
                for (int column = 0; column < this.columns - 1; columns++)
                {
                    Latex += this.Values[row,column].ToString() + "&";
                }
                Latex += this.Values[row, this.columns-1].ToString() + @"\\";
            }

            return Latex;
        }
    }

    
}
