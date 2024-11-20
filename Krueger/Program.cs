using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace Krueger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" ____  __.                                         \r\n|    |/ _|______ __ __   ____   ____   ___________ \r\n|      < \\_  __ \\  |  \\_/ __ \\ / ___\\_/ __ \\_  __ \\\r\n|    |  \\ |  | \\/  |  /\\  ___// /_/  >  ___/|  | \\/\r\n|____|__ \\|__|  |____/  \\___  >___  / \\___  >__|   \r\n        \\/                  \\/_____/      \\/");
            Console.WriteLine("~~~~~~\n@_logangoins\n@hullabrian\n");

            try
            {
                Modules.ArgParse.Execute(args);
            }
            catch (Exception e)
            {
                Console.WriteLine("[!] Exception: " + e.Message);
            }
        }
    }
}
