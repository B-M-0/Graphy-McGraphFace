using ScottPlot.Ticks.DateTimeTickUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Global
    {

        public static double HalfLine(double X,double angle, double y_1, double x_1)
        {
            double M = Math.Tan(angle);
            if(((angle < Math.PI/2 && angle > -Math.PI/2) && X < x_1) || ((angle > Math.PI / 2 || angle < -Math.PI / 2) && X > x_1))
            { 
          
                return double.NaN;
            }
            
            double Y = M*(X - x_1) + y_1;

            return Y;
        }
        public static double Line(double X, double M, double y_1, double x_1)
        {
            double Y = M * (X - x_1) + y_1;

            return Y;
        }

        public static Dictionary<string,Surd> LociDictionary_S = new Dictionary<string,Surd>();
        public static Dictionary<string, ComplexNumber> LociDictionary_C = new Dictionary<string, ComplexNumber>();

        public static int ThemeIndex = 6;
        public static int angleFormatIndex= 0;
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




        static public string removeWhiteSpace(string input)
        {
            string temp = "";
            foreach (char element in input)
            {
                if (element != ' ')
                    temp += element;
            }
            return temp;
        }


        static public int decimalPlaces = 7;
        /*static public Dictionary<string,Rational> defineConstants()
        {
            return new Dictionary<string, Rational> { { "e", Math.Round(Math.E, Global.decimalPlaces) }, { "\\pi", Math.Round(Math.PI, Global.decimalPlaces) } };
        }*/
        static public Dictionary<string, ComplexNumber> defineConstants()
        {
            return new Dictionary<string, ComplexNumber> { { "e", Math.E }, { "\\pi", Math.PI } };
        }

        static public Dictionary<string, double> defineConstants<T>()
        {
           return new Dictionary<string, double> { { "e",Math.E }, { "\\pi", Math.PI } };
        }


    }
}
