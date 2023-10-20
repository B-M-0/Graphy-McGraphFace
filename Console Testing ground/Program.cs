using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using WpfMath;
using WpfMath.Parsers;
using XamlMath.Exceptions;
using System.Numerics;


namespace ConsoleApplication2
{
    internal class Program
    {  
        static void drawimage(string filename)
        {

            Bitmap image = new Bitmap(filename);

            int x, y;
            int height = image.Height;
            int width = image.Width;
            Console.WriteLine($"{width}, {height}");

            for (y = 0; y < height; y++)
            {


                for (x = 0; x < width; x++)
                {
                    
                    Color TruePixelColor = image.GetPixel(x, y);

                    if (TruePixelColor.A == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else 
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
          
                    Console.Write($"* ");
                }
                Console.WriteLine();

            }
            Console.ReadKey();
        }

        public static Bitmap convertLatexToPng(string fileName, string latex)
        {

            fileName =  fileName + ".png";


            var a = new Complex(5, 5) * 3;

            Console.ReadKey();
            try
            {
                var parser = WpfTeXFormulaParser.Instance;
                var formula = parser.Parse(latex);
                var png_Data = formula.RenderToPng(40.0, 0.0, 0.0, "Arial"); 

                File.WriteAllBytes(fileName, png_Data);

                Bitmap Image = new Bitmap(fileName);
                return Image;
            }
            catch (TexException e)
            {
                Console.WriteLine("Error when parsing formula: " + e.Message);
                return null;    
            }
        }


        public static void Main(string[] args)
        {

            // convertLatexToPng("Test1", @"e^{i \theta} = \cos(\theta) + i \sin(\theta)");
            //convertLatexToPng("Test1",@"\hat{ g} (f) =  \int_{ -\infty}^{\infty}g(t) e ^{ -2 \pi i f t}\, dt");
            convertLatexToPng("Test1", "\\Hut");
        drawimage("Test1.png");

        
        }
    }
}