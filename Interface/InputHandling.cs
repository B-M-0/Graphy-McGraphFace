using ScottPlot;
using ScottPlot.Drawing.Colormaps;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NEA26
{
    static class InputHandling
    {
        static Random random = new Random();
        static List<string> FunctionCodes = new List<string>() { "cos", "sin", "tan", "exp", "log", "ln", "acos", "asin", "atan" };
        public static double evaluateInfix(List<string> expression, Dictionary<string, double> Variables)
        {
            Stack<string> stack = new Stack<string>();
            foreach (string element in expression)
            {

                double operand1, operand2;
                switch (element)
                {
                    case "exp":
                        operand1 = double.Parse(stack.Pop());
                        stack.Push((Math.Exp(operand1)).ToString());
                        break;
                    case "sin":
                        operand1 = double.Parse(stack.Pop());
                        stack.Push((Math.Sin(operand1)).ToString());
                        break;
                    case "cos":
                        operand1 = double.Parse(stack.Pop());
                        stack.Push((Math.Cos(operand1)).ToString());
                        break;
                    case "tan":
                        operand1 = double.Parse(stack.Pop());
                        stack.Push((Math.Tan(operand1)).ToString());
                        break;
                    case "asin":
                        operand1 = double.Parse(stack.Pop());
                        stack.Push((Math.Asin(operand1)).ToString());
                        break;
                    case "acos":
                        operand1 = double.Parse(stack.Pop());
                        stack.Push((Math.Acos(operand1)).ToString());
                        break;
                    case "atan":
                        operand1 = double.Parse(stack.Pop());
                        stack.Push((Math.Atan(operand1)).ToString());
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
                        catch { }
                        break;
                }

            }

            double output = double.Parse(stack.Pop());
            return output;
        }


        public static string GenerateKey(Dictionary<string, Matrix> Dictionary)
        {
            string key = "";
            for (int i = 0; i < 4; i++)
            {
                key += (char)random.Next(65, 91);
            }
            if (Dictionary.ContainsKey(key)) 
                key = GenerateKey(Dictionary);
            
            return key;
        }

        public static Matrix evaluateInfixMatrix(List<string> expression, Dictionary<string, double> Variables, Dictionary<string, Matrix> Matrices)
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
                            A = Matrices[operand1];
                            B = Matrices[operand2];
                            C = B * A;
                            Matrices.Add(operand2 + operand1, C);
                            stack.Push(operand2 + operand1);
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
                            stack.Push((b*a).ToString());
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
                        int power= int.Parse(operand1);
                        C = (SquareMatrix)(A) ^ power;
                        break;
                    default:
                        string temp;
                        try
                        {
                            if (Regex.IsMatch(element, "[a-z]+$"))
                                temp = Variables[element].ToString();
                            else
                                temp = element;

                            stack.Push(temp);
                        }
                        catch { }
                        break;
                }

            }

            double output = double.Parse(stack.Pop());
            return null;
        }



        public static string formatInput(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case 'a':
                        string function = input.Substring(i, 4);
                        if (FunctionCodes.Contains(function))
                        {
                            input = input.Substring(0, i) + function + " " + input.Substring(i + 4);
                            i += 4;
                        }
                        break;
                    case 's':
                    case 'c':
                    case 't':
                    case 'e':
                    case 'l':
                        function = input.Substring(i, 3);
                        if (FunctionCodes.Contains(function))
                        {
                            input = input.Substring(0, i) + function + " " + input.Substring(i + 3);
                            i += 3;
                        }
                        else if (FunctionCodes.Contains(function.Substring(i)))
                        {
                            input = input.Substring(0, i) + function + " " + input.Substring(i + 2);
                            i += 2;
                        }
                        break;
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case '^':
                        char temp = input[i];
                        input = input.Substring(0, i) + " " + temp.ToString() + " " + input.Substring(i + 1);
                        i++;
                        break;
                    case '(':
                        input = input.Substring(0, i) + "( " + input.Substring(i + 1);
                        i++;
                        break;
                    case ')':
                        input = input.Substring(0, i) + " )" + input.Substring(i + 1);
                        i++;
                        break;
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
                return operator_ == "^" || operator_ == "*" || operator_ == "/" || operator_ == "+" || operator_ == "-";
            }
            bool isfunction(string function_)
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
                        return 3;
                    default:
                        if (isfunction(operator_))
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
                if (Regex.IsMatch(element, "[a-zA-Z0-9]+$") && !isfunction(element))
                    postfix.Add(element);
                else if (element == "(")
                {
                    Stack.Push(element);
                }
                else if (element == ")")
                {

                    while (Stack.Count > 0 && Stack.Peek() != "(")
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
                postfix.Add(Stack.Pop());
            }


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
