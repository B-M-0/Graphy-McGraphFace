using ScottPlot.Styles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfMath;
using WpfMath.Parsers;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    class AngleTypes : ObservableCollection<string>
    {
        public AngleTypes()
        {
            Add("Radians");
            Add("Degrees");
            Add("Gradians");
        }
    }

    public partial class MainWindow : Window
    {

        static int input_count = 3;

        Function[] functions = new Function[input_count];

        Function function0;
        Function function1;
        Function function2;

        Function[] transformations = new Function[input_count];

        Function transformation0;
        Function transformation1;
        Function transformation2;



        bool[] valid_function = new bool[input_count];
        bool[] valid_transformation = new bool[input_count];
        bool[] valid_complex = new bool[input_count];

        int currentPage = 1;
        int universial_range = 20;
        int universial_lower_bound = -10;
        double universial_step = 0.1;

        string result;
        string alt_result;
        bool showAltForm = false;

        string[] function_inputs = new string[input_count];
        string[] transformation_inputs = new string[input_count];
        string[] complex_inputs = new string[input_count];
        string[] parsed_complex_inputs = new string[input_count];
        static public Dictionary<string, double> Vars = new Dictionary<string, double>() { { "e", Math.E }, { "\\pi", Math.PI } };
        public Dictionary<string, Matrix> Matrices = new Dictionary<string, Matrix>();
        public Dictionary<string, Vector> Vectors = new Dictionary<string, Vector>();



        /// <summary>
        /// reffactor later
        /// </summary>
        public Dictionary<string, ComplexNumber> RationalVars = new Dictionary<string, ComplexNumber>() { { "e", Math.E }, { "\\pi",Math.PI} };
        public Dictionary<string, Matrix> RAtionalMatrices = new Dictionary<string, Matrix>();
        public Dictionary<string, Surd> SurdVars = new Dictionary<string, Surd>();


        public bool bracketsBalanced(string input)
        {
            int openCount = 0;
            int closedCount = 0;
            foreach (char element in input)
            {
                switch (element)
                {
                    case '(':
                        openCount++;
                        break;
                    case ')':
                        closedCount++;
                        break;
                }
            }
            return openCount == closedCount;
        }
        public string formatLatex(string input, string Mode = "f")
        {
            //Modes f ==> functions, c ==> calcuations
            string temp = "";
            for (int i = 0; i <input.Length; i++)
            {
                char character = input[i];
                switch (character)
                {
                    case 'i':
                        if (i != 0 && input[i - 1] != 'p')
                            temp += " i ";
                        else temp += "i ";
                        break;
                    case ' ':
                    case '*':
                        if (Mode == "c")
                            temp += " \\cdot ";
                        break;
                    case '_':
                        temp += " -";
                        break;
                    default:
                        temp += character;
                        break;
                }
            }
            if (temp.Contains("\\M"))
                temp = temp.Replace("\\M", "\\pmatrix");
            return temp;
        }
        public static Bitmap convertLatexToPng(string latex)
        {
            var parser = WpfTeXFormulaParser.Instance;
            var formula = parser.Parse(latex);
            var png_Data = formula.RenderToPng(190.0, 4.0, 2.0, "Arial");

            Bitmap Image;
            using (MemoryStream ms = new MemoryStream(png_Data))
            {
                Image = new Bitmap(ms);
            }
            return Image;
        }
        ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            BitmapSource bitmapsource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            //^ this line is stolen         
            return bitmapsource;
        }


        public MainWindow()
        {
            //scuffed test environment
            //\start
            List<List<int>> mtxlist = new List<List<int>> { new List<int> { 12, -51, 4 }, new List<int> { 6, 167, -68 }, new List<int> { -4, 24, -41 } };
            SquareMatrix a = new SquareMatrix(Matrix.Convert2DListToArray(mtxlist));
            var b = SquareMatrix.QR_Decomposition(a);

            // \end
            InitializeComponent();
            Title.Source = BitmapToImageSource(convertLatexToPng("Graphy McGraphFace"));

            OpenMainMenu();
            function_inputs = new string[] { Function_Input0.Text, Function_Input1.Text, Function_Input2.Text };
            for (int i = 0; i < valid_function.Length; i++)
            {
                valid_function[i] = false;
                valid_transformation[i] = false;
            }

            Matrix UnitSquare = new Matrix(new double[2, 4] { { 0, 1, 1, 0 }, { 0, 0, 1, 1 } });
            Matrix UnitTriangle = new Matrix(new double[2, 3] { { 0, 1, 0.5 }, { 0, 0, Math.Sqrt(3) / 2 } });
            Matrices.Add("U", UnitSquare);
            Matrices.Add("T", UnitTriangle);
            UpdateGraph_functions();
        }


        public void UpdateRender(int id, ImageSource Render)
        {
            switch (id)
            {
                case 0:
                    Input0Render.Source = Render;
                    break;
                case 1:
                    Input1Render.Source = Render;
                    break;
                case 2:
                    Input2Render.Source = Render;
                    break;
            }
        }

        public void UpdateText()
        {
            function_inputs = new string[] { Function_Input0.Text, Function_Input1.Text, Function_Input2.Text };
            transformation_inputs = new string[] { Transformation_Input0.Text, Transformation_Input1.Text, Transformation_Input2.Text };
            complex_inputs = new string[] { Complex_Input0.Text, Complex_Input1.Text, Complex_Input2.Text };
        }

        public void UpdateGraph_functions()
        {
            Graph.Plot.Clear();
            UpdateText();
            ResetGraph_Clicked();

            Graph.Plot.Style(ScottPlot.Style.GetStyles()[Global.ThemeIndex]);

            Graph.Plot.AddHorizontalLine(y: 0, color: System.Drawing.Color.Black);
            Graph.Plot.AddVerticalLine(x: 0, color: System.Drawing.Color.Black);

            for (int i = 0; i < input_count; i++)
            {
                if (valid_function[i] && function_inputs[i] != "")
                {
                    bool validinput = true;
                    Function function = functions[i];
                    var func = new Func<double, double?>((x) => function.Evaluate(Vars, ref validinput, x));
                    valid_function[i] = validinput;
                    if (validinput)
                        Graph.Plot.AddFunction(func, lineWidth: 2);
                }

            }

            Graph.Plot.SetAxisLimits(-10, 10, -5, 5);
            Graph.Refresh();
        }

        public void PlotMatrix(Matrix M)
        {
            if (M.rows == 2)
            {
                double[] xs = new double[M.columns];
                double[] ys = new double[M.columns];
                for (int position = 0; position < M.columns; position++)
                {
                    xs[position] = M.Values[0, position];
                    ys[position] = M.Values[1, position];

                    if (M.columns > 2)
                        Graph.Plot.AddPolygon(xs, ys, Graph.Plot.GetNextColor(.3), lineWidth: 2);

                    Graph.Plot.AddScatter(xs, ys, lineWidth: 0, markerSize: 7, color: System.Drawing.Color.Black);
                }
            }
        }
        public void UpdateGraph_transformations()
        {
            Graph.Plot.Clear();
            ResetGraph_Clicked();
            //Update();

            Graph.Plot.Style(ScottPlot.Style.GetStyles()[Global.ThemeIndex]);

            Graph.Plot.AddHorizontalLine(y: 0, color: System.Drawing.Color.Black);
            Graph.Plot.AddVerticalLine(x: 0, color: System.Drawing.Color.Black);
            bool UPlotted = false;
            for (int i = 0; i < input_count; i++)
            {
                if (valid_transformation[i])
                {
                    Function function = functions[i];
                    double decimalval = 0;
                    Matrix Result = function.Evaluate(Vars, Matrices, Vectors, ref decimalval);
                    for (int ResultRow = 0; ResultRow < Result.rows; ResultRow++)
                    {
                        for (int ResultColumn = 0; ResultColumn < Result.columns; ResultColumn++)
                        {
                            if (Result.Values[ResultRow, ResultColumn] == null)
                            {
                                return;
                            }
                        }
                    }
                    Graph.Refresh();
                    PlotMatrix(Result);

                    if (function.GetFunction().Contains("U") && !UPlotted)
                    {
                        PlotMatrix(Matrices["U"]);
                        UPlotted = true;
                    }



                }

                Graph.Plot.SetAxisLimits(-10, 10, -5, 5);
                Graph.Refresh();
            }
        }
        public void UpdateGraph_ComplexLoci()
        {
            Graph.Plot.Clear();
            ResetGraph_Clicked();
            

            for (int i = 0; i < complex_inputs.Length; i++) 
            {
                if (valid_complex[i] == false)
                    continue;
                string input = parsed_complex_inputs[i];
                string typeOfLoci = InputHandling.IdentifyLoci(input);
                string[] expression = input.Split('=');
                List<string> LHS;
                List<string> RHS;
                LHS = InputHandling.infixToPostfix(expression[0].Split(' '));
                RHS = InputHandling.infixToPostfix(expression[1].Split(' '));
                // Types :: HalfLine (H), Bisector (B), Circle (C), Undefined (U)
                try
                {
                    switch (typeOfLoci)
                    {
                        case "H":
                            Tuple<ComplexNumber, Rational> HalflineStats = InputHandling.ComplexLociParse_Halfline(LHS, RHS);
                            double centerX = -HalflineStats.Item1.real;
                            double centerY = -HalflineStats.Item1.imaginary;
                            double angle = HalflineStats.Item2;
                            //center
                            Graph.Plot.AddCircle(centerX, centerY, 0.05, lineWidth: (float)2);
                            //Loci
                            var func = new Func<double, double?>((x) => Global.HalfLine(x, angle, centerY, centerX));
                            Graph.Plot.AddFunction(func, lineWidth: 2);
                            break;
                        case "C":
                            Tuple<ComplexNumber, Rational> CircleStats = InputHandling.ComplexLociParse_Circle(LHS, RHS);
                            centerX = -CircleStats.Item1.real;
                            centerY = -CircleStats.Item1.imaginary;
                            double radius = CircleStats.Item2;
                            //center
                            Graph.Plot.AddCircle(centerX, centerY, 0.05, lineWidth: (float)2);
                            //Loci
                            Graph.Plot.AddCircle(centerX, centerY, radius);
                            break;
                        case "B":
                            Tuple<ComplexNumber, ComplexNumber> BisectorStats = InputHandling.ComplexLociParse_Bisector(LHS, RHS);
                            double x_1 = -BisectorStats.Item1.real;
                            double y_1 = -BisectorStats.Item1.imaginary;
                            double x_2 = -BisectorStats.Item2.real;
                            double y_2 = -BisectorStats.Item2.imaginary;

                            double gradient;
                            double dy = y_2 - y_1;
                            double dx = x_2 - x_1;
                            if (dy == 0 || dx == 0)
                                gradient = 0;
                            else
                                gradient = -(dx / dy);
                            //MidPoint mp_? == midpoint_?
                            double mp_x = (x_1 + x_2) / 2;
                            double mp_y = (y_1 + y_2) / 2;

                            if (y_2 == y_1)
                            {
                                Graph.Plot.AddVerticalLine(x: mp_x);
                            }
                            else
                            {
                                func = new Func<double, double?>((x) => Global.Line(x, gradient, mp_y, mp_x));
                                Graph.Plot.AddFunction(func, lineWidth: 2);
                            }
                            break;
                    }
                }
                catch 
                { 
                
                }
            }
            Graph.Plot.Style(ScottPlot.Style.GetStyles()[Global.ThemeIndex]);

            Graph.Plot.AddHorizontalLine(y: 0, color: System.Drawing.Color.Black);
            Graph.Plot.AddVerticalLine(x: 0, color: System.Drawing.Color.Black);
            Graph.Refresh();    
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SideBar.Visibility == Visibility.Collapsed && OpenSideBar.Visibility == Visibility.Visible)
            {
                SideBar.Visibility = Visibility.Visible;
                if (currentPage == 1)
                {
                    Function_Input0.Visibility = Visibility.Visible;
                    Function_Input1.Visibility = Visibility.Visible;
                    Function_Input2.Visibility = Visibility.Visible;
                }
                else if (currentPage == 2)
                {
                    Transformation_Input0.Visibility = Visibility.Visible;
                    Transformation_Input1.Visibility = Visibility.Visible;
                    Transformation_Input2.Visibility = Visibility.Visible;
                }
                else if (currentPage == 4)
                {
                    Complex_Input0.Visibility = Visibility.Visible;
                    Complex_Input1.Visibility = Visibility.Visible;
                    Complex_Input2.Visibility = Visibility.Visible;
                }


                Input0Render.Visibility = Visibility.Visible;
                Input1Render.Visibility = Visibility.Visible;
                Input2Render.Visibility = Visibility.Visible;
                Input0RenderBackDrop.Visibility = Visibility.Visible;
                Input1RenderBackDrop.Visibility = Visibility.Visible;
                Input2RenderBackDrop.Visibility = Visibility.Visible;

                ResetGraph.Visibility = Visibility.Visible;
                GoToMainMenu.Visibility = Visibility.Visible;

                Grid.SetColumn(Graph, 4);
                Grid.SetColumnSpan(Graph, 4);

                (sender as Button).Content = "<";
            }
            else
            {
                Grid.SetColumn(Graph, 2);
                Grid.SetColumnSpan(Graph, 6);

                SideBar.Visibility = Visibility.Collapsed;
                if (currentPage == 1)
                {
                    Function_Input0.Visibility = Visibility.Collapsed;
                    Function_Input1.Visibility = Visibility.Collapsed;
                    Function_Input2.Visibility = Visibility.Collapsed;
                }
                else if (currentPage == 2)
                {
                    Transformation_Input0.Visibility = Visibility.Collapsed;
                    Transformation_Input1.Visibility = Visibility.Collapsed;
                    Transformation_Input2.Visibility = Visibility.Collapsed;
                }
                else if (currentPage == 4)
                {
                    Complex_Input0.Visibility = Visibility.Collapsed;
                    Complex_Input1.Visibility = Visibility.Collapsed;
                    Complex_Input2.Visibility = Visibility.Collapsed;

                }


                Input0Render.Visibility = Visibility.Collapsed;
                Input1Render.Visibility = Visibility.Collapsed;
                Input2Render.Visibility = Visibility.Collapsed;
                Input0RenderBackDrop.Visibility = Visibility.Collapsed;
                Input1RenderBackDrop.Visibility = Visibility.Collapsed;
                Input2RenderBackDrop.Visibility = Visibility.Collapsed;



                ResetGraph.Visibility = Visibility.Collapsed;
                GoToMainMenu.Visibility = Visibility.Collapsed;

                (sender as Button).Content = ">";
            }
        }

        private void Calculation0_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string inputText = (sender as TextBox).Text;
                string Latex = formatLatex(inputText, "c");
                Calculation_InputRender0.Source = BitmapToImageSource(convertLatexToPng(Latex));
                Calculation_OutputRender0.Source = null;
                "sd".ToString();
                List<string> output = InputHandling.infixToPostfix(InputHandling.formatInput(Global.removeWhiteSpace((sender as TextBox).Text)).Split(' '));

                Function f = new Function(output);
                result = "=";
                if (inputText.Contains("\\M") || inputText.Contains("I(") || Regex.IsMatch(inputText, "[A-Z]+$"))
                {
                    double decialVal = 0;
                    Matrix Alfa = f.Evaluate(Vars, Matrices, Vectors, ref decialVal);
                    if (Alfa is null)
                        result += decialVal.ToString();
                    else
                        result += Alfa.MatrixToLatex();
                }
                else
                {
                    double decimalVal = 0;
                    Surd Surdval = 0;
                    ComplexNumber resultVAl = f.Evaluate(SurdVariables: SurdVars, Variables: RationalVars, decimalval: ref decimalVal,Surdval: ref Surdval);
                    
                    if (resultVAl.isComplex())
                    {
                        ToggleModuloButton.Visibility = Visibility.Visible;
                        alt_result = resultVAl.ModulousArgumentForm();
                    }
                    else if (Surdval != null && Surdval != 0.0)
                    {
                        ToggleModuloButton.Visibility = Visibility.Visible;
                        alt_result = Surdval.ToLatex();
                    }


                    else
                        ToggleModuloButton.Visibility = Visibility.Collapsed;

                    if (resultVAl == null)
                        result += Math.Round(decimalVal, Global.decimalPlaces).ToString();
                    else if (resultVAl.imaginary.isFloat || resultVAl.real.isFloat)
                        result = "\\approx" + (resultVAl.LatexFormat_float());
                    else if (resultVAl.LatexFormat_frac() != resultVAl.LatexFormat_float())
                        result += resultVAl.LatexFormat_frac() + "=" + resultVAl.LatexFormat_float();
                    else
                        result += resultVAl.LatexFormat_frac();

                }
                Calculation_OutputRender0.Source = BitmapToImageSource(convertLatexToPng(result));
            }
            catch
             { //RenderErrorlhudyugywgygey}   
            }
        }

        public void TextChanged(int id)
        {
            UpdateText();
            string[] inputs;
            bool[] valid;
            if (currentPage == 1)
            {
                inputs = function_inputs;
                valid = valid_function;
            }
            else if (currentPage == 2)
            {
                inputs = transformation_inputs;
                valid = valid_transformation;
            }
            else
            {
                inputs = complex_inputs;
                valid = valid_complex;
            }

            if (bracketsBalanced(inputs[id]))
            {
                try
                {
                    string Latex = formatLatex(inputs[id]);
                    UpdateRender(id, BitmapToImageSource(convertLatexToPng(Latex)));
                }
                catch
                {
                    UpdateRender(id, null);
                }
                Graph.Plot.Clear();

                try
                {
                    string input_string = InputHandling.formatInput(Global.removeWhiteSpace(inputs[id]));
                    List<string> expression = InputHandling.infixToPostfix(input_string.Split(' '));
                    if (currentPage == 4)
                        parsed_complex_inputs[id] = input_string;
                    else
                        functions[id] = new Function(expression);
                    valid[id] = true;

                    if (currentPage == 1)
                        UpdateGraph_functions();
                    else if (currentPage == 2)
                        UpdateGraph_transformations();
                    else if (currentPage == 4)
                        UpdateGraph_ComplexLoci();
                }
                catch (Exception exception)
                {
                    Console.Error.WriteLine("Error when parsing formula: " + exception.Message);
                }

                if (currentPage == 1)
                    valid_function = valid;
                else if (currentPage == 2)
                    valid_transformation = valid;
                else
                    valid_complex = valid;
            }
        }

        private void TextBox0_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged(0);
        }
        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged(1);
        }
        private void TextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged(2);
        }


        public void ShowFunctions()
        {
            Graph.Visibility = Visibility.Visible;
            OpenSideBar.Visibility = Visibility.Visible;
            Grid.SetColumn(Graph, 2);
            Grid.SetColumnSpan(Graph, 6);
        }
        public void HideFunctions()
        {
            Graph.Visibility = Visibility.Collapsed;
            SideBar.Visibility = Visibility.Collapsed;
            Function_Input0.Visibility = Visibility.Collapsed;
            Function_Input1.Visibility = Visibility.Collapsed;
            Function_Input2.Visibility = Visibility.Collapsed;
            Input0Render.Visibility = Visibility.Collapsed;
            Input1Render.Visibility = Visibility.Collapsed;
            Input2Render.Visibility = Visibility.Collapsed;
            Input0Render.Visibility = Visibility.Collapsed;
            Input0RenderBackDrop.Visibility = Visibility.Collapsed;
            Input1RenderBackDrop.Visibility = Visibility.Collapsed;
            Input2RenderBackDrop.Visibility = Visibility.Collapsed;
            ResetGraph.Visibility = Visibility.Collapsed;
            GoToMainMenu.Visibility = Visibility.Collapsed;
            OpenSideBar.Visibility = Visibility.Collapsed;
        }

        public void ShowTransformations()
        {
            Graph.Visibility = Visibility.Visible;
            OpenSideBar.Visibility = Visibility.Visible;
            Grid.SetColumn(Graph, 2);
            Grid.SetColumnSpan(Graph, 6);
        }
        public void HideTransformations()
        {
            Graph.Visibility = Visibility.Collapsed;
            SideBar.Visibility = Visibility.Collapsed;
            Transformation_Input0.Visibility = Visibility.Collapsed;
            Transformation_Input1.Visibility = Visibility.Collapsed;
            Transformation_Input2.Visibility = Visibility.Collapsed;
            Input0Render.Visibility = Visibility.Collapsed;
            Input1Render.Visibility = Visibility.Collapsed;
            Input2Render.Visibility = Visibility.Collapsed;
            Input0Render.Visibility = Visibility.Collapsed;
            Input0RenderBackDrop.Visibility = Visibility.Collapsed;
            Input1RenderBackDrop.Visibility = Visibility.Collapsed;
            Input2RenderBackDrop.Visibility = Visibility.Collapsed;
            ResetGraph.Visibility = Visibility.Collapsed;
            GoToMainMenu.Visibility = Visibility.Collapsed;
            OpenSideBar.Visibility = Visibility.Collapsed;
        }

        public void ShowComplexPlane()
        {
            Graph.Visibility = Visibility.Visible;
            OpenSideBar.Visibility = Visibility.Visible;
            Grid.SetColumn(Graph, 2);
            Grid.SetColumnSpan(Graph, 6);
        }
        public void HideComplexPlane()
        {
            Graph.Visibility = Visibility.Collapsed;
            SideBar.Visibility = Visibility.Collapsed;
            Complex_Input0.Visibility = Visibility.Collapsed;
            Complex_Input1.Visibility = Visibility.Collapsed;
            Complex_Input2.Visibility = Visibility.Collapsed;
            Input0Render.Visibility = Visibility.Collapsed;
            Input1Render.Visibility = Visibility.Collapsed;
            Input2Render.Visibility = Visibility.Collapsed;
            Input0Render.Visibility = Visibility.Collapsed;
            Input0RenderBackDrop.Visibility = Visibility.Collapsed;
            Input1RenderBackDrop.Visibility = Visibility.Collapsed;
            Input2RenderBackDrop.Visibility = Visibility.Collapsed;
            GoToMainMenu.Visibility = Visibility.Collapsed;
            ResetGraph.Visibility = Visibility.Collapsed;
            OpenSideBar.Visibility = Visibility.Collapsed;
        }

        public void ShowCalculations()
        {
            GoToMainMenu.Visibility = Visibility.Visible;

            Calculation_InputRender0.Visibility = Visibility.Visible;
            Calculation_Input0.Visibility = Visibility.Visible;
            Calculation_InputBackDrop0.Visibility = Visibility.Visible;
            Calculation_OutputRender0.Visibility = Visibility.Visible;
            Calculation_OutputBackDrop0.Visibility = Visibility.Visible;
        }
        public void HideCalculations()
        {
            ResetGraph.Visibility = Visibility.Collapsed;
            GoToMainMenu.Visibility = Visibility.Collapsed;
            ToggleModuloButton.Visibility = Visibility.Collapsed;
            Calculation_InputRender0.Visibility = Visibility.Collapsed;
            Calculation_Input0.Visibility = Visibility.Collapsed;
            Calculation_InputBackDrop0.Visibility = Visibility.Collapsed;
            Calculation_OutputRender0.Visibility = Visibility.Collapsed;
            Calculation_OutputBackDrop0.Visibility = Visibility.Collapsed;
        }

        public void ShowMainMenu()
        {
            Grid.SetColumn(Graph, 2);
            Grid.SetColumnSpan(Graph, 4);
            GoToFunctions.Visibility = Visibility.Visible;
            GoToTransformations.Visibility = Visibility.Visible;
            GoToCalculations.Visibility = Visibility.Visible;
            GoToComplexPlane.Visibility = Visibility.Visible;
        }
        public void HideMainMenu()
        {
            Title.Visibility = Visibility.Collapsed;
            TitleBackDrop.Visibility = Visibility.Collapsed;
            GoToFunctions.Visibility = Visibility.Collapsed;
            GoToTransformations.Visibility = Visibility.Collapsed;
            GoToCalculations.Visibility = Visibility.Collapsed;
            GoToComplexPlane.Visibility = Visibility.Collapsed;
        }

        private void ShowSettings()
        {
            GoToSettings.Visibility = Visibility.Collapsed;

            AngleMeasurementLabel.Visibility = Visibility.Visible;
            AngleMeasurement.Visibility = Visibility.Visible;
            Theme.Visibility = Visibility.Visible;
            ThemeLabel.Visibility = Visibility.Visible;

            ReturnFromSettings.Visibility = Visibility.Visible;
            GoToMainMenu.Visibility = Visibility.Visible;
        }
        public void HideSettings()
        {
            GoToMainMenu.Visibility = Visibility.Collapsed;
            ReturnFromSettings.Visibility = Visibility.Collapsed;
            GoToSettings.Visibility = Visibility.Visible;
            AngleMeasurement.Visibility = Visibility.Collapsed;
            AngleMeasurementLabel.Visibility = Visibility.Collapsed;
            Theme.Visibility = Visibility.Collapsed;
            ThemeLabel.Visibility = Visibility.Collapsed;
        }

        private void OpenMainMenu(object sender = null, RoutedEventArgs e = null)
        {
            currentPage = 0;

            Title.Visibility = Visibility.Visible;
            TitleBackDrop.Visibility = Visibility.Visible;

            HideSettings();
            HideFunctions();
            HideTransformations();
            HideCalculations();
            HideComplexPlane();

            ShowMainMenu();
        }
        private void OpenFunctions(object sender = null, RoutedEventArgs e = null)
        {
            ResetGraph_Clicked();
            OpenSideBar.Content = ">";
            currentPage = 1;
            HideMainMenu();
            ShowFunctions();
        }
        private void OpenTransformations(object sender = null, RoutedEventArgs e = null)
        {
            ResetGraph_Clicked();
            OpenSideBar.Content = ">";
            currentPage = 2;
            HideMainMenu();
            ShowTransformations();
        }
        private void OpenCalculations(object sender = null, RoutedEventArgs e = null)
        {
            ResetGraph_Clicked();
            currentPage = 3;
            HideMainMenu();
            ShowCalculations();
        }
        private void OpenComplexNumbers(object sender = null, RoutedEventArgs e = null)
        {
            ResetGraph_Clicked();
            OpenSideBar.Content = ">";
            currentPage = 4;
            HideMainMenu();
            ShowComplexPlane();
        }
        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            HideCalculations();
            HideTransformations();
            HideFunctions();
            HideComplexPlane();
            HideMainMenu();

            ShowSettings();
        }

        private void ResetGraph_Clicked(object sender = null, RoutedEventArgs e = null)
        {
            Graph.Plot.SetAxisLimits(-10, 10, -5, 5);
            Graph.Plot.AddHorizontalLine(y: 0, color: System.Drawing.Color.Black);
            Graph.Plot.AddVerticalLine(x: 0, color: System.Drawing.Color.Black);

            Graph.Plot.Style(ScottPlot.Style.GetStyles()[Global.ThemeIndex]);

            Graph.Refresh();
        }
        private void ExitSettings(object sender, RoutedEventArgs e)
        {
            HideSettings();
            switch (currentPage)
            {
                case 1:
                    OpenFunctions();
                    break;
                case 2:
                    OpenTransformations();
                    break;
                case 3:
                    OpenCalculations();
                    break;
                case 4:
                    OpenComplexNumbers();
                    break;
                default:
                    OpenMainMenu();
                    break;
            }
        }

        private void AngleMeasurement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Global.angleFormatIndex = AngleMeasurement.SelectedIndex;
        }
        private void Theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Global.ThemeIndex = Theme.SelectedIndex;
            Graph.Plot.Style(ScottPlot.Style.GetStyles()[Global.ThemeIndex]);
        }

        private void ToggleModulo(object sender, RoutedEventArgs e)
        {
            if (!showAltForm)
            {
                Calculation_OutputRender0.Source = BitmapToImageSource(convertLatexToPng(alt_result));
                showAltForm = true;    
            }
            else
            { 
                Calculation_OutputRender0.Source = BitmapToImageSource(convertLatexToPng(result));
                showAltForm = false;
            }
        }
    }
}
