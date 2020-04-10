using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNewFeature8ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
        }

    }
    public class MyClass
    {
        private int Age { get; set; }
        private string Name { get; set; }
    }
    public struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Distance => Math.Sqrt(X * X + Y * Y);

        //You can apply the readonly modifier to members of a struct. It indicates that the member doesn't modify state. It's more granular than applying the readonly modifier to a struct declaration. Consider the following mutable struct:
        public readonly override string ToString() =>
            $"({X}, {Y}) is {Distance} from the origin";
    }
}
