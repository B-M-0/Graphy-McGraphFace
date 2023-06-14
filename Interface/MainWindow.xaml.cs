using System.IO;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using WpfMath.Parsers;
using WpfMath;
using System.Drawing;
using XamlMath.Exceptions;
using ScottPlot;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

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


        BitmapSource BitmapToImageSource(Bitmap bitmap)
        {
            BitmapSource bitmapsource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            //^ this line is stolen         
            return bitmapsource;
        }



        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SideBar.Visibility == System.Windows.Visibility.Collapsed)
            {
                SideBar.Visibility = System.Windows.Visibility.Visible;
                Input0.Visibility = System.Windows.Visibility.Visible;
                Input1.Visibility = System.Windows.Visibility.Visible;
                Input2.Visibility = System.Windows.Visibility.Visible;
                Input0Render.Visibility = System.Windows.Visibility.Visible;
                Input1Render.Visibility = System.Windows.Visibility.Visible;
                Input2Render.Visibility = System.Windows.Visibility.Visible;
                Input0Render.Visibility = System.Windows.Visibility.Visible;
                Input0RenderBackDrop.Visibility = System.Windows.Visibility.Visible;
                Input1RenderBackDrop.Visibility = System.Windows.Visibility.Visible;
                Input2RenderBackDrop.Visibility = System.Windows.Visibility.Visible;
                GoToMainMenu.Visibility = System.Windows.Visibility.Visible;

                Grid.SetColumn(Graph ,3);
                Grid.SetColumnSpan(Graph, 2);
                   

                (sender as Button).Content = "<";

            }
            else
            {
                Grid.SetColumn(Graph, 1);      
                Grid.SetColumnSpan(Graph, 4);


                SideBar.Visibility = System.Windows.Visibility.Collapsed;
                Input0.Visibility = System.Windows.Visibility.Collapsed;
                Input1.Visibility = System.Windows.Visibility.Collapsed;
                Input2.Visibility = System.Windows.Visibility.Collapsed;
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
        private void TextBox0_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Input0Render.Source = BitmapToImageSource(convertLatexToPng(Input0.Text));
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error when parsing formula: " + exception.Message);        
            }

        }
        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Input1Render.Source = BitmapToImageSource(convertLatexToPng(Input1.Text));
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error when parsing formula: " + exception.Message);
            }
        }

        private void TextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Input2Render.Source = BitmapToImageSource(convertLatexToPng(Input2.Text));
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error when parsing formula: " + exception.Message);
            }
        }


        private void OpenMainMenu(object sender, RoutedEventArgs e)
        {
           
            
        }
    }
}
