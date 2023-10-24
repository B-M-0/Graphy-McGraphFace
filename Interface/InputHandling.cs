using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Interface
{
    //  \sqrt{23}


    static class InputHandling
    {

        static Random random = new Random();
        static List<string> FunctionCodes = new List<string>()
        {
            "\\sqrt","abs",
            "mod", "arg",
            "cos", "sin", "tan",
            "cosh", "sinh", "tanh",
            "exp", "log", "ln",
            "arccos", "arcsin", "arctan",
            "arccosh", "arcsinh", "arctanh",
            "\\M" , "\\V", "det" ,"dot","cross"
        };
        public static ComplexNumber evaluatePostfix(List<string> expression, Dictionary<string, Surd> SurdVariables, Dictionary<string, ComplexNumber> Variables, ref double decimalval)
        {
            Variables = Global.defineConstants();
            Stack<string> stack = new Stack<string>();
            foreach (string element in expression)
            {
                string operand1Str, operand2Str;
                Rational operand1_R, operand2_R;
                ComplexNumber operand1_C, operand2_C;
                Surd operand1_S, operand2_S;
                double operand1, operand2;
                string key;

                switch (element)
                {
                    case "arg":
                        operand1_C = Variables[stack.Pop()];
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(ComplexNumber.Arg(operand1_C)));

                        stack.Push(key);
                        break;
                    case "abs":
                    case "mod":
                        operand1_C = Variables[stack.Pop()];
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(ComplexNumber.Abs(operand1_C)));

                        stack.Push(key);
                        break;
                    case "\\sqrt":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Math.Sqrt(operand1)));

                        stack.Push(key);
                        break;
                    case "exp":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Math.Exp(operand1)));

                        stack.Push(key);
                        break;
                    case "sin":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Math.Sin(Global.InputAngleConversion(operand1))));

                        stack.Push(key);
                        break;
                    case "cos":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Math.Cos(Global.InputAngleConversion(operand1))));

                        stack.Push(key);
                        break;
                    case "tan":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Math.Tan(Global.InputAngleConversion(operand1))));

                        stack.Push(key);
                        break;
                    case "sinh":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Math.Sinh(Global.InputAngleConversion(operand1))));

                        stack.Push(key);
                        break;
                    case "cosh":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Math.Cosh(Global.InputAngleConversion(operand1))));

                        stack.Push(key);
                        break;
                    case "tanh":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Math.Tanh(Global.InputAngleConversion(operand1))));

                        stack.Push(key);
                        break;
                    case "arcsin":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Global.OutputAngleConversion(Math.Asin(operand1))));

                        stack.Push(key);
                        break;
                    case "arccos":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Global.OutputAngleConversion(Math.Acos(operand1))));
                        stack.Push(key);
                        break;
                    case "arctan":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Global.OutputAngleConversion(Math.Atan(operand1))));
                        stack.Push(key);
                        break;
                    case "arcsinh":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Global.OutputAngleConversion(Math.Asinh(operand1))));

                        stack.Push(key);
                        break;
                    case "arccosh":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Global.OutputAngleConversion(Math.Acosh(operand1))));
                        stack.Push(key);
                        break;
                    case "arctanh":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Global.OutputAngleConversion(Math.Atanh(operand1))));
                        stack.Push(key);
                        break;
                    case "log":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Math.Log10(operand1)));

                        stack.Push(key);
                        break;
                    case "ln":
                        operand1_R = Variables[stack.Pop()].real;
                        operand1 = operand1_R.Decimal();
                        key = GenerateKey(Variables);
                        Variables.Add(key, new Rational(Math.Log(operand1)));

                        stack.Push(key);
                        break;
                    case "+":
                        operand1_C = Variables[stack.Pop()];
                        operand2_C = Variables[stack.Pop()];

                        key = GenerateKey(Variables);
                        Variables.Add(key, operand2_C + operand1_C);

                        stack.Push(key);
                        break;
                    case "-":
                        operand1_C = Variables[stack.Pop()];
                        try { operand2_C = Variables[stack.Pop()]; }
                        catch { operand2_C = 0; }
                        key = GenerateKey(Variables);
                        Variables.Add(key, operand2_C - operand1_C);

                        stack.Push(key);
                        break;
                    case "*":
                        operand1_C = Variables[stack.Pop()];
                        operand2_C = Variables[stack.Pop()];

                        key = GenerateKey(Variables);
                        Variables.Add(key, operand2_C * operand1_C);

                        stack.Push(key);
                        break;
                    case "/":
                        operand1_C = Variables[stack.Pop()];
                        operand2_C = Variables[stack.Pop()];

                        key = GenerateKey(Variables);
                        Variables.Add(key, operand2_C / operand1_C);

                        stack.Push(key);
                        break;
                    case "^":
                        operand1_R = Variables[stack.Pop()].real;
                        operand2_R = Variables[stack.Pop()].real;

                        key = GenerateKey(Variables);
                        Variables.Add(key, operand2_R ^ operand1_R);

                        stack.Push(key);
                        break;
                    default:
                        ComplexNumber temp = 1;
                        bool negative = false;
                        if (element.Contains("_"))
                        {
                            temp = -1;
                            negative = true;
                        }


                        try
                        {
                            if (Regex.IsMatch(element, "[0-9]*i"))
                            {
                                if (element == "i")
                                    temp *= new ComplexNumber(0, 1);
                                else
                                    temp *= new ComplexNumber(0, double.Parse(element.Substring(0, element.Length - 1)));
                            }
                            else if (Regex.IsMatch(element, "[A-Za-z\\e]+$") && negative)
                                temp *= Variables[element.Substring(1)].real;
                            else if (Regex.IsMatch(element, "[A-Za-z\\e]+$"))
                                temp *= Variables[element].real;
                            else if (negative)
                                temp *= double.Parse(element.Substring(1));
                            else
                                temp *= double.Parse(element);

                            key = GenerateKey(Variables);
                            Variables.Add(key, temp);
                            stack.Push(key);
                        }
                        catch { }
                        break;
                }

            }

            string outputstr = stack.Pop();
            ComplexNumber output = new ComplexNumber(1, 0);
            if (Regex.IsMatch(outputstr, "[A-Z]+$"))
            {
                output = Variables[outputstr];
                Variables.Clear();
                return output;
            }
            else
            {
                decimalval = double.Parse(outputstr);
                Variables.Clear();
                return null;
            }
        }
        public static double evaluatePostfix(List<string> expression, Dictionary<string, double> Variables, ref bool validinput)
        {
            validinput = true;
            Stack<string> stack = new Stack<string>();
            for (int iterator = 0; iterator < expression.Count() && validinput; iterator++)
            {
                string element = expression[iterator];
                try
                {
                    double operand1, operand2;
                    switch (element)
                    {

                        case "abs":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push((Math.Abs(operand1)).ToString());
                            break;
                        case "\\sqrt":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push((Math.Sqrt(operand1)).ToString());
                            break;
                        case "exp":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push((Math.Exp(operand1)).ToString());
                            break;
                        case "sin":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push((Math.Sin(Global.InputAngleConversion(operand1))).ToString());
                            break;
                        case "cos":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push((Math.Cos(Global.InputAngleConversion(operand1))).ToString());
                            break;
                        case "tan":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push((Math.Tan(Global.InputAngleConversion(operand1))).ToString());
                            break;
                        case "sinh":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push((Math.Sinh(Global.InputAngleConversion(operand1))).ToString());
                            break;
                        case "cosh":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push((Math.Cosh(Global.InputAngleConversion(operand1))).ToString());
                            break;
                        case "tanh":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push((Math.Tanh(Global.InputAngleConversion(operand1))).ToString());
                            break;
                        case "arcsin":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push(Global.OutputAngleConversion(Math.Asin(operand1)).ToString());
                            break;
                        case "arccos":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push(Global.OutputAngleConversion(Math.Acos(operand1)).ToString());
                            break;
                        case "arctan":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push(Global.OutputAngleConversion(Math.Atan(operand1)).ToString());
                            break;
                        case "arcsinh":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push(Global.OutputAngleConversion(Math.Asinh(operand1)).ToString());
                            break;
                        case "arccosh":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push(Global.OutputAngleConversion(Math.Acosh(operand1)).ToString());
                            break;
                        case "arctanh":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push(Global.OutputAngleConversion(Math.Atanh(operand1)).ToString());
                            break;
                        case "log":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push((Math.Log10(operand1)).ToString());
                            break;
                        case "ln":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push((Math.Log(operand1)).ToString());
                            break;
                        case "+":
                            operand1 = double.Parse(stack.Pop());
                            operand2 = double.Parse(stack.Pop());
                            stack.Push((operand2 + operand1).ToString());
                            break;
                        case "-":
                            operand1 = double.Parse(stack.Pop());
                            if (stack.Count() == 0)
                                operand2 = 0;
                            else
                                operand2 = double.Parse(stack.Pop());
                            stack.Push((operand2 - operand1).ToString());
                            break;
                        case "*":
                            operand1 = double.Parse(stack.Pop());
                            operand2 = double.Parse(stack.Pop());
                            stack.Push((operand2 * operand1).ToString());
                            break;
                        case "/":
                            operand1 = double.Parse(stack.Pop());
                            operand2 = double.Parse(stack.Pop());
                            if (operand1 == 0)
                                stack.Push("0");
                            else
                                stack.Push((operand2 / operand1).ToString());
                            break;
                        case "^":
                            operand1 = double.Parse(stack.Pop());
                            operand2 = double.Parse(stack.Pop());
                            stack.Push((Math.Pow(operand2, operand1)).ToString());
                            break;
                        default:
                            double temp = 1;
                            bool negative = false;
                            if (element.Contains("_"))
                            {
                                temp = -1;
                                negative = true;
                            }


                            try
                            {
                                if (Regex.IsMatch(element, "[a-z]+$") && negative)
                                    temp *= Variables[element.Substring(1)];
                                else if (Regex.IsMatch(element, "[a-z]+$"))
                                    temp *= Variables[element];
                                else if (negative)
                                    temp *= double.Parse(element.Substring(1));
                                else
                                    temp *= double.Parse(element);

                                stack.Push(temp.ToString());
                            }
                            catch
                            {
                                validinput = false;
                            }
                            break;
                    }
                }
                catch (Exception error)
                {
                    validinput = false;
                }
            }

            double output = double.Parse(stack.Pop());
            return output;
        }

        static public string IdentifyLoci(string input)
        {
            var temp = input.Split('=');
            string LHS = temp[0];
            string RHS = temp[1];

            if (LHS.Contains("mod(z") && RHS.Contains("mod(z"))
                return "B";//bisector
            else if (LHS.Contains("mod(z") && Regex.IsMatch(RHS, "[0-9.]"))
                return "C";//cirlce
            else if (LHS.Contains("arg(z") & Regex.IsMatch(RHS, "[0-9.]"))
                return "H";//halfline
            else
                return "U";//undefined
        }

        public static string GenerateKey<T>(Dictionary<string, T> Dictionary)
        {
            int a = 0;
            if (typeof(T).FullName == "Surd")
            {
                a = 5;
            }
            else a = 4;
            string key = "";
            for (int i = 0; i < a; i++)
            {
                key += (char)random.Next(65, 91);
            }
            if (Dictionary.ContainsKey(key))
                key = GenerateKey(Dictionary);

            return key;
        }

        public static Matrix evaluatePostfixMatrix(List<string> expression, Dictionary<string, Rational> Variables, Dictionary<string, Matrix> Matrices, Dictionary<string, Vector> Vectors, ref double DecimalVal)
        {
            Stack<string> stack = new Stack<string>();
            foreach (string element in expression)
            {
                string operand1, operand2;
                Matrix A, B, C;
                double a, b;
                string key;
                switch (element)
                {

                    case @"\M":
                        operand1 = stack.Pop();
                        Matrix NewMatrix = Matrix.LatexToMatrix(operand1);
                        key = GenerateKey(Matrices);
                        Matrices.Add(key, NewMatrix);
                        stack.Push(key);
                        break;
                    case "det":
                        operand1 = stack.Pop();
                        A = Matrices[operand1];
                        break;
                    case "+":

                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        if (Regex.IsMatch(operand1, "[0-9]+$") && Regex.IsMatch(operand2, "[0-9]+$"))
                        {
                            stack.Push((operand2 + operand1).ToString());
                        }
                        else
                        {
                            A = Matrices[operand1];
                            B = Matrices[operand2];
                            C = B + A;
                            key = GenerateKey(Matrices);
                            Matrices.Add(key, C);
                            stack.Push(key);
                        }
                        break;
                    case "-":
                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        if (Regex.IsMatch(operand1, "[0-9]+$") && Regex.IsMatch(operand2, "[0-9]+$"))
                        {
                            stack.Push((operand2 + operand1).ToString());
                        }
                        else
                        {
                            A = Matrices[operand1];
                            B = Matrices[operand2];
                            C = B - A;
                            key = GenerateKey(Matrices);
                            Matrices.Add(key, C);
                            stack.Push(key);
                        }
                        break;
                    case "*":
                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        if (Regex.IsMatch(operand1, "[A-Z]") && Regex.IsMatch(operand2, "[A-Z]"))
                        {
                            try
                            {
                                A = Matrices[operand1];
                                B = Matrices[operand2];
                                C = B * A;
                                Matrices.Add(operand2 + operand1, C);
                                stack.Push(operand2 + operand1);
                            }
                            catch (NullReferenceException e)
                            {
                                string msg = e.Message;
                            }
                        }
                        else if (Regex.IsMatch(operand1, "[A-Z]"))
                        {
                            A = Matrices[operand1];
                            C = double.Parse(operand2) * A;
                            key = GenerateKey(Matrices);
                            Matrices.Add(key, C);
                            stack.Push(key);
                        }
                        else if (Regex.IsMatch(operand2, "[A-Z]"))
                        {
                            B = Matrices[operand2];
                            C = double.Parse(operand1) * B;
                            key = GenerateKey(Matrices);
                            Matrices.Add(key, C);
                            stack.Push(key);
                        }
                        else
                        {
                            a = double.Parse(operand1);
                            b = double.Parse(operand2);
                            stack.Push((b * a).ToString());
                            break;
                        }
                        break;
                    case "/":
                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        a = double.Parse(operand1);
                        b = double.Parse(operand2);
                        stack.Push((b / a).ToString());
                        break;
                    case "^":
                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        A = Matrices[operand2];
                        int power = int.Parse(operand1);
                        C = new SquareMatrix(A) ^ power;
                        key = GenerateKey(Matrices);
                        Matrices.Add(key, C);
                        stack.Push(key);
                        break;
                    default:
                        string temp = "";
                        bool negative = false;

                        if (element[0] == '_' && element.Contains("\\\\") == false)
                        {
                            negative = true;
                            temp = element.Substring(1);
                        }
                        else
                            temp = element;

                        if (Regex.IsMatch(element, "[a-z]+$"))
                            temp = Variables[temp].ToString();


                        if (negative)
                        {
                            temp = ((-1) * double.Parse(temp)).ToString();
                        }
                        stack.Push(temp);

                        break;
                }

            }

            return Matrices[stack.Pop()];
        }
        public static Matrix evaluatePostfixMatrix(List<string> expression, Dictionary<string, double> Variables, Dictionary<string, Matrix> Matrices, Dictionary<string, Vector> Vectors, ref double DecimalVal)
        {
            Stack<string> stack = new Stack<string>();
            foreach (string element in expression)
            {
                string operand1, operand2;
                Matrix A, B, C;
                SquareMatrix sqA;
                Vector V1, V2, V3;
                double a, b;
                string key;
                switch (element)
                {
                    case @"\M":
                        operand1 = stack.Pop();
                        Matrix NewMatrix = Matrix.LatexToMatrix(operand1);
                        key = GenerateKey(Matrices);
                        Matrices.Add(key, NewMatrix);
                        stack.Push(key);
                        break;
                    case "cross":
                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        A = Matrices[operand1];
                        B = Matrices[operand2];
                        V1 = new Vector(A.Values);
                        V2 = new Vector(B.Values);
                        V3 = V2 + V1;
                        C = new Matrix(V3.GetValues());
                        key = GenerateKey(Matrices);
                        Matrices.Add(key, C);
                        stack.Push(key);
                        break;
                    case "dot":
                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        A = Matrices[operand1];
                        B = Matrices[operand2];
                        V1 = new Vector(A.Values);
                        V2 = new Vector(B.Values);
                        a = V2 * V1;
                        stack.Push(a.ToString());
                        break;
                    case "det":
                        operand1 = stack.Pop();
                        A = Matrices[operand1];
                        sqA = new SquareMatrix(A);
                        stack.Push(((double)SquareMatrix.determinant(sqA)).ToString());
                        break;
                    case "+":

                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        if (Regex.IsMatch(operand1, "[0-9]+$") && Regex.IsMatch(operand2, "[0-9]+$"))
                        {
                            stack.Push((operand2 + operand1).ToString());
                        }
                        else
                        {
                            A = Matrices[operand1];
                            B = Matrices[operand2];
                            C = B + A;
                            key = GenerateKey(Matrices);
                            Matrices.Add(key, C);
                            stack.Push(key);
                        }
                        break;
                    case "-":
                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        if (Regex.IsMatch(operand1, "[0-9]+$") && Regex.IsMatch(operand2, "[0-9]+$"))
                        {
                            stack.Push((operand2 + operand1).ToString());
                        }
                        else
                        {
                            A = Matrices[operand1];
                            B = Matrices[operand2];
                            C = B - A;
                            key = GenerateKey(Matrices);
                            Matrices.Add(key, C);
                            stack.Push(key);
                        }
                        break;
                    case "*":
                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        if (Regex.IsMatch(operand1, "[A-Z]") && Regex.IsMatch(operand2, "[A-Z]"))
                        {
                            try
                            {
                                A = Matrices[operand1];
                                B = Matrices[operand2];
                                C = B * A;
                                Matrices.Add(operand2 + operand1, C);
                                stack.Push(operand2 + operand1);
                            }
                            catch (NullReferenceException e)
                            {
                                string msg = e.Message;
                            }
                        }
                        else if (Regex.IsMatch(operand1, "[A-Z]"))
                        {
                            A = Matrices[operand1];
                            C = double.Parse(operand2) * A;
                            key = GenerateKey(Matrices);
                            Matrices.Add(key, C);
                            stack.Push(key);
                        }
                        else if (Regex.IsMatch(operand2, "[A-Z]"))
                        {
                            B = Matrices[operand2];
                            C = double.Parse(operand1) * B;
                            key = GenerateKey(Matrices);
                            Matrices.Add(key, C);
                            stack.Push(key);
                        }
                        else
                        {
                            a = double.Parse(operand1);
                            b = double.Parse(operand2);
                            stack.Push((b * a).ToString());
                            break;
                        }
                        break;
                    case "/":
                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        a = double.Parse(operand1);
                        b = double.Parse(operand2);
                        stack.Push((b / a).ToString());
                        break;
                    case "^":
                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        A = Matrices[operand2];
                        int power = int.Parse(operand1);
                        C = new SquareMatrix(A) ^ power;
                        key = GenerateKey(Matrices);
                        Matrices.Add(key, C);
                        stack.Push(key);
                        break;
                    default:
                        string temp = "";
                        bool negative = false;

                        if (element[0] == '_' && element.Contains("\\\\") == false)
                        {
                            negative = true;
                            temp = element.Substring(1);
                        }
                        else
                            temp = element;

                        if (Regex.IsMatch(element, "[a-z]+$"))
                            temp = Variables[temp].ToString();


                        if (negative)
                        {
                            temp = ((-1) * double.Parse(temp)).ToString();
                        }
                        stack.Push(temp);

                        break;
                }

            }


            string outputstr = stack.Pop();
            Matrix output;
            if (Regex.IsMatch(outputstr, "[A-Z]+$"))
            {
                output = Matrices[outputstr];
                Variables.Clear();
                return output;
            }
            else
            {
                DecimalVal = double.Parse(outputstr);
                Variables.Clear();
                return null;
            }
        }

        public static string formatInput(string input)
        {
            string function;
            for (int i = 0; i < input.Length; i++)
            {
                try
                {
                    char character = input[i];
                    switch (character)
                    {
                        case 'x':
                            int maxlen = 5;
                            if (i < 5)
                                maxlen = i;
                            for (int coefficentlen = maxlen; coefficentlen > 0; coefficentlen--)
                            {
                                string coefficient = input.Substring(i - coefficentlen, coefficentlen);
                                if (Regex.IsMatch(coefficient, "[0-9]+$"))
                                {
                                    input = input.Substring(0, i - coefficentlen) + coefficient + " * " + input.Substring(i);
                                    coefficentlen = -1;
                                    i += 3;
                                }
                            }
                            break;
                        case '\\':
                        case 'a':
                        case 'd':
                        case 's':
                        case 'c':
                        case 't':
                        case 'e':
                        case 'l':
                            for (int k = 7; k > 1; k--)
                            {
                                try
                                {
                                    function = input.Substring(i, k);
                                    if (FunctionCodes.Contains(function))
                                    {
                                        input = input.Substring(0, i) + function + " " + input.Substring(i + k);
                                        i += k;
                                        break;
                                    }
                                }
                                catch (Exception e)
                                {

                                }
                            }
                            break;
                        case '+':
                        case '-':
                        case '=':
                        case '*':
                        case '/':
                        case '^':
                        case ',':
                            char temp = input[i];
                            input = input.Substring(0, i) + " " + temp.ToString() + " " + input.Substring(i + 1);
                            i++;
                            break;
                        case '(':
                        case '{':
                            input = input.Substring(0, i) + input[i] + " " + input.Substring(i + 1);
                            i++;
                            break;
                        case ')':
                        case '}':
                            input = input.Substring(0, i) + " " + input[i] + input.Substring(i + 1);
                            i++;
                            break;
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e.Message);
                }
            }

            if (input[0] == ' ')
                input = input.Substring(1);
            if (input[input.Length - 1] == ' ')
                input = input.Substring(0, input.Length - 1);

            return input;
        }

        public static List<string> infixToPostfix(string[] infix)
        {
            bool isOperator(string operator_)
            {
                return operator_ == "^" || operator_ == "*" || operator_ == "/" || operator_ == "+" || operator_ == "-" || operator_ == ",";
            }
            bool isFunction(string function_)
            {
                return FunctionCodes.Contains(function_);
            }
            int precedence(string operator_)
            {
                switch (operator_)
                {
                    case "+":
                    case "-":
                        return 1;
                    case "*":
                    case "/":
                        return 2;
                    case "^":
                    case ",":
                        return 3;
                    default:
                        if (isFunction(operator_))
                            return 3;
                        else
                            return 0;
                }
            }
            List<string> postfix = new List<string>();
            Stack<string> Stack = new Stack<string>();
            foreach (string element in infix)
            {
                //
                if (Regex.IsMatch(element, "[a-zA-Z0-9\\e]") && !isFunction(element))
                    postfix.Add(element);
                else if (element == "(" || element == "{")
                {
                    Stack.Push(element);
                }
                else if (element == ")" || element == "}")
                {

                    while (Stack.Count > 0 && (Stack.Peek() != "(" && Stack.Peek() != "{"))
                        postfix.Add(Stack.Pop());
                    Stack.Pop();
                }
                else
                {
                    while (Stack.Count() > 0 && precedence(element) <= precedence(Stack.Peek()))
                    {
                        postfix.Add(Stack.Pop());
                    }
                    Stack.Push(element);
                }
            }
            while (Stack.Count() > 0)
            {
                string element = Stack.Pop();
                if (element != ",")
                    postfix.Add(element);
            }

            //cross(\M{3\\5\\1\\},\M{32\\4\\4\\})
            while (postfix.Remove(",")) ;

            return postfix;
        }

        public static List<int> ListParse(string input)
        {
            input = input.Substring(1, input.Length - 2);
            string[] itemStrs = input.Split(',');

            List<int> terms = new List<int>();
            for (int i = 0; i < itemStrs.Length; i++)
            {
                int x = Convert.ToInt32(itemStrs[i]);
                terms.Add(x);
            }

            return terms;
        }

        static void cPrintMatrix(Matrix M)
        {
            for (int i = 0; i < M.columns; i++)
            {
                for (int j = 0; j < M.rows; j++)
                {
                    Console.Write((string)M.Values[i, j] + " ");
                }
                Console.WriteLine();
            }

        }
    }
}
