using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Interface
{
    static class InputHandling
    {
        static Random random = new Random();
        static List<string> FunctionCodes = new List<string>()
        {
            "QRr", "QRq", "I",
            "\\sqrt","abs",
            "mod", "arg",
            "sec","csc","cot",
            "cos", "sin", "tan",
            "cosh", "sinh", "tanh",
            "exp", "log", "ln",
            "arccos", "arcsin", "arctan",
            "arccosh", "arcsinh", "arctanh",
            "\\M" , "\\V", "det" ,"dot","cross"
        };

        public static ComplexNumber EvaluatePostfix(List<string> expression, Dictionary<string, Surd> surdVariables, Dictionary<string, ComplexNumber> variables, ref double decimalVal, ref Surd surdVal)
        {
            Stack<string> stack = new Stack<string>();
            foreach (string element in expression)
            {
                string operand1_Str, operand2_Str;
                Rational operand1_Rational, operand2_Rational;
                ComplexNumber operand1_Complex, operand2_Complex;
                Surd operand1_Surd, operand2_Surd;
                double operand_Double;
                string key;

                switch (element)
                {
                    case "arg":
                        operand1_Complex = variables[stack.Pop()];
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(ComplexNumber.Arg(operand1_Complex)));

                        stack.Push(key);
                        break;
                    case "abs":
                    case "mod":
                        operand1_Complex = variables[stack.Pop()];
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(ComplexNumber.Abs(operand1_Complex)));

                        stack.Push(key);
                        break;
                    case "\\sqrt":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Math.Sqrt(operand_Double)));
                        if (operand1_Rational.isFloat == false)
                            surdVariables.Add("#" + key, Surd.Sqrt(operand1_Rational));
                        stack.Push(key);
                        break;
                    case "exp":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Math.Exp(operand_Double)));

                        stack.Push(key);
                        break;
                    case "sin":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Math.Sin(Global.InputAngleConversion(operand_Double))));

                        stack.Push(key);
                        break;
                    case "cos":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Math.Cos(Global.InputAngleConversion(operand_Double))));

                        stack.Push(key);
                        break;
                    case "tan":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Math.Tan(Global.InputAngleConversion(operand_Double))));

                        stack.Push(key);
                        break;
                    case "sinh":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Math.Sinh(Global.InputAngleConversion(operand_Double))));

                        stack.Push(key);
                        break;
                    case "cosh":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Math.Cosh(Global.InputAngleConversion(operand_Double))));

                        stack.Push(key);
                        break;
                    case "tanh":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Math.Tanh(Global.InputAngleConversion(operand_Double))));

                        stack.Push(key);
                        break;
                    case "arcsin":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Global.OutputAngleConversion(Math.Asin(operand_Double))));

                        stack.Push(key);
                        break;
                    case "arccos":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Global.OutputAngleConversion(Math.Acos(operand_Double))));
                        stack.Push(key);
                        break;
                    case "arctan":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Global.OutputAngleConversion(Math.Atan(operand_Double))));
                        stack.Push(key);
                        break;
                    case "arcsinh":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Global.OutputAngleConversion(Math.Asinh(operand_Double))));

                        stack.Push(key);
                        break;
                    case "arccosh":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Global.OutputAngleConversion(Math.Acosh(operand_Double))));
                        stack.Push(key);
                        break;
                    case "arctanh":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Global.OutputAngleConversion(Math.Atanh(operand_Double))));
                        stack.Push(key);
                        break;
                    case "log":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Math.Log10(operand_Double)));

                        stack.Push(key);
                        break;
                    case "ln":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand_Double = operand1_Rational.Decimal();
                        key = GenerateKey(variables);
                        variables.Add(key, new Rational(Math.Log(operand_Double)));

                        stack.Push(key);
                        break;
                    case "+":
                        operand1_Str = stack.Pop();
                        operand2_Str = stack.Pop();

                        operand1_Complex = variables[operand1_Str];
                        operand2_Complex = variables[operand2_Str];

                        key = GenerateKey(variables);
                        variables.Add(key, operand2_Complex + operand1_Complex);

                        if (!surdVariables.TryGetValue("#" + operand1_Str, out operand1_Surd))
                            operand1_Surd = operand1_Complex.real;
                        if (!surdVariables.TryGetValue("#" + operand2_Str, out operand2_Surd))
                            operand2_Surd = operand2_Complex.real;

                        if (operand1_Surd.isIrrational || operand2_Surd.isIrrational)
                            surdVariables.Add("#" + key, operand2_Surd + operand1_Surd);

                        stack.Push(key);
                        break;
                    case "-":
                        operand1_Str = stack.Pop();
                        operand2_Str = "";
                        operand1_Complex = variables[operand1_Str];


                        if (stack.Count() == 0)
                            operand2_Complex = 0;
                        else
                        {
                            operand2_Str = stack.Pop();
                            operand2_Complex = variables[operand2_Str];
                        }
                        key = GenerateKey(variables);
                        variables.Add(key, operand2_Complex - operand1_Complex);

                        if (!surdVariables.TryGetValue("#" + operand1_Str, out operand1_Surd))
                            operand1_Surd = operand1_Complex.real;
                        if (!surdVariables.TryGetValue("#" + operand2_Str, out operand2_Surd))
                            operand2_Surd = operand2_Complex.real;

                        if (operand1_Surd.isIrrational || operand2_Surd.isIrrational)
                            surdVariables.Add("#" + key, operand2_Surd - operand1_Surd);
                        stack.Push(key);
                        break;
                    case "*":
                        operand1_Str = stack.Pop();
                        operand2_Str = stack.Pop();

                        operand1_Complex = variables[operand1_Str];
                        operand2_Complex = variables[operand2_Str];

                        key = GenerateKey(variables);
                        variables.Add(key, operand2_Complex * operand1_Complex);

                        if (!surdVariables.TryGetValue("#" + operand1_Str, out operand1_Surd))
                            operand1_Surd = operand1_Complex.real;
                        if (!surdVariables.TryGetValue("#" + operand2_Str, out operand2_Surd))
                            operand2_Surd = operand2_Complex.real;

                        if (operand1_Surd.isIrrational || operand2_Surd.isIrrational)
                            surdVariables.Add("#" + key, operand2_Surd * operand1_Surd);


                        stack.Push(key);
                        break;
                    case "/":
                        operand1_Complex = variables[stack.Pop()];
                        operand2_Complex = variables[stack.Pop()];

                        key = GenerateKey(variables);
                        variables.Add(key, operand2_Complex / operand1_Complex);

                        stack.Push(key);
                        break;
                    case "^":
                        operand1_Rational = variables[stack.Pop()].real;
                        operand2_Rational = variables[stack.Pop()].real;

                        key = GenerateKey(variables);
                        variables.Add(key, operand2_Rational ^ operand1_Rational);

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
                            if (Regex.IsMatch(element, "[0-9]*i") && element != "\\pi")
                            {
                                if (element == "i")
                                    temp *= new ComplexNumber(0, 1);
                                else
                                    temp *= new ComplexNumber(0, double.Parse(element.Substring(0, element.Length - 1)));
                            }
                            else if (Regex.IsMatch(element, "[A-Za-z\\e]+$") && negative)
                                temp *= variables[element.Substring(1)].real;
                            else if (Regex.IsMatch(element, "[A-Za-z\\e]+$"))
                                temp *= variables[element].real;
                            else if (negative)
                                temp *= double.Parse(element.Substring(1));
                            else
                                temp *= double.Parse(element);

                            key = GenerateKey(variables);
                            variables.Add(key, temp);
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
                output = variables[outputstr];
                variables.Clear();
                surdVariables.TryGetValue("#" + outputstr, out surdVal);
                surdVariables.Clear();
                return output;
            }
            else
            {
                decimalVal = double.Parse(outputstr);
                variables.Clear();
                return null;
            }
        }
        public static double EvaluatePostfix(List<string> expression, Dictionary<string, double> variables,int functionId,ref List<double>[] asymptotes , ref bool validInput)
        {
            validInput = true;
            Stack<string> stack = new Stack<string>();
            for (int iterator = 0; iterator < expression.Count() && validInput; iterator++)
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
                        case "sec":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push((1/Math.Cos(Global.InputAngleConversion(operand1))).ToString());
                            break;
                        case "csc":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push((1/Math.Sin(Global.InputAngleConversion(operand1))).ToString());
                            break;
                        case "cot":
                            operand1 = double.Parse(stack.Pop());
                            stack.Push((1/Math.Tan(Global.InputAngleConversion(operand1))).ToString());
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
                                stack.Push("NaN");
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
                                    temp *= variables[element.Substring(1)];
                                else if (Regex.IsMatch(element, "[a-z]+$"))
                                    temp *= variables[element];
                                else if (negative)
                                    temp *= double.Parse(element.Substring(1));
                                else
                                    temp *= double.Parse(element);

                                stack.Push(temp.ToString());
                            }
                            catch
                            {
                                validInput = false;
                            }
                            break;
                    }
                }
                catch (Exception error)
                {
                    validInput = false;
                }
            }

            double output = double.Parse(stack.Pop());
            if (functionId < 3 &&output == double.NaN)
            {
                asymptotes[functionId].Add(variables["x"]);
            }
            return output;
        }
        public static Matrix EvaluatePostfix_Matrices(List<string> expression, Dictionary<string, double> variables, Dictionary<string, Matrix> matrices, Dictionary<string, Vector> vectors, ref double decimalVal)
        {
            Stack<string> stack = new Stack<string>();
            foreach (string element in expression)
            {
                string operand1, operand2;
                Matrix A, B, C;
                SquareMatrix squareMatrixA;
                Vector vector1, vector2, vector3;
                double operand1_double, operand2_double;
                string key;
                switch (element)
                {
                    case "I":
                        operand1 = stack.Pop();
                        A = SquareMatrix.Identity(int.Parse(operand1));
                        key = GenerateKey(matrices);
                        matrices.Add(key, A);
                        stack.Push(key);
                        break;
                    case @"\M":
                        operand1 = stack.Pop();
                        Matrix NewMatrix = Matrix.LatexToMatrix(operand1);
                        key = GenerateKey(matrices);
                        matrices.Add(key, NewMatrix);
                        stack.Push(key);
                        break;
                    case "cross":
                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        A = matrices[operand1];
                        B = matrices[operand2];
                        vector1 = A.GetColumn(0);
                        vector2 = B.GetColumn(0);
                        vector3 = Vector.cross(vector2, vector1);
                        C = new Matrix(vector3.GetValues());
                        key = GenerateKey(matrices);
                        matrices.Add(key, C);
                        stack.Push(key);
                        break;
                    case "dot":
                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        A = matrices[operand1];
                        B = matrices[operand2];
                        vector1 = A.GetColumn(0);
                        vector2 = B.GetColumn(0);
                        operand1_double = vector2 * vector1;
                        stack.Push(operand1_double.ToString());
                        break;
                    case "det":
                        operand1 = stack.Pop();
                        A = matrices[operand1];
                        squareMatrixA = new SquareMatrix(A);
                        stack.Push(((double)SquareMatrix.Determinant(squareMatrixA)).ToString());
                        break;
                    case "QRr":
                        operand1 = stack.Pop();
                        A = matrices[operand1];
                        squareMatrixA = new SquareMatrix(A);
                        key = GenerateKey(matrices);

                        squareMatrixA = SquareMatrix.QR_R(squareMatrixA);
                        matrices.Add(key, squareMatrixA);
                        stack.Push(key);
                        break;
                    case "QRq":
                        operand1 = stack.Pop();
                        A = matrices[operand1];
                        squareMatrixA = new SquareMatrix(A);
                        key = GenerateKey(matrices);

                        squareMatrixA = SquareMatrix.QR_Q(squareMatrixA);
                        matrices.Add(key, squareMatrixA);
                        stack.Push(key);
                        break;
                    case "+":

                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        if (Regex.IsMatch(operand1, "[0-9]+$") && Regex.IsMatch(operand2, "[0-9]+$"))
                        {
                            stack.Push((double.Parse(operand2) + double.Parse(operand1)).ToString());
                        }
                        else
                        {
                            A = matrices[operand1];
                            B = matrices[operand2];
                            C = B + A;
                            key = GenerateKey(matrices);
                            matrices.Add(key, C);
                            stack.Push(key);
                        }
                        break;
                    case "-":
                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        if (Regex.IsMatch(operand1, "[0-9]+$") && Regex.IsMatch(operand2, "[0-9]+$"))
                        {
                            stack.Push((double.Parse(operand2) - double.Parse(operand1)).ToString());
                        }
                        else
                        {
                            A = matrices[operand1];
                            B = matrices[operand2];
                            C = B - A;
                            key = GenerateKey(matrices);
                            matrices.Add(key, C);
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
                                A = matrices[operand1];
                                B = matrices[operand2];
                                C = B * A;
                                matrices.Add(operand2 + operand1, C);
                                stack.Push(operand2 + operand1);
                            }
                            catch (NullReferenceException e)
                            {
                                string msg = e.Message;
                            }
                        }
                        else if (Regex.IsMatch(operand1, "[A-Z]"))
                        {
                            A = matrices[operand1];
                            C = double.Parse(operand2) * A;
                            key = GenerateKey(matrices);
                            matrices.Add(key, C);
                            stack.Push(key);
                        }
                        else if (Regex.IsMatch(operand2, "[A-Z]"))
                        {
                            B = matrices[operand2];
                            C = double.Parse(operand1) * B;
                            key = GenerateKey(matrices);
                            matrices.Add(key, C);
                            stack.Push(key);
                        }
                        else
                        {
                            operand1_double = double.Parse(operand1);
                            operand2_double = double.Parse(operand2);
                            stack.Push((operand2_double * operand1_double).ToString());
                            break;
                        }
                        break;
                    case "/":
                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        operand1_double = double.Parse(operand1);
                        operand2_double = double.Parse(operand2);
                        stack.Push((operand2_double / operand1_double).ToString());
                        break;
                    case "^":
                        operand1 = stack.Pop();
                        operand2 = stack.Pop();
                        A = matrices[operand2];
                        int power = int.Parse(operand1);
                        C = new SquareMatrix(A) ^ power;
                        key = GenerateKey(matrices);
                        matrices.Add(key, C);
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
                            temp = variables[temp].ToString();


                        if (negative)
                        {
                            temp = ((-1) * double.Parse(temp)).ToString();
                        }
                        stack.Push(temp);

                        break;
                }
            }


            string outputString = stack.Pop();
            Matrix output;
            if (Regex.IsMatch(outputString, "[A-Z]+$"))
            {
                output = matrices[outputString];
                variables.Clear();
                return output;
            }
            else
            {
                decimalVal = double.Parse(outputString);
                variables.Clear();
                return null;
            }
        }


        public static Tuple<ComplexNumber, Rational> ParseComplexLoci_HalfLine(List<string> LHS, List<string> RHS)
        {

            Dictionary<string, Surd> surdDictionary = new Dictionary<string, Surd>();
            Dictionary<string, ComplexNumber> complexDictionary = new Dictionary<string, ComplexNumber>();

            LHS.Remove("arg");
            double decimalValue = 0;
            Surd placeholderSurd = 0;
            Function LHS_expression = new Function(LHS);
            Function RHS_expression = new Function(RHS);
            ComplexNumber Angle = RHS_expression.Evaluate(surdDictionary, complexDictionary, ref decimalValue, ref placeholderSurd);
            ComplexNumber Center = LHS_expression.Evaluate(surdDictionary, complexDictionary, ref decimalValue, ref placeholderSurd, 0);
            return Tuple.Create(Center, Angle.real);
        }
        public static Tuple<ComplexNumber, Rational> ParseComplexLoci_Circle(List<string> LHS, List<string> RHS)
        {
            Dictionary<string, Surd> surdDictionary = new Dictionary<string, Surd>();
            Dictionary<string, ComplexNumber> complexDictionary = new Dictionary<string, ComplexNumber>();

            LHS.Remove("mod");
            double decimalValue = 0;
            Surd placeholderSurd = 0;
            Function LHS_expression = new Function(LHS);
            Function RHS_expression = new Function(RHS);
            ComplexNumber radius = RHS_expression.Evaluate(surdDictionary, complexDictionary, ref decimalValue, ref placeholderSurd);
            ComplexNumber center = LHS_expression.Evaluate(surdDictionary, complexDictionary, ref decimalValue, ref placeholderSurd, 0);
            return Tuple.Create(center, radius.real);
        }
        public static Tuple<ComplexNumber, ComplexNumber> ComplexLociParse_Bisector(List<string> LHS, List<string> RHS)
        {
            Dictionary<string, Surd> surdDictionary = new Dictionary<string, Surd>();
            Dictionary<string, ComplexNumber> complexDictionary = new Dictionary<string, ComplexNumber>();

            LHS.Remove("mod");
            RHS.Remove("mod");
            double decimalValue = 0;
            Surd placeholderSurd = 0;
            Function LHS_expression = new Function(LHS);
            Function RHS_expression = new Function(RHS);
            ComplexNumber Z_2 = RHS_expression.Evaluate(surdDictionary, complexDictionary, ref decimalValue, ref placeholderSurd);
            ComplexNumber Z_1 = LHS_expression.Evaluate(surdDictionary, complexDictionary, ref decimalValue, ref placeholderSurd, 0);
            return Tuple.Create(Z_1, Z_2);
        }
        static public string IdentifyLoci(string input)
        {
            string[] temp = input.Split('=');
            string LHS = temp[0];
            string RHS = temp[1];

            if (LHS.Contains("mod") && RHS.Contains("mod"))
                return "B";
            else if (LHS.Contains("mod") && Regex.IsMatch(RHS, "[0-9.]*"))
                return "C";
            else if (LHS.Contains("arg") & Regex.IsMatch(RHS, "[0-9.]*"))
                return "H";
            else
                return "U";
        }
        public static string GenerateKey<T>(Dictionary<string, T> Dictionary)
        {
            int keyLength = 4;
            string key = "";
            for (int i = 0; i < keyLength; i++)
            {
                key += (char)random.Next(65, 91);
            }
            if (Dictionary.ContainsKey(key))
                key = GenerateKey(Dictionary);

            return key;
        }
        public static string FormatInput(string input)
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
                            int maxLen = 5;
                            if (i < 5)
                                maxLen = i;
                            for (int coefficientLen = maxLen; coefficientLen > 0; coefficientLen--)
                            {
                                string coefficient = input.Substring(i - coefficientLen, coefficientLen);
                                if (Regex.IsMatch(coefficient, "[0-9]+$"))
                                {
                                    input = input.Substring(0, i - coefficientLen) + coefficient + " * " + input.Substring(i);
                                    coefficientLen = -1;
                                    i += 3;
                                }
                            }
                            break;
                        case 'I':
                            input = input.Substring(0, i) + "I " + input.Substring(i + 1);
                            i++;
                            break;
                        case '\\':
                        case 'a':
                        case 'd':
                        case 'm':
                        case 's':
                        case 'Q':
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

        public static List<string> InfixToPostfix(string[] infix)
        {
            var temp = infix.ToList();
            while (temp.Remove("")) ;

            infix = temp.ToArray();
            
            bool IsFunction(string function_)
            {
                return FunctionCodes.Contains(function_);
            }
            int Precedence(string operator_)
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
                        if (IsFunction(operator_))
                            return 3;
                        else
                            return 0;
                }
            }

            List<string> postfix = new List<string>();
            Stack<string> stack = new Stack<string>();
            foreach (string element in infix)
            {
                //
                if (Regex.IsMatch(element, "[a-zA-Z0-9\\e]") && !IsFunction(element))
                    postfix.Add(element);
                else if (element == "(" || element == "{")
                {
                    stack.Push(element);
                }
                else if (element == ")" || element == "}")
                {

                    while (stack.Count > 0 && (stack.Peek() != "(" && stack.Peek() != "{"))
                        postfix.Add(stack.Pop());
                    stack.Pop();
                }
                else
                {
                    while (stack.Count() > 0 && Precedence(element) <= Precedence(stack.Peek()))
                    {
                        postfix.Add(stack.Pop());
                    }
                    stack.Push(element);
                }
            }
            while (stack.Count() > 0)
            {
                string element = stack.Pop();
                if (element != ",")
                    postfix.Add(element);
            }

            while (postfix.Remove(",")) ;

            return postfix;
        }
        public static List<int> ParseList(string input)
        {
            input = input.Substring(1, input.Length - 2);
            string[] itemStrings = input.Split(',');

            List<int> terms = new List<int>();
            for (int i = 0; i < itemStrings.Length; i++)
            {
                int x = Convert.ToInt32(itemStrings[i]);
                terms.Add(x);
            }

            return terms;
        }
     
    }
}
