using System;
using System.IO;
using System.Reflection;

namespace Krueger.Modules
{
    internal class Policy
    {
        public static byte[] ReadPolicy()
        {
            string resourceName = "Krueger.SiPolicy.p7b";

            byte[] fileBytes = GetEmbeddedResourceBytes(resourceName);

            return fileBytes;
        }

        static byte[] GetEmbeddedResourceBytes(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"Resource '{resourceName}' not found.");
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
