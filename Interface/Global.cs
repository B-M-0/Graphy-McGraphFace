using ScottPlot.Ticks.DateTimeTickUnits;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Global
    {
        public static int ThemeIndex = 6;
        public static int angleFormatIndex= 0;
        public static int decimalPlaces = 7;
        public static string[] colours = { "Red", "Green", "Teal", "OrangeRed", "Purple" }; 


        public static double InputAngleConversion(double input)
        {
            
            switch (angleFormatIndex)
            {
                case 0:
                    return input;
                case 1:
                    return input * Math.PI / 180;
                case 2:
                    return input * Math.PI / 200;
                default:
                    return input;
            }
        }
        public static double OutputAngleConversion(double input)
        {
            switch (angleFormatIndex)
            {
                case 0:
                    return input;
                case 1:
                    return input * 180 / Math.PI;
                case 2:
                    return input * 200/Math.PI;
                default:
                    return input;
            }
        }
        static public string RemoveWhiteSpace(string input)
        {
            string temp = "";
            foreach (char element in input)
            {
                if (element != ' ')
                    temp += element;
            }
            return temp;
        }

        public static double HalfLine(double X, double angle, double y_1, double x_1)
        {
            double M = Math.Tan(angle);
            if (((angle < Math.PI / 2 && angle > -Math.PI / 2) && X < x_1) || ((angle > Math.PI / 2 || angle < -Math.PI / 2) && X > x_1))
            {

                return double.NaN;
            }

            double Y = M * (X - x_1) + y_1;

            return Y;
        }
        public static double Line(double X, double M, double y_1, double x_1)
        {
            double Y = M * (X - x_1) + y_1;

            return Y;
        }


        static public Dictionary<string, ComplexNumber> DefineConstants()
        {
            return new Dictionary<string, ComplexNumber> { { "e", Math.E }, { "\\pi", Math.PI } };
        }
        static public Dictionary<string, double> DefineConstants<T>()
        {
           return new Dictionary<string, double> { { "e",Math.E }, { "\\pi", Math.PI } };
        }
        static public Dictionary<string, Matrix> DefineCommonMatrices()
        {
            Dictionary<string, Matrix> matrices = new Dictionary<string, Matrix>();

            Matrix UnitSquare = new Matrix(new double[2, 4] { { 0, 1, 1, 0 }, { 0, 0, 1, 1 } });
            Matrix UnitTriangle = new Matrix(new double[2, 3] { { 0, 1, 0.5 }, { 0, 0, Math.Sqrt(3) / 2 } });
            matrices.Add("U", UnitSquare);
            matrices.Add("T", UnitTriangle);
            return matrices;
        }

    }
}
