using Interface;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Rational Evaluate(Dictionary<string, Rational> Variables, double x = 1)
        {
            if(!Variables.ContainsKey("x"))
                Variables.Add("x", x);
            else
                Variables["x"] = x;
            return InputHandling.evaluatePostfix(postFixFunction, Variables);
        }
        public double Evaluate(Dictionary<string, double> Variables, double x = 1)
        {
            if (!Variables.ContainsKey("x"))
                Variables.Add("x", x);
            else
                Variables["x"] = x;
            return InputHandling.evaluatePostfix(postFixFunction, Variables);
        }

        public Matrix Evaluate( Dictionary<string, Rational> Variables, Dictionary<string, Matrix> Matrices)
        {
            return InputHandling.evaluatePostfixMatrix(postFixFunction, Variables,Matrices);
        }
        public Matrix Evaluate(Dictionary<string, double> Variables, Dictionary<string, Matrix> Matrices)
        {
            return InputHandling.evaluatePostfixMatrix(postFixFunction, Variables, Matrices);
        }


        public (double[], double[]) Evaluate(int lower_bound, int range, double step, Dictionary<string, Rational> Variables)
        {
            if (!Variables.ContainsKey("x"))
                Variables.Add("x", lower_bound);
            int index = 0;


            List<double> Xvals = new List<double>();
            List<double> Yvals = new List<double>();
            for (double x = lower_bound; x <= lower_bound + range; x += step)
            {
                Variables["x"] = x;
                Xvals.Add(x);
                Yvals.Add(InputHandling.evaluatePostfix(postFixFunction, Variables));

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
