using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using WpfMath;
using WpfMath.Parsers;

namespace WpfTestingGrounds
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }




        public static Bitmap convertLatexToPng(string latex)
        {
            var parser = WpfTeXFormulaParser.Instance;
            var formula = parser.Parse(latex);
            var png_Data = formula.RenderToPng(40.0, 1.0, 1.0, "Arial");

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
    }
}
