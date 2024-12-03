using System;
using System.ServiceProcess;

namespace Krueger.Modules
{
    internal class Services
    {
        public static void StartRemoteRegistry(string computer)
        {
            IntPtr scmHandle = IntPtr.Zero;
            IntPtr serviceHandle = IntPtr.Zero;
            IntPtr sHandle = IntPtr.Zero;  

            try
            {
                string remoteMachine = @"\\" + computer; 
                string serviceName = "RemoteRegistry"; 
                
                scmHandle = Interop.OpenSCManager(remoteMachine, null, Interop.SC_MANAGER_ALL_ACCESS);
                if (scmHandle == IntPtr.Zero)
                {
                    Console.WriteLine("[!] Failed to connect to the Service Control Manager.");
                    return;
                }

                sHandle = Interop.OpenService(scmHandle, serviceName, Interop.SERVICE_ALL_ACCESS);
                if (sHandle == IntPtr.Zero)
                {
                    Console.WriteLine("[!] Failed to open service.");
                    Interop.CloseServiceHandle(scmHandle);
                    return;
                }

                bool success = Interop.ChangeServiceConfig(
                    sHandle,
                    (uint) Service.SERVICE_WIN32_OWN_PROCESS,
                    (uint) Service.SERVICE_DEMAND_START,      
                    0,                               
                    null,                      
                    null,                     
                    IntPtr.Zero,             
                    null,            
                    null,              
                    null,                
                    null                     
                );

                if (success)
                {
                    Console.WriteLine("[+] Successfully changed service startup type.");
                }
                else
                {
                    uint error = Interop.GetLastError();
                    Console.WriteLine($"[!] Failed to change startup type. Error code: {error}");
                }

                Interop.CloseServiceHandle(scmHandle);
                Interop.CloseServiceHandle(sHandle);

                ServiceController serviceController = new ServiceController(serviceName, computer);

                if (serviceController.Status != ServiceControllerStatus.Running)
                {
                    Console.WriteLine("[+] Starting the service...");
                    serviceController.Start();
                    serviceController.WaitForStatus(ServiceControllerStatus.Running);
                    Console.WriteLine("[+] Service started successfully.");
                }
                else
                {
                    Console.WriteLine("[!] The service is already running.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error: {ex.Message}");
            }

        }
    }
}
