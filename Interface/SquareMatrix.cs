using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class SquareMatrix : Matrix
    {

        //For Testing
        public SquareMatrix(int a = 2, bool empty = true) : base(a, a)
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
                        values[i, j] = r.Next(0, 10);
                    }
                }
            }
        }
        //Constructors
        public SquareMatrix(Rational[,] matrixElements) : base(matrixElements) { }
        public SquareMatrix(int[,] matrixElements) : base(matrixElements) { }
        public SquareMatrix(double[,] matrixElements) : base(matrixElements) { }
        public SquareMatrix(Matrix A) : base(A.rows, A.columns)
        {
            if (A.rows != A.columns)
            {
                throw new Exception("Matrix is not Square");
            }
            this.values = A.GetValues();
        }
        public SquareMatrix(Vector[] vectors): base(vectors.Length)
        {                
            for (int i = 0; i < vectors.Length; i++)
            {
                for (int j = 0; j < vectors.Length; j++)
                {
                    this.values[j, i] = vectors[i][j];
                }
            }
        }
        
        //Operators
        public static SquareMatrix operator ^(SquareMatrix A, int power)
        {
            SquareMatrix B = A;

            if (power < -1)
                throw new ArgumentException("Powers < -1 are unsupported");

            switch (power)
            {
                case -1:
                    B = A.Inverse();
                    break;
                case 0:
                    B = Identity(A.rows);
                    break;
                default:
                    for (int i = 1; i < power; i++)
                        B = new SquareMatrix(B * A);
                    break;
            }
            return B;
        }
        public static SquareMatrix operator ^(SquareMatrix A, string operation)
        {
            SquareMatrix B;
            switch (operation)
            {
                case "A":
                    B = Adjugate(A);
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

        //Matrix operations
        public Rational Trace()
        {
            Rational sum = 0;
            for (int i = 0; i < this.rows; i++)
                sum += this.values[i, i];
            return sum;
        }
        public static SquareMatrix Identity(int dimension)
        {
            dimension = dimension % 12;
            SquareMatrix I = new SquareMatrix(dimension, empty: true);
            for (int i = 0; i < dimension; i++)
            {
                I.values[i, i] = 1;
            }
            return I;
        }

        public static Rational Determinant(SquareMatrix A)
        {
            Rational det = 0;

            if (A.rows == 2)
            {
                // Calculates the determinant if the matrix is 2x2
                det = (A.values[0, 0] * A.values[1, 1]) - (A.values[1, 0] * A.values[0, 1]);
            }
            else if (A.rows == 1)
            {
                //if the matrix is a 1x1 it returns the sole element as the determinant
                det = A.values[0, 0];
            }
            else
            {
                // this iterates through every pivot value along the top row
                // and multiplies it with its corresponding minor value
                // signs to calculate the determinant properly
                for (int i = 0; i < A.rows; i++)
                {
                    det += (A.values[0, i] * (Rational)Math.Pow(-1, i)) * A.GetMinorElement(0, i);
                }
            }

            return (Rational)det;
        }
        public Rational GetMinorElement(int targetRow, int targetColumn)
        {
            SquareMatrix minorMatrix = new SquareMatrix(this.rows - 1);
            int minorRow = 0;
            for (int currentRow = 0; currentRow < this.rows; currentRow++)
            {
                int minorColumn = 0;
                if (currentRow == targetRow)
                    continue;
                for (int currentColumn = 0; currentColumn < this.columns; currentColumn++)
                {
                    if (currentColumn == targetColumn)
                        continue;

                    minorMatrix.values[minorRow, minorColumn] = this.values[currentRow, currentColumn];
                    minorColumn++;
                }
                minorRow++;
            }
            return Determinant(minorMatrix);
        }
        public static SquareMatrix Minor(SquareMatrix A)
        {
            //Calculates a matrix of minor elements
            SquareMatrix MinorValueMatrix = new SquareMatrix(A.rows);


            for (int minorValueMatrixRow = 0; minorValueMatrixRow < A.rows; minorValueMatrixRow++)
            {
                for (int minorValueMatrixColumn = 0; minorValueMatrixColumn < A.columns; minorValueMatrixColumn++)
                {
                    MinorValueMatrix.values[minorValueMatrixRow, minorValueMatrixColumn] = A.GetMinorElement(minorValueMatrixRow, minorValueMatrixColumn);
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
                    A.values[row, column] = (int)Math.Pow(-1, row + column + 2) * A.values[row, column];
                }
            }
            return A;
        }
        public static SquareMatrix Transpose(SquareMatrix A)
        {
            SquareMatrix B = new SquareMatrix(A.rows);
            int matrixOrder = A.rows;
            for (int i = 0; i < A.values.Length; i++)
            {
                B.values[i % matrixOrder, i / matrixOrder] = A.values[i / matrixOrder, i % matrixOrder];
            }
            return B;
        }
        public static SquareMatrix Adjugate(SquareMatrix A)
        {
            A = A ^ "M";
            A = A ^ "C";
            A = A ^ "T";
            return A;
        }
        public SquareMatrix Inverse()
        {
            Rational detA = Determinant(this);
            if (detA.Decimal() == 0)
                throw new DivideByZeroException("Determinant = 0 => A is singular => no inverse.");

            if (this.rows == 1)
            {
                Rational[,] MatrixArr = new Rational[1, 1];
                MatrixArr[0, 0] = detA.Reciprocal();
                SquareMatrix Inverse = new SquareMatrix(MatrixArr);
                return Inverse;
            }

            return (SquareMatrix)(detA.Reciprocal() * (this ^ "A"));
        }

        //Factorisation A = QR
        public static (SquareMatrix,SquareMatrix) QR_Decomposition(SquareMatrix A)
        {
            Vector[] a = new Vector[A.columns];
            for (int i = 0; i < A.columns; i++)
                a[i] = A.GetColumn(i);

            Vector[] e = Vector.GramSchmidtOrthonormalization(a);

            SquareMatrix Q = new SquareMatrix(e);
            SquareMatrix R = new SquareMatrix((Transpose(Q)) * A);
            return (Q, R);
        }
        public static SquareMatrix QR_Q(SquareMatrix A)
        {
            return QR_Decomposition(A).Item1;
        }
        public static SquareMatrix QR_R(SquareMatrix A)
        {
            return QR_Decomposition(A).Item2;
        }


    }
}
