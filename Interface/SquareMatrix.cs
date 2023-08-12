using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace  Interface
{
    public class SquareMatrix : Matrix
    {
        public SquareMatrix(int a = 2, bool empty = false) : base(a, a)
        {
            //Test values
            if (!empty)
            {
                int x = a;
                Random r = new Random();
                for (int i = 0; i < a; i++)
                {
                    for (int j = 0; j < a; j++)
                    {
                        Values[i, j] = r.Next(0, 10);
                    }
                }
            }
        }
       

        public SquareMatrix(Rational[,] MatrixElements) : base(MatrixElements) { }
        public SquareMatrix(int[,] MatrixElements) : base(MatrixElements) { }
        public SquareMatrix(double[,] MatrixElements) : base(MatrixElements) { }

        public static SquareMatrix identityMatrix(int dimension )
        { 
            SquareMatrix I = new SquareMatrix(dimension, true);
            for (int i = 0; i < dimension;i++ )
            {
                I.Values[i,i] = 1;    
            }
            return I;
        }

        public static Matrix operator ^(SquareMatrix A, int Power)
        {
            SquareMatrix B = A;

            if(Power < -1)
                throw new ArgumentException("Powers < -1 are unsupported");
            
            switch (Power)
            {
                case -1:
                    B = A.Inverse();
                    break;
                case 0:
                    B = identityMatrix(A.rows);
                    break;
                default:
                    for (int i = 1; i < Power; i++)  B = (SquareMatrix)(B * A);
                    break;
            }
            return B;
        }

        public static SquareMatrix operator ^(SquareMatrix A, string Operation)
        {
            SquareMatrix B;
            switch (Operation)
            {
                case "A":
                    B = Adjoint(A);
                    break;
                case "M":
                    B = Minor(A);
                    break;
                case "T":
                    B = Transpose(A);
                    break;
                case "C":
                    B = Cofactor(A);
                    break;
                default:
                    throw new ArgumentException("Invalid Operation");
            }
            return B;
        }

        public Rational trace()
        {
            Rational sum = 0;
            for (int i = 0; i < this.rows; i++)
                sum += this.Values[i, i];
            return sum;
        }

        public static Rational determinant(SquareMatrix A)
        {
            Rational det = 0;

            if (A.rows == 2)
            {
                // Calculates the determinant if the order of the matrix is 2x2
                det = (A.Values[0, 0] * A.Values[1, 1]) - (A.Values[1, 0] * A.Values[0, 1]);
            }
            else if (A.rows == 1)
            {
                //if the matrix is a 1x1 it returns the sole element as the determinant
                det = A.Values[0, 0];
            }
            else
            {
                // this iterates through every pivot value along the top row
                // and multiplies it with its corresponding minor value
                // signs to calculate the determinant properly
                for (int i = 0; i < A.rows; i++)
                {   
                    det += (A.Values[0, i] * (Rational)Math.Pow(-1, i)) * A.getMinorElement(0,i);
                }
            }

            return (Rational)det;
        }
        public  Rational getMinorElement(int minorValueMatrixRow, int minorValueMatrixColumn)
        {
            SquareMatrix Minor = new SquareMatrix(this.rows - 1);
            int MinorRow = 0;
            for (int currentRow= 0; currentRow< this.rows; currentRow++)
            {
                int MinorColumn = 0;
                if (currentRow== minorValueMatrixRow)
                    continue;
                for (int currentColumn = 0; currentColumn < this.columns; currentColumn++)
                {
                    if (currentColumn == minorValueMatrixColumn)
                        continue;

                    Minor.Values[MinorRow, MinorColumn] = this.Values[currentRow, currentColumn];
                    MinorColumn++;
                }
                MinorRow++;
            }
            return determinant(Minor);
        }

        public static SquareMatrix Minor(SquareMatrix A)
        {
            //Calculates a matrix of minor elements
            SquareMatrix MinorValueMatrix = new SquareMatrix(A.rows);


            for (int minorValueMatrixRow = 0; minorValueMatrixRow < A.rows; minorValueMatrixRow++)
            {
                for (int minorValueMatrixColumn = 0; minorValueMatrixColumn < A.columns; minorValueMatrixColumn++)
                {
                    MinorValueMatrix.Values[minorValueMatrixRow, minorValueMatrixColumn] = A.getMinorElement(minorValueMatrixRow,minorValueMatrixColumn);
                }
            }
            return MinorValueMatrix;
        }
        public static SquareMatrix Cofactor(SquareMatrix A)
        {
            for (int row = 0; row < A.rows; row++)
            {
                for (int column = 0; column < A.columns; column++)
                {
                    A.Values[row, column] = (int)Math.Pow(-1, row + column + 2) * A.Values[row, column];
                }
            }
            return A;
        }
        public static SquareMatrix Transpose(SquareMatrix A)
        {
            SquareMatrix B = new SquareMatrix(A.rows);
            int matrixOrder = A.rows;
            for (int i = 0; i < A.Values.Length; i++)
            {
                B.Values[i % matrixOrder, i / matrixOrder] = A.Values[i / matrixOrder, i % matrixOrder];
            }
            return B;
        }
        public static SquareMatrix Adjoint(SquareMatrix A)
        {
            A = A ^ "M";
            A = A ^ "C";
            A = A ^ "T";
            return A;
        }

        public (Vector,Rational) Eigen(SquareMatrix A)
        {
            Vector EigenVector = new Vector(A.rows);
            Rational EigenValue = 1;
            
            
            return (EigenVector, EigenValue);
        }


        public SquareMatrix Inverse()
        {
            Rational detA = determinant(this);
            if (detA.Decimal() == 0)
                throw new DivideByZeroException("Determinant = 0 => A is singular => no inverse.");

            if (this.rows == 1)      
            {
                Rational[,] MatrixArr = new Rational[1, 1];
                MatrixArr[0, 0] = detA.reciprocal();
                SquareMatrix Inverse = new SquareMatrix(MatrixArr);
                return Inverse;
            }
 
            return (SquareMatrix)(detA.reciprocal() * (this ^ "A"));
        }
    }
}
