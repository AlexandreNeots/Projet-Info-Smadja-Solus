using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TD2
{
    class Program
    {
        //objectif : matrice de convolution pour après les vacances 

        static void Main(string[] args)
        {
            MyImage image = new MyImage("test.bmp");
            image.toStringMatriceRGB();
            Console.ReadKey();
        }
    }
}
