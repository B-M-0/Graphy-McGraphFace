using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfMath;
using WpfMath.Parsers;

namespace Interface
{
   
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
        FunctionPlot[] functionPlots = new FunctionPlot[input_count];
        List<double>[] asymptotes = new List<double>[] { new List<double>(), new List<double>(), new List<double>() };

        double[] axisBounds = new double[input_count];
        IPlottable[] Locus = new IPlottable[input_count];
        bool[] Loci_plotted = new bool[input_count];
        bool[] Loci_isHalfLine = new bool[input_count];

        bool[] valid_function = new bool[input_count];
        bool[] valid_transformation = new bool[input_count];
        bool[] valid_complex = new bool[input_count];

        int currentPage = 0;
        int lastPage = 0;
        string result;
        string alt_result;
        bool showAltForm = false;

        string[] function_inputs = new string[input_count];
        string[] transformation_inputs = new string[input_count];
        string[] complex_inputs = new string[input_count];
        string[] parsed_complex_inputs = new string[input_count];


        public bool BracketsBalanced(string input)
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
        public string FormatLatex(string input, string mode = "f")
        {
            //Modes f ==> functions, c ==> calcuations
            string temp = "";
            for (int i = 0; i < input.Length; i++)
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
                        if (mode == "c")
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
        public static Bitmap ConvertLatexToPng(string latex)
        {
            Bitmap Image;
            var parser = WpfTeXFormulaParser.Instance;
            var formula = parser.Parse(latex);


            var png_Data = formula.RenderToPng(190.0, 4.0, 2.0, "Arial"); ;

            using (MemoryStream ms = new MemoryStream(png_Data))
            {
                Image = new Bitmap(ms);
            }

            return Image;
        }
        ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            //^ this line is stolen         
            return bitmapSource;
        }


        public MainWindow()
        {
            InitializeComponent();
            TitleText.Source = BitmapToImageSource(ConvertLatexToPng("Graphy McGraphFace"));

            OpenMainMenu();
            function_inputs = new string[] { Function_Input0.Text, Function_Input1.Text, Function_Input2.Text };
            for (int i = 0; i < valid_function.Length; i++)
            {
                valid_function[i] = false;
                valid_transformation[i] = false;
            }
        }


        public void PlotMatrix(Matrix M)
        {
            if (M.rows == 2)
            {
                double[] xs = new double[M.columns];
                double[] ys = new double[M.columns];
                for (int position = 0; position < M.columns; position++)
                {
                    xs[position] = M[0, position];
                    ys[position] = M[1, position];

                    if (M.columns > 2)
                        Graph.Plot.AddPolygon(xs, ys, Graph.Plot.GetNextColor(.3), lineWidth: 2);

                    Graph.Plot.AddScatter(xs, ys, lineWidth: 0, markerSize: 7, color: System.Drawing.Color.Black);
                }
            }
        }

        private void ResetGraph_Clicked(object? sender = null, RoutedEventArgs? e = null)
        {

            Graph.Plot.SetAxisLimits(-15, 15, -15, 15);
            Graph.Plot.AddHorizontalLine(y: 0, color: System.Drawing.Color.Black);
            Graph.Plot.AddVerticalLine(x: 0, color: System.Drawing.Color.Black);

            Graph.Plot.Style(ScottPlot.Style.GetStyles()[Global.ThemeIndex]);
            GraphChanged(null, null);
            Graph.Refresh();
        }
        public void UpdateRenders(int id, ImageSource Render)
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
        public void UpdateGraph_Functions()
        {
            Graph.Plot.Clear();
            UpdateText();
            ResetGraph_Clicked();

            Dictionary<string, double> variables = Global.DefineConstants<double>();

            Graph.Plot.Style(ScottPlot.Style.GetStyles()[Global.ThemeIndex]);

            Graph.Plot.AddHorizontalLine(y: 0, color: System.Drawing.Color.Black);
            Graph.Plot.AddVerticalLine(x: 0, color: System.Drawing.Color.Black);

            for (int i = 0; i < 3; ++i)
            {
                if (valid_function[i] && function_inputs[i] != "")
                {
                    bool validInput = valid_function[i];
                    asymptotes[i].Clear();
                    Function function = functions[i];


                    var func = new Func<double, double?>((x) => function.Evaluate(variables, ref validInput, i, ref asymptotes, x)); ;
                    if (validInput)
                    {
                        var plt = new FunctionPlot(func);
                        functionPlots[i] = plt;
                        plt.Color = System.Drawing.Color.FromName(Global.colors[i]);
                        Graph.Plot.Add(plt);
                        foreach (double xVal in asymptotes[i])
                        {
                            Graph.Plot.AddVerticalLine(xVal, width: 2, color: System.Drawing.Color.Red, style: LineStyle.Dot);
                        }
                    }
                }
            }

            Graph.Plot.SetAxisLimits(-15, 15, -15, 15);
            Graph.Refresh();
        }
        public void UpdateGraph_Transformations()
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
                    double decimalVal = 0;

                    Dictionary<string, double> variables = new Dictionary<string, double>();
                    Dictionary<string, Matrix> matrices = Global.DefineCommonMatrices();
                    Dictionary<string, Vector> vectors = new Dictionary<string, Vector>();

                    Matrix Result = function.Evaluate(variables, matrices, vectors, ref decimalVal);
                    for (int ResultRow = 0; ResultRow < Result.rows; ResultRow++)
                    {
                        for (int ResultColumn = 0; ResultColumn < Result.columns; ResultColumn++)
                        {
                            if (Result[ResultRow, ResultColumn] == null)
                            {
                                return;
                            }
                        }
                    }
                    Graph.Refresh();
                    PlotMatrix(Result);

                    if (function.GetFunction().Contains("U") && !UPlotted)
                    {
                        PlotMatrix(matrices["U"]);
                        UPlotted = true;
                    }
                }

                Graph.Plot.SetAxisLimits(-15, 15, -15, 15);
                Graph.Refresh();
            }
        }
        public void UpdateGraph_ComplexLoci()
        {
            Graph.Plot.Clear();
            Graph.Plot.AddHorizontalLine(y: 0, color: System.Drawing.Color.Black);
            Graph.Plot.AddVerticalLine(x: 0, color: System.Drawing.Color.Black);


            for (int i = 0; i < complex_inputs.Length; i++)
            {
                if (valid_complex[i] == false)
                    continue;
                string input = parsed_complex_inputs[i];
                string typeOfLoci = InputHandling.IdentifyLoci(input);
                string[] expression = input.Split('=');
                List<string> LHS;
                List<string> RHS;
                LHS = InputHandling.InfixToPostfix(expression[0].Split(' '), ref valid_complex[i]);
                RHS = InputHandling.InfixToPostfix(expression[1].Split(' '), ref valid_complex[i]);
                // Types :: HalfLine (H), Bisector (B), Circle (C), Undefined (U)

                Loci_isHalfLine[i] = false;
                Loci_plotted[i] = false;

                try
                {
                    switch (typeOfLoci)
                    {
                        case "H":
                            Tuple<ComplexNumber, Rational> halfLineStats = InputHandling.ParseComplexLoci_HalfLine(LHS, RHS);
                            double centerX = -halfLineStats.Item1.real;
                            double centerY = -halfLineStats.Item1.imaginary;
                            double angle = halfLineStats.Item2;
                            if (angle > Math.PI || angle <= -Math.PI || !LHS.Contains("z"))
                                break;

                            //center
                            Graph.Plot.AddCircle(centerX, centerY, 0.25, lineWidth: (float)2);
                            //Loci
                            var func = new Func<double, double?>((x) => Global.HalfLine(x, angle, centerY, centerX));
                            var plt = new FunctionPlot(func);
                            plt.LineWidth = 2;
                            plt.Color = System.Drawing.Color.Red;
                            Locus[i] = plt;
                            Loci_isHalfLine[i] = true;
                            Loci_plotted[i] = true;
                            Graph.Plot.Add(plt);
                            axisBounds[i] = centerX;
                            Graph.Plot.SetAxisLimits(centerX - 15, centerX + 15, centerY - 15, centerY + 15);
                            break;
                        case "C":
                            Tuple<ComplexNumber, Rational> CircleStats = InputHandling.ParseComplexLoci_Circle(LHS, RHS);
                            centerX = -CircleStats.Item1.real;
                            centerY = -CircleStats.Item1.imaginary;
                            double radius = CircleStats.Item2;
                            //center
                            Graph.Plot.AddCircle(centerX, centerY, radius / 100, lineWidth: (float)2);
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

            Graph.Refresh();
        }
        private void SideBarButton_Clicked(object sender, RoutedEventArgs? e)
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

            string inputText = (sender as TextBox).Text;
            string Latex = FormatLatex(inputText, "c");
            bool validInput = true;


            Dictionary<string, Surd> surdVariables = new Dictionary<string, Surd>();
            Dictionary<string, ComplexNumber> complexVariables = new Dictionary<string, ComplexNumber>();
            Dictionary<string, double> doubleVariables = new Dictionary<string, double>();
            Dictionary<string, Matrix> matrices = Global.DefineCommonMatrices();
            Dictionary<string, Vector> vectors = new Dictionary<string, Vector>();


            try
            {
                Calculation_OutputRender0.Source = null;
                Calculation_InputRender0.Source = BitmapToImageSource(ConvertLatexToPng(Latex));
            }
            catch
            {
                Calculation_InputRender0.Source = null;
            }
            Graph.Plot.Clear();

            
            
            try
            {
                List<string> output = InputHandling.InfixToPostfix(InputHandling.FormatInput(Global.RemoveWhiteSpace((sender as TextBox).Text), ref validInput).Split(' '), ref validInput);
                if (!validInput)
                    throw new Exception("Invalid input");

                Function f = new Function(output);
                result = "=";
                if (inputText.Contains("\\M") || inputText.Contains("I(") || Regex.IsMatch(inputText, "[A-Z]+$"))
                {
                    double decialVal = 0;
                    Matrix Alfa = f.Evaluate(doubleVariables, matrices, vectors, ref decialVal);
                    if (Alfa is null)
                        result += decialVal.ToString();
                    else
                        result += Alfa.MatrixToLatex();
                }
                else
                {
                    double decimalValue = 0;
                    Surd surdValue = 0;
                    ComplexNumber resultVAl = f.Evaluate(surdVariables, complexVariables, ref decimalValue, ref surdValue);

                    if (resultVAl.IsComplex())
                    {
                        ToggleModuloButton.Visibility = Visibility.Visible;
                        alt_result = resultVAl.ModulusArgumentForm();
                    }
                    else if (surdValue != null && surdValue != 0.0)
                    {
                        ToggleModuloButton.Visibility = Visibility.Visible;
                        alt_result = surdValue.ToLatex();
                    }
                    else
                    {
                        ToggleModuloButton.Visibility = Visibility.Collapsed;
                    }

                    if (resultVAl == null)
                        result += Math.Round(decimalValue, Global.decimalPlaces).ToString();
                    else if (resultVAl.imaginary.isFloat || resultVAl.real.isFloat)
                        result = "\\approx" + (resultVAl.LatexFormat_float());
                    else if (resultVAl.LatexFormat_frac() != resultVAl.LatexFormat_float())
                        result += resultVAl.LatexFormat_frac() + "=" + resultVAl.LatexFormat_float();
                    else
                        result += resultVAl.LatexFormat_frac();

                }
                Calculation_OutputRender0.Source = BitmapToImageSource(ConvertLatexToPng(result));
            }
            catch
            {

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

            if (BracketsBalanced(inputs[id]))
            {
                try
                {
                    string Latex = FormatLatex(inputs[id]);
                    UpdateRenders(id, BitmapToImageSource(ConvertLatexToPng(Latex)));
                }
                catch
                {
                    UpdateRenders(id, null);
                }
                Graph.Plot.Clear();

                try
                {
                    bool validInput = true;
                    string input_string = InputHandling.FormatInput(Global.RemoveWhiteSpace(inputs[id]), ref validInput);
                    List<string> expression = InputHandling.InfixToPostfix(input_string.Split(' '), ref validInput);
                    if (currentPage == 4)
                        parsed_complex_inputs[id] = input_string;
                    else
                        functions[id] = new Function(expression);
                    valid[id] = validInput;


                    if (currentPage == 1)
                        valid_function = valid;
                    else if (currentPage == 2)
                        valid_transformation = valid;
                    else
                        valid_complex = valid;

                    if (currentPage == 1)
                        UpdateGraph_Functions();
                    else if (currentPage == 2)
                        UpdateGraph_Transformations();
                    else if (currentPage == 4)
                        UpdateGraph_ComplexLoci();
                }
                catch (Exception exception)
                {
                    Console.Error.WriteLine("Error when parsing formula: " + exception.Message);
                }
;
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
            TitleText.Visibility = Visibility.Collapsed;
            TitleBackDrop.Visibility = Visibility.Collapsed;
            GoToFunctions.Visibility = Visibility.Collapsed;
            GoToTransformations.Visibility = Visibility.Collapsed;
            GoToCalculations.Visibility = Visibility.Collapsed;
            GoToComplexPlane.Visibility = Visibility.Collapsed;
        }

        public void ShowSettings()
        {
            GoToSettings.Visibility = Visibility.Collapsed;

            AngleMeasurementLabel.Visibility = Visibility.Visible;
            AngleMeasurement.Visibility = Visibility.Visible;
            Theme.Visibility = Visibility.Visible;
            ThemeLabel.Visibility = Visibility.Visible;

            HelpInfo.Visibility = Visibility.Visible;

            ReturnToLast.Visibility = Visibility.Visible;
            GoToMainMenu.Visibility = Visibility.Visible;
        }
        public void HideSettings()
        {
            GoToMainMenu.Visibility = Visibility.Collapsed;
            ReturnToLast.Visibility = Visibility.Collapsed;
            GoToSettings.Visibility = Visibility.Visible;
            AngleMeasurement.Visibility = Visibility.Collapsed;
            AngleMeasurementLabel.Visibility = Visibility.Collapsed;
            HelpInfo.Visibility = Visibility.Collapsed;

            Theme.Visibility = Visibility.Collapsed;
            ThemeLabel.Visibility = Visibility.Collapsed;
        }


        private void OpenMainMenu(object? sender = null, RoutedEventArgs? e = null)
        {
            lastPage = currentPage;
            currentPage = 0;

            TitleText.Visibility = Visibility.Visible;
            TitleBackDrop.Visibility = Visibility.Visible;

            HideSettings();
            HideFunctions();
            HideTransformations();
            HideCalculations();
            HideComplexPlane();
            ShowMainMenu();
        }
        private void OpenFunctions(object? sender = null, RoutedEventArgs? e = null)
        {
            lastPage = currentPage;
            currentPage = 1;

            ResetGraph_Clicked();

            for (int i = 0; i < input_count; i++)
                TextChanged(i);

            UpdateGraph_Functions();
            OpenSideBar.Content = ">";

            HideMainMenu();
            ShowFunctions();
        }
        private void OpenTransformations(object? sender = null, RoutedEventArgs? e = null)
        {
            lastPage = currentPage;
            currentPage = 2;

            ResetGraph_Clicked();
            UpdateGraph_Transformations();
            for (int i = 0; i < input_count; i++)
                TextChanged(i);
            OpenSideBar.Content = ">";

            HideMainMenu();
            ShowTransformations();
        }
        private void OpenCalculations(object? sender = null, RoutedEventArgs? e = null)
        {
            ResetGraph_Clicked();
            lastPage = currentPage;
            currentPage = 3;
            HideMainMenu();
            ShowCalculations();
        }
        private void OpenComplexNumbers(object? sender = null, RoutedEventArgs? e = null)
        {
            lastPage = currentPage;
            currentPage = 4;

            ResetGraph_Clicked();
            UpdateGraph_ComplexLoci();
            for (int i = 0; i < input_count; i++)
                TextChanged(i);
            OpenSideBar.Content = ">";
            HideMainMenu();
            ShowComplexPlane();
        }
        private void OpenSettings(object? sender = null, RoutedEventArgs? e = null)
        {
            lastPage = currentPage;
            currentPage = 7;
            HideCalculations();
            HideTransformations();
            HideFunctions();
            HideComplexPlane();
            HideMainMenu();
            ShowSettings();
        }
        private void ReturnToLastWindow(object? sender, RoutedEventArgs? e)
        {
            HideSettings();

            switch (lastPage)
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
                case 7:
                    OpenSettings();
                    break;
                default:
                    OpenMainMenu();
                    break;
            }
        }

        private void AngleMeasurement_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            Global.angleFormatIndex = AngleMeasurement.SelectedIndex;
        }
        private void Theme_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            Global.ThemeIndex = Theme.SelectedIndex;
            Graph.Plot.Style(ScottPlot.Style.GetStyles()[Global.ThemeIndex]);
        }

        private void ToggleAlternateFormat(object? sender, RoutedEventArgs? e)
        {
            if (!showAltForm)
            {
                Calculation_OutputRender0.Source = BitmapToImageSource(ConvertLatexToPng(alt_result));
                showAltForm = true;
            }
            else
            {
                Calculation_OutputRender0.Source = BitmapToImageSource(ConvertLatexToPng(result));
                showAltForm = false;
            }
        }

        private void GraphChanged(object? sender, EventArgs e)
        {
            if (currentPage == 4)
            {
                double error = Graph.Plot.GetAxisLimits(0).XSpan / 30;

                double maxX = Graph.Plot.GetAxisLimits(0).XMax;
                double minX = Graph.Plot.GetAxisLimits(0).XMin;
                for (int i = 0; i < input_count; i++)
                {

                    if (Loci_plotted[i] == true && (maxX <= axisBounds[i] + error || minX + error >= axisBounds[i]) && Loci_isHalfLine[i] == true)
                    {
                        Graph.Plot.Remove(Locus[i]);
                        Loci_plotted[i] = false;
                        Graph.Refresh();
                    }
                    else if (Loci_plotted[i] == false && Loci_isHalfLine[i] == true && maxX >= axisBounds[i] + error && minX + error <= axisBounds[i])
                    {
                        Graph.Plot.Add(Locus[i]);
                        Loci_plotted[i] = true;
                    }

                }
            }
        }


    }
}
