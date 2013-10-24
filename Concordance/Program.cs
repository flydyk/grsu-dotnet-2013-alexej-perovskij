using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Concordance
{
    class Program
    {
        static void Main(string[] args)
        {
            Concordance concordance = null;
            switch (args.Length)
            {
                case 0:
                    Console.Write("Enter a FULL PATH to txt file for which you want create corcondance: ");
                    BuildConcordance(Console.ReadLine(), concordance);
                    break;
                case 1:
                    BuildConcordance(args[0], concordance);
                    break;
                default:
                    break;
            }
            Console.ReadLine();
        }

        private static void BuildConcordance(string loadPath, Concordance concordance)
        {
            concordance = new Concordance(loadPath, null);
            concordance.GenerateConcordance();
            string path =
                Path.GetFileNameWithoutExtension(loadPath) +
                "_concordance" +
                Path.GetExtension(loadPath);

            concordance.PrintToFile(path);
            Console.Write("Corcordance was succesfully created.\nPress any key to exit..");
        }
    }
}
