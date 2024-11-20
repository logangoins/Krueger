using System;
using System.Management;

namespace Krueger.Modules
{
    internal class Reboot
    {
        public static bool WmiReboot(string computer)
        {
            try
            {
                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope("\\\\" + computer + "\\root\\cimv2");
                scope.Connect();
                ObjectGetOptions objectGetOptions = new ObjectGetOptions();
                ManagementPath managementPath = new ManagementPath("Win32_Process");
                ManagementClass processClass = new ManagementClass(scope, managementPath, objectGetOptions);
                ManagementBaseObject inParams = processClass.GetMethodParameters("Create");
                inParams["CommandLine"] = "shutdown /r /t 0";
                ManagementBaseObject outParams = processClass.InvokeMethod("Create", inParams, null);

                if (outParams["returnValue"].ToString() == "0")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (ManagementException err)
            {
                Console.WriteLine("An error occurred while trying to execute the WMI method: " + err.Message);
            }
            catch (System.UnauthorizedAccessException unauthorizedErr)
            {
                Console.WriteLine("Connection error (user name or password might be incorrect): " + unauthorizedErr.Message);
            }
            return false;  

        }
    }
}
