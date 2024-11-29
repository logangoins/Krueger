using System;
using System.ComponentModel;
using System.Management;
using System.Runtime.InteropServices;

namespace Krueger.Modules
{
    internal class Reboot
    {
        public static bool reboot(string computer)
        {
            bool shutdown = Interop.InitiateSystemShutdownEx(computer, "testing", 0, true, true, ShutdownReason.SHTDN_REASON_MAJOR_OTHER);
            if (shutdown)
            {
                return true;
            }
            else
            {
                string errorMessage = new Win32Exception(Marshal.GetLastWin32Error()).Message;
                Console.WriteLine("[!] Error: " + errorMessage);
                return false;
            }
        }
    }
}
