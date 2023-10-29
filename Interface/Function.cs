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

        public double Evaluate(Dictionary<string, double> Variables, ref bool validinput ,  double x = 1)
        {
            if (!Variables.ContainsKey("x"))
                Variables.Add("x", x);
            else
                Variables["x"] = x;
            if (validinput == false)
                return 0;
            return InputHandling.evaluatePostfix(postFixFunction, Variables, ref validinput);
        }

        public ComplexNumber Evaluate(Dictionary<string, Surd> SurdVariables, Dictionary<string, ComplexNumber> Variables, ref double decimalval, ref Surd Surdval, double x = 1)
        {
            if (!Variables.ContainsKey("z"))
                Variables.Add("z", x);
            else
                Variables["z"] = x;
            return InputHandling.evaluatePostfix(postFixFunction, SurdVariables, Variables, ref decimalval , ref  Surdval);
        }


        public Matrix Evaluate(Dictionary<string, double> Variables, Dictionary<string, Matrix> Matrices, Dictionary<string, Vector> Vectors , ref double decimalval)
        {
            return InputHandling.evaluatePostfixMatrix(postFixFunction, Variables, Matrices, Vectors, ref decimalval);
        }

        public Matrix Evaluate( Dictionary<string, Rational> Variables, Dictionary<string, Matrix> Matrices, Dictionary<string, Vector> Vectors, ref double decimalval)
        {
            return InputHandling.evaluatePostfixMatrix(postFixFunction, Variables, Matrices, Vectors, ref decimalval);
        }
    }
}
