using ScottPlot.Ticks.DateTimeTickUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Global
    {
        static public int decimalPlaces = 7;
        static public Dictionary<string,Rational> defineConstants()
        {
            return new Dictionary<string, Rational> { { "e", Math.Round(Math.E, Global.decimalPlaces) }, { "\\pi", Math.Round(Math.PI, Global.decimalPlaces) } };
        }
        static public Dictionary<string, double> defineConstants<T>()
        {
           return new Dictionary<string, double> { { "e", Math.Round(Math.E, Global.decimalPlaces) }, { "\\pi", Math.Round(Math.PI, Global.decimalPlaces) } };
        }


    }
}
