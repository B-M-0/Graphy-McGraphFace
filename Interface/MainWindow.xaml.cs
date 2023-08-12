
using ScottPlot.Drawing.Colormaps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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


    public partial class MainWindow : Window
    {

        static int input_count = 3;

        Function[] functions = new Function[input_count];

        Function function0;
        Function function1;
        Function function2;

        

        bool[] valid = new bool[input_count];

        int currentPage = 1;
        int universial_range = 20;
        int universial_lower_bound = -10;
        double universial_step = 0.1;

        string[] function_inputs = new string[input_count];
        string[] transformation_inputs = new string[input_count];
       
        public Dictionary<string, double> Vars = new Dictionary<string, double>() { { "e", Math.E }, { "\\pi", Math.PI } };
        public Dictionary<string, Matrix> Matrices = new Dictionary<string, Matrix>();


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

        public string formatLatex(string input)
        {
            string temp = "";
            foreach (char character in input)
            {
                switch (character)
                {
                    case ' ':
                    case '*':
                        break;
                    case '_':
                        temp += '-';
                        break;
                    default:
                        temp += character;
                        break;
                }
            }
            return temp;
        }

        public string removeWhiteSpace(string input)
        {
            string temp = "";
            foreach (char element in input)
            { 
                if(element != ' ')
                    temp+= element;
            }
            return temp;
        }


        public static Bitmap convertLatexToPng(string latex)
        {
            var parser = WpfTeXFormulaParser.Instance;
            var formula = parser.Parse(latex);
            var png_Data = formula.RenderToPng(190.0, 1.0, 1.0, "Arial");

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
            InitializeComponent();
            OpenMainMenu();
            function_inputs = new string[] { Function_Input0.Text, Function_Input1.Text, Function_Input2.Text };
            for (int i = 0; i < valid.Length; i++) valid[i] = false;

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
        }
    
        public void UpdateGraph_functions()
        {
            Graph.Plot.Clear();

            //Update();

            Graph.Plot.AddHorizontalLine(y: 0, color: System.Drawing.Color.Black);
            Graph.Plot.AddVerticalLine(x: 0, color: System.Drawing.Color.Black);

            for (int i = 0; i < input_count; i++)
            {
                if (valid[i])
                {
                    Function function = functions[i];
                    var func = new Func<double, double?>((x) => function.Evaluate(x, Vars));
                    Graph.Plot.AddFunction(func, lineWidth: 2);
                }

            }
       
            Graph.Plot.SetAxisLimits(-10, 10, -5, 5);
            Graph.Refresh();
        }

        public void UpdateGraph_transformations()
        {
            Graph.Plot.Clear();

            //Update();

            Graph.Plot.AddHorizontalLine(y: 0, color: System.Drawing.Color.Black);
            Graph.Plot.AddVerticalLine(x: 0, color: System.Drawing.Color.Black);

            for (int i = 0; i < input_count; i++)
            {
                if (valid[i])
                {
                    Function function = functions[i];


                    
                      
                    Matrix Result = function.Evaluate(Vars ,Matrices);
                    if (Result.rows == 2)
                    {
                        double[] xs = new double[Result.columns];
                        double[] ys = new double[Result.columns];
                        for (int position = 0; position < Result.columns; position++)
                        {
                            xs[position] = Result.Values[0, position];
                            ys[position] = Result.Values[1, position];
                            Graph.Plot.AddPolygon(xs, ys, lineWidth: 2);
                            Graph.Refresh();
                        }
                        

                    }
                }

            }

            Graph.Plot.SetAxisLimits(-10, 10, -5, 5);
            Graph.Refresh();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SideBar.Visibility == System.Windows.Visibility.Collapsed && OpenSideBar.Visibility == System.Windows.Visibility.Visible)
            {
                SideBar.Visibility = System.Windows.Visibility.Visible;
                if (currentPage == 1)
                {
                    Function_Input0.Visibility = System.Windows.Visibility.Visible;
                    Function_Input1.Visibility = System.Windows.Visibility.Visible;
                    Function_Input2.Visibility = System.Windows.Visibility.Visible;
                }
                else if(currentPage == 2)
                {
                    Transformation_Input0.Visibility = System.Windows.Visibility.Visible;
                    Transformation_Input1.Visibility = System.Windows.Visibility.Visible;
                    Transformation_Input2.Visibility = System.Windows.Visibility.Visible;
                }
                Input0Render.Visibility = System.Windows.Visibility.Visible;
                Input1Render.Visibility = System.Windows.Visibility.Visible;
                Input2Render.Visibility = System.Windows.Visibility.Visible;             
                Input0RenderBackDrop.Visibility = System.Windows.Visibility.Visible;
                Input1RenderBackDrop.Visibility = System.Windows.Visibility.Visible;
                Input2RenderBackDrop.Visibility = System.Windows.Visibility.Visible;
                GoToMainMenu.Visibility = System.Windows.Visibility.Visible;

                Grid.SetColumn(Graph, 3);
                Grid.SetColumnSpan(Graph, 2);


                (sender as Button).Content = "<";

            }
            else
            {
                Grid.SetColumn(Graph, 1);
                Grid.SetColumnSpan(Graph, 4);


                SideBar.Visibility = System.Windows.Visibility.Collapsed;
                if (currentPage == 1)
                {
                    Function_Input0.Visibility = System.Windows.Visibility.Collapsed;
                    Function_Input1.Visibility = System.Windows.Visibility.Collapsed;
                    Function_Input2.Visibility = System.Windows.Visibility.Collapsed;
                }
                else if(currentPage == 2)
                {
                    Transformation_Input0.Visibility = System.Windows.Visibility.Collapsed;
                    Transformation_Input1.Visibility = System.Windows.Visibility.Collapsed;
                    Transformation_Input2.Visibility = System.Windows.Visibility.Collapsed;
                }
                Input0Render.Visibility = System.Windows.Visibility.Collapsed;
                Input1Render.Visibility = System.Windows.Visibility.Collapsed;
                Input2Render.Visibility = System.Windows.Visibility.Collapsed;
                Input0RenderBackDrop.Visibility = System.Windows.Visibility.Collapsed;
                Input1RenderBackDrop.Visibility = System.Windows.Visibility.Collapsed;
                Input2RenderBackDrop.Visibility = System.Windows.Visibility.Collapsed;
                GoToMainMenu.Visibility = System.Windows.Visibility.Collapsed;
                (sender as Button).Content = ">";
            }
        }

        private void InputOrdering(object sender, TextChangedEventArgs e)
        {
            //foreach
        }

        public void TextChanged(int id)
        {
            UpdateText();
            string[] inputs;
            if (currentPage == 1)
                inputs = function_inputs;
            else winputs = transformation_inputs;

            if (bracketsBalanced(inputs[id]))
            {
                
                try
                {
                    string Latex = formatLatex(inputs[id]);
                    UpdateRender(id, BitmapToImageSource(convertLatexToPng(Latex)));

                    Graph.Plot.Clear();
                    List<string> output = InputHandling.infixToPostfix(InputHandling.formatInput(removeWhiteSpace(inputs[id])).Split(' '));

                    functions[id] = new Function(output);
                    valid[id] = true;

                    
                    if(currentPage == 1)
                        UpdateGraph_functions();
                    else if(currentPage == 2)
                        UpdateGraph_transformations();
                }
                catch (Exception exception)
                {
                    Console.Error.WriteLine("Error when parsing formula: " + exception.Message);
                }
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
            Graph.Visibility = System.Windows.Visibility.Visible;           
            OpenSideBar.Visibility = System.Windows.Visibility.Visible;

        }

        public void HideFunctions()
        {
            Graph.Visibility = System.Windows.Visibility.Collapsed;
            SideBar.Visibility = System.Windows.Visibility.Collapsed;
            Function_Input0.Visibility = System.Windows.Visibility.Collapsed;
            Function_Input1.Visibility = System.Windows.Visibility.Collapsed;
            Function_Input2.Visibility = System.Windows.Visibility.Collapsed;
            Input0Render.Visibility = System.Windows.Visibility.Collapsed;
            Input1Render.Visibility = System.Windows.Visibility.Collapsed;
            Input2Render.Visibility = System.Windows.Visibility.Collapsed;
            Input0Render.Visibility = System.Windows.Visibility.Collapsed;
            Input0RenderBackDrop.Visibility = System.Windows.Visibility.Collapsed;
            Input1RenderBackDrop.Visibility = System.Windows.Visibility.Collapsed;
            Input2RenderBackDrop.Visibility = System.Windows.Visibility.Collapsed;
            GoToMainMenu.Visibility = System.Windows.Visibility.Collapsed;
            OpenSideBar.Visibility = System.Windows.Visibility.Collapsed;
        }


        public void ShowTransformations()
        {
            Graph.Visibility = System.Windows.Visibility.Visible;          
            OpenSideBar.Visibility = System.Windows.Visibility.Visible;
            Grid.SetColumn(Graph, 1);
            Grid.SetColumnSpan(Graph, 4);
        }

        public void HideTransformations()
        {
            Graph.Visibility = System.Windows.Visibility.Collapsed;
            SideBar.Visibility = System.Windows.Visibility.Collapsed;
            Transformation_Input0.Visibility = System.Windows.Visibility.Collapsed;
            Transformation_Input1.Visibility = System.Windows.Visibility.Collapsed;
            Transformation_Input2.Visibility = System.Windows.Visibility.Collapsed;
            Input0Render.Visibility = System.Windows.Visibility.Collapsed;
            Input1Render.Visibility = System.Windows.Visibility.Collapsed;
            Input2Render.Visibility = System.Windows.Visibility.Collapsed;
            Input0Render.Visibility = System.Windows.Visibility.Collapsed;
            Input0RenderBackDrop.Visibility = System.Windows.Visibility.Collapsed;
            Input1RenderBackDrop.Visibility = System.Windows.Visibility.Collapsed;
            Input2RenderBackDrop.Visibility = System.Windows.Visibility.Collapsed;
            GoToMainMenu.Visibility = System.Windows.Visibility.Collapsed;
            OpenSideBar.Visibility = System.Windows.Visibility.Collapsed;
        }










        public void ShowMainMenu()
        {
            Grid.SetColumn(Graph, 1);
            Grid.SetColumnSpan(Graph, 4);
            GoToFunctions.Visibility = System.Windows.Visibility.Visible;
            GoToTransformations.Visibility = System.Windows.Visibility.Visible; 
        }
        public void HideMainMenu()
        {
            GoToFunctions.Visibility = System.Windows.Visibility.Collapsed;
            GoToTransformations.Visibility = System.Windows.Visibility.Collapsed;
        }













        private void OpenMainMenu(object sender = null, RoutedEventArgs e = null)
        {
            currentPage = 0;
            HideFunctions();
            HideTransformations();
            ShowMainMenu();
        }
       

        private void OpenFunctions(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            HideMainMenu();
            ShowFunctions();
        }

        private void OpenTransformations(object sender, RoutedEventArgs e)
        {
            currentPage = 2;
            HideMainMenu();
            ShowTransformations();
        }
    }
}
