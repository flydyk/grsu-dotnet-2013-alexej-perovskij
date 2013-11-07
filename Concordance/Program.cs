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
            try
            {
                switch (args.Length)
                {
                    case 0:
                        Console.Write("Enter a FULL PATH to txt file for which you want create concordance: ");
                        BuildConcordance(Console.ReadLine(), concordance, Concordance.DefaultLinesPerPage);
                        break;
                    case 1:
                        BuildConcordance(args[0], concordance, Concordance.DefaultLinesPerPage);
                        break;
                    case 2:
                        BuildConcordance(args[0], concordance, int.Parse(args[1]));
                        break;
                    default:
                        break;
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("\nCouldn't find file " + ex.FileName);
            }
            catch (Exception e) { Console.WriteLine("\n" + e.Message); }
            finally
            {
                Console.Write("\nPress any key to exit..");
                Console.ReadKey();
            }

        }

        private static void BuildConcordance(string loadPath, Concordance concordance, int linesPerPage)
        {
            concordance = new Concordance(loadPath, null) { LinesPerPage = linesPerPage };
            concordance.GenerateConcordance();
            string path =
                Path.GetFileNameWithoutExtension(loadPath) +
                "_concordance" +
                Path.GetExtension(loadPath);

            concordance.PrintToFile(path);
            Console.Write("Concordance was succesfully created.");
        }
    }
}
