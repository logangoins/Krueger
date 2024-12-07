using System;
using System.Runtime.InteropServices;

namespace Krueger.Modules
{
    public enum LogonType
    {
        LOGON32_LOGON_INTERACTIVE = 2,
        LOGON32_LOGON_NETWORK = 3,
        LOGON32_LOGON_BATCH = 4,
        LOGON32_LOGON_SERVICE = 5,
        LOGON32_LOGON_UNLOCK = 7,
        LOGON32_LOGON_NETWORK_CLEARTEXT = 8,
        LOGON32_LOGON_NEW_CREDENTIALS = 9,
    }

    public enum LogonProvider
    {
        LOGON32_PROVIDER_DEFAULT = 0,
        LOGON32_PROVIDER_WINNT35 = 1,
        LOGON32_PROVIDER_WINNT40 = 2,
        LOGON32_PROVIDER_WINNT50 = 3
    }

    [Flags]
    public enum ShutdownReason : uint
    {
        SHTDN_REASON_MAJOR_OTHER = 0x00000000,
        SHTDN_REASON_MAJOR_NONE = 0x00000000,
        SHTDN_REASON_MAJOR_HARDWARE = 0x00010000,
        SHTDN_REASON_MAJOR_OPERATINGSYSTEM = 0x00020000,
        SHTDN_REASON_MAJOR_SOFTWARE = 0x00030000,
        SHTDN_REASON_MAJOR_APPLICATION = 0x00040000,
        SHTDN_REASON_MAJOR_SYSTEM = 0x00050000,
        SHTDN_REASON_MAJOR_POWER = 0x00060000,
        SHTDN_REASON_MAJOR_LEGACY_API = 0x00070000,

        SHTDN_REASON_MINOR_OTHER = 0x00000000,
        SHTDN_REASON_MINOR_NONE = 0x000000ff,
        SHTDN_REASON_MINOR_MAINTENANCE = 0x00000001,
        SHTDN_REASON_MINOR_INSTALLATION = 0x00000002,
        SHTDN_REASON_MINOR_UPGRADE = 0x00000003,
        SHTDN_REASON_MINOR_RECONFIG = 0x00000004,
        SHTDN_REASON_MINOR_HUNG = 0x00000005,
        SHTDN_REASON_MINOR_UNSTABLE = 0x00000006,
        SHTDN_REASON_MINOR_DISK = 0x00000007,
        SHTDN_REASON_MINOR_PROCESSOR = 0x00000008,
        SHTDN_REASON_MINOR_NETWORKCARD = 0x00000000,
        SHTDN_REASON_MINOR_POWER_SUPPLY = 0x0000000a,
        SHTDN_REASON_MINOR_CORDUNPLUGGED = 0x0000000b,
        SHTDN_REASON_MINOR_ENVIRONMENT = 0x0000000c,
        SHTDN_REASON_MINOR_HARDWARE_DRIVER = 0x0000000d,
        SHTDN_REASON_MINOR_OTHERDRIVER = 0x0000000e,
        SHTDN_REASON_MINOR_BLUESCREEN = 0x0000000F,
        SHTDN_REASON_MINOR_SERVICEPACK = 0x00000010,
        SHTDN_REASON_MINOR_HOTFIX = 0x00000011,
        SHTDN_REASON_MINOR_SECURITYFIX = 0x00000012,
        SHTDN_REASON_MINOR_SECURITY = 0x00000013,
        SHTDN_REASON_MINOR_NETWORK_CONNECTIVITY = 0x00000014,
        SHTDN_REASON_MINOR_WMI = 0x00000015,
        SHTDN_REASON_MINOR_SERVICEPACK_UNINSTALL = 0x00000016,
        SHTDN_REASON_MINOR_HOTFIX_UNINSTALL = 0x00000017,
        SHTDN_REASON_MINOR_SECURITYFIX_UNINSTALL = 0x00000018,
        SHTDN_REASON_MINOR_MMC = 0x00000019,
        SHTDN_REASON_MINOR_TERMSRV = 0x00000020,

        SHTDN_REASON_FLAG_USER_DEFINED = 0x40000000,
        SHTDN_REASON_FLAG_PLANNED = 0x80000000,
        SHTDN_REASON_UNKNOWN = SHTDN_REASON_MINOR_NONE,
        SHTDN_REASON_LEGACY_API = (SHTDN_REASON_MAJOR_LEGACY_API | SHTDN_REASON_FLAG_PLANNED),

        SHTDN_REASON_VALID_BIT_MASK = 0xc0ffffff
    }

    internal class Interop
    {
        [DllImport("advapi32.dll", SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool LogonUser(
            [MarshalAs(UnmanagedType.LPStr)] string pszUserName,
            [MarshalAs(UnmanagedType.LPStr)] string pszDomain,
            [MarshalAs(UnmanagedType.LPStr)] string pszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken
        );

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool InitiateSystemShutdownEx(
            string lpMachineName,
            string lpMessage,
            uint dwTimeout,
            bool bForceAppsClosed,
            bool bRebootAfterShutdown,
            ShutdownReason dwReason
        );
    }
}
