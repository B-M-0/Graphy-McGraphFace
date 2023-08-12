using Interface;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NEA26
{
    class Function
    {
        public Function(List<string> postFix)
        {
            postFixFunction = postFix;
        }

        List<string> postFixFunction;

        public double Evaluate(double x , Dictionary<string, double> Variables)
        {
            if(!Variables.ContainsKey("x"))
                Variables.Add("x", x);
            else
                Variables["x"] = x;
            return InputHandling.evaluateInfix(postFixFunction, Variables);
        }

        

        public (double[], double[]) Evaluate(int lower_bound, int range, double step, Dictionary<string, double> Variables)
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
                Yvals.Add(InputHandling.evaluateInfix(postFixFunction, Variables));

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
