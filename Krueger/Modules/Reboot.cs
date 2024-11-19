using System.Management;

namespace Krueger.Modules
{
    internal class Reboot
    {
        public static void reboot(string computer)
        {
            ManagementScope scope = new ManagementScope("\\\\" + computer + "\\root\\cimv2");
            scope.Connect();
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            foreach (ManagementObject os in searcher.Get())
            {
                // Obtain in-parameters for the method
                ManagementBaseObject inParams =
                    os.GetMethodParameters("Win32Shutdown");

                // Add the input parameters.
                inParams["Flags"] = 2;

                // Execute the method and obtain the return values.
                ManagementBaseObject outParams =
                    os.InvokeMethod("Win32Shutdown", inParams, null);
            }

        }
    }
}
