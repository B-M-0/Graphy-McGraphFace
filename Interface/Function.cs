using Interface;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;//?why
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    class Function
    {
        public Function(List<string> postFix)
        {
            postFixFunction = postFix;
        }

        List<string> postFixFunction;

        public List<string> GetFunction()
        { 
            return postFixFunction; 
        }

        public Rational Evaluate(Dictionary<string, Surd> SurdVariables, Dictionary<string, Rational> Variables, ref double decimalval, double x = 1)
        {
            if (!Variables.ContainsKey("x"))
                Variables.Add("x", x);
            else
                Variables["x"] = x;
            return InputHandling.evaluatePostfix(postFixFunction, SurdVariables, Variables, ref decimalval);
        }

        public double Evaluate(Dictionary<string, double> Variables, double x = 1)
        {
            if (!Variables.ContainsKey("x"))
                Variables.Add("x", x);
            else
                Variables["x"] = x;
            return InputHandling.evaluatePostfix(postFixFunction, Variables);
        }

        public Complex Evaluate(Dictionary<string, Complex> Variables, double x = 0)
        {
            if (!Variables.ContainsKey("z"))
                Variables.Add("z", x);
            else
                Variables["z"] = x;
            return InputHandling.evaluatePostfix(postFixFunction, Variables);
        }


        public Matrix Evaluate(Dictionary<string, double> Variables, Dictionary<string, Matrix> Matrices, Dictionary<string, Vector> Vectors , ref double decimalval)
        {
            return InputHandling.evaluatePostfixMatrix(postFixFunction, Variables, Matrices, Vectors, ref decimalval);
        }

        public Matrix Evaluate( Dictionary<string, Rational> Variables, Dictionary<string, Matrix> Matrices, Dictionary<string, Vector> Vectors, ref double decimalval)
        {
            return InputHandling.evaluatePostfixMatrix(postFixFunction, Variables, Matrices, Vectors, ref decimalval);
        }

       


        public (double[], double[]) Evaluate(int lower_bound, int range, double step, Dictionary<string, Surd> SurdVariables, Dictionary<string, Rational> Variables)
        {
            if (!Variables.ContainsKey("x"))
                Variables.Add("x", lower_bound);
            int index = 0;
            double decval =0;

            List<double> Xvals = new List<double>();
            List<double> Yvals = new List<double>();
            for (double x = lower_bound; x <= lower_bound + range; x += step)
            {
                Variables["x"] = x;
                Xvals.Add(x);
                Yvals.Add(InputHandling.evaluatePostfix(postFixFunction,SurdVariables, Variables, ref decval));

                if (Yvals[index] >= (int.MaxValue - 1000000) || int.MinValue + 1000000 > Yvals[index] )
                {
                    Yvals.RemoveAt(index);
                    Xvals.RemoveAt(index);
                }
                else
                    index++;

            }

            

            double[] xValsArr = new double[Xvals.Count()];
            double[] yValsArr = new double[Yvals.Count()];
            for (int i = 0; i < Xvals.Count(); i++)
            {
                xValsArr[i] = Xvals[i];
                yValsArr[i] = Yvals[i];
            }
            return (xValsArr, yValsArr);
        }
    }

}
