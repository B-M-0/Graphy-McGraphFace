using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Diagnostics;
using System.Windows.Navigation;
using System.Diagnostics.Eventing.Reader;

namespace Interface
{

    public class Matrix
    {        
        public readonly int rows, columns;
        protected  Rational[,] values;

        public Rational this[int row,int column]
        { 
            get => values[row, column];
            set => values[row, column] = value;
        }

        public Matrix(Rational[,] matrixElements) : this(matrixElements.GetLength(0), matrixElements.GetLength(1))
        {
            _2DArrayToMatrix(matrixElements);
        }
        public Matrix(double[,] matrixElements) : this(matrixElements.GetLength(0), matrixElements.GetLength(1))
        {
            _2DArrayToMatrix(matrixElements);
        }
        public Matrix(int[,] matrixElements) : this(matrixElements.GetLength(0), matrixElements.GetLength(1))
        {
            _2DArrayToMatrix(matrixElements);
        }

        public Matrix(Rational[] vectorElements) : this(vectorElements.GetLength(0), 1)
        {
            for (int i = 0; i < this.rows; i++)
                this.values[i, 0] = vectorElements[i];
        }
        public Matrix(double[] vectorElements) : this(vectorElements.GetLength(0), 1)
        {
            for (int i = 0; i < this.rows; i++)
                this.values[i, 0] = vectorElements[i];
        }
        public Matrix(int[] vectorElements) : this(vectorElements.GetLength(0), 1)
        {
            for (int i = 0; i < this.rows; i++)
                this.values[i, 0] = vectorElements[i];
        }

        public Matrix(Rational matrixElement) : this(1, 1)
        {
            _2DArrayToMatrix(new Rational[1, 1] { { matrixElement } });
        }
        public Matrix(double matrixElement) : this(1, 1)
        {
            _2DArrayToMatrix(new double[1, 1] { { matrixElement } });
        }
        public Matrix(int matrixElement) : this(1, 1)
        {
            _2DArrayToMatrix(new int[1, 1] { { matrixElement } });
        }

        public Matrix(int a = 2, int b = 2)
        {
            rows = a;
            columns = b;
            values = new Rational[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    values[i, j] = 0;
                }
            }
        }
       
        public static T[,] Convert2DListToArray<T>(List<List<T>> list)
        {
            T[,] array = new T[list.Count(), list[0].Count()];

            for (int row = 0; row < list.Count(); row++)
            {
                for (int column = 0; column < list[0].Count(); column++)
                {
                    array[row, column] = list[row][column];
                }
            }
            return array;
        }
        protected void _2DArrayToMatrix(Rational[,] matrixElements)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    values[i, j] = matrixElements[i, j];
                }
            }
        }
        protected void _2DArrayToMatrix(int[,] matrixElements)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    values[i, j] = matrixElements[i, j];
                }
            }
        }
        protected void _2DArrayToMatrix(double[,] matrixElements)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    values[i, j] = Math.Round(matrixElements[i, j], Utilities.decimalPlaces);
                }
            }
        }

        public Vector GetColumn(int columnIndex)
        {
            Vector output = new Vector(this.rows);
            for (int i = 0; i < this.rows; i++)
            {
                output[i] = this.values[i, columnIndex];
            }
            return output;
        }
        public  Rational[,] GetValues()
        {
            return this.values;
        }

        public bool ValuesNotNull()
        {
            foreach (Rational element in this.values)
            {
                if (element == null)
                    return false;
            }
            return true;
        }
        
        
        //Arithmetic operations.

        //Addition
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
                        C.values[i, j] = A.values[i, j] + (B.values[i, j]);
                    }
                }
            }
            else
            {
                throw new ArgumentException();
            }
            return C;
        }

        //Subtraction
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
                        C.values[i, j] = A.values[i, j] - (B.values[i, j]);
                    }
                }
            }
            else
            {
                throw new ArgumentException();
            }
            return C;
        }

        //Matrix* Multiplication
        public static Matrix operator *(Matrix A, Matrix B)
        {
            Matrix C;
            
            if (A.columns == B.rows && A.ValuesNotNull() && B.ValuesNotNull())
            {
                C = new Matrix(A.rows, B.columns);

                for (int row = 0; row < C.rows; row++)
                {
                    for (int column = 0; column < C.columns; column++)
                    {
                        Rational sum = 0;
                        for (int i = 0; i < A.columns; i++)
                        {
                            sum += (A.values[row, i] * B.values[i, column]);
                        }
                        C.values[row, column] = sum;
                    }
                }
            }
            else
            {
                throw new ArgumentException();
            }
            return C;
        }

        //Multiply  by  scalars
        public static Matrix operator *(Rational scalar, Matrix A)
        {
            for (int i = 0; i < A.rows; i++)
            {
                for (int j = 0; j < A.columns; j++)
                {
                    A.values[i, j] = A.values[i, j] * scalar;
                }
            }
            return A;
        }
        public static Matrix operator *(Matrix A, Rational scalar)
        {
            return scalar * A;
        }



        //parses a set of sets in the form M:{{a,b,c},{d,e,f},{g,h,i}}
        public static Matrix StringToMatrix(string input)
        {
            //Validation
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
                    MatrixElements.Add(InputHandling.ParseList(sublistStr));
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
        
        /*public static explicit operator string(Matrix A)
        {
            return A.MatrixToLatex();
        }*/

        static public Matrix LatexToMatrix(string input = @"\pmatrix{a&b\\c&d\\}")
        {
            int rowCount = 0;


            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input.Substring(i, 2) == @"\\")
                {
                    input = input.Substring(0, i) + "%" + input.Substring(i + 2);
                    i = 0;
                    rowCount++;
                }
            }
            string[] rows = input.Split('%');


            Rational[,] M = new Rational[rowCount, rows[0].Split('&').Length];
            for (int i = 0; i < rows.Length - 1; i++)
            {
                string[] row = rows[i].Split('&');
                for (int j = 0; j < row.Length; j++)
                {
                    if (row[j].Contains('_'))
                        row[j] = "-" + row[j].Substring(1);
                    M[i, j] = (Rational)(double.Parse(row[j]));
                }
            }


            return new Matrix(M);
        }
        public string MatrixToLatex()
        {

            string Latex = @"\pmatrix{";
            for (int row = 0; row < this.rows; row++)
            {
                for (int column = 0; column < this.columns - 1; column++)
                {
                    if(this.values[row, column] == null)    
                        return "format error";
                    Latex += this.values[row, column].LatexFormat() + "&";
                }
                if (this.values[row, this.columns-1] == null)
                    return "format error";
                Latex += this.values[row, this.columns - 1].LatexFormat() + @"\\";
            }

            return Latex + "}";
        }
    }
}
