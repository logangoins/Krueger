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

            Console.WriteLine("\n ██ ▄█▀ ██▀███   █    ██ ▓█████   ▄████ ▓█████  ██▀███  \r\n ██▄█▒ ▓██ ▒ ██▒ ██  ▓██▒▓█   ▀  ██▒ ▀█▒▓█   ▀ ▓██ ▒ ██▒\r\n▓███▄░ ▓██ ░▄█ ▒▓██  ▒██░▒███   ▒██░▄▄▄░▒███   ▓██ ░▄█ ▒\r\n▓██ █▄ ▒██▀▀█▄  ▓▓█  ░██░▒▓█  ▄ ░▓█  ██▓▒▓█  ▄ ▒██▀▀█▄  \r\n▒██▒ █▄░██▓ ▒██▒▒▒█████▓ ░▒████▒░▒▓███▀▒░▒████▒░██▓ ▒██▒\r\n▒ ▒▒ ▓▒░ ▒▓ ░▒▓░░▒▓▒ ▒ ▒ ░░ ▒░ ░ ░▒   ▒ ░░ ▒░ ░░ ▒▓ ░▒▓░\r\n░ ░▒ ▒░  ░▒ ░ ▒░░░▒░ ░ ░  ░ ░  ░  ░   ░  ░ ░  ░  ░▒ ░ ▒░\r\n░ ░░ ░   ░░   ░  ░░░ ░ ░    ░   ░ ░   ░    ░     ░░   ░ \r\n░  ░      ░        ░        ░  ░      ░    ░  ░   ░ \n");

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
