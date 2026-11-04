using Interface;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        public double Evaluate(Dictionary<string, double> variables, ref bool isValidInput, int functionID, ref List<double>[] asymptotes, double x = 1)
        {
            variables["x"] = x;

            if (isValidInput == false)
                return 0;
            return InputHandling.EvaluatePostfix(postFixFunction, variables, functionID, ref asymptotes, ref isValidInput);
        }
        public ComplexNumber Evaluate(Dictionary<string, Surd> surdVariables, Dictionary<string, ComplexNumber> variables, ref double decimalVal, ref Surd surdVal, double z = 1)
        {
            variables = Utilities.DefineConstants();
            variables["z"] = z;
            return InputHandling.EvaluatePostfix(postFixFunction, surdVariables, variables, ref decimalVal, ref surdVal);
        }
        public Matrix Evaluate(Dictionary<string, double> variables, Dictionary<string, Matrix> matrices, Dictionary<string, Vector> vectors, ref double decimalVal)
        {
            return InputHandling.EvaluatePostfix_Matrices(postFixFunction, variables, matrices, vectors, ref decimalVal);
        }
    }
}
