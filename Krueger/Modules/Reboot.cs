using System;
using System.ComponentModel;
using System.Management;
using System.Runtime.InteropServices;

namespace Krueger.Modules
{
    internal class Reboot
    {
        public static bool reboot(string computer, bool warn)
        {
            bool shutdown = false;
            if (warn)
            {
                shutdown = Interop.InitiateSystemShutdownEx(computer, "System will restart in 2 minutes due to an update.", 120, true, true, ShutdownReason.SHTDN_REASON_MAJOR_OTHER | ShutdownReason.SHTDN_REASON_MINOR_INSTALLATION);
            }
            else
            {
                shutdown = Interop.InitiateSystemShutdownEx(computer, "", 0, true, true, ShutdownReason.SHTDN_REASON_MAJOR_OTHER);
            }
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
