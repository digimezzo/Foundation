using System;
using System.Reflection;

namespace Digimezzo.Foundation.Core.IO
{
    public static class ProcessExecutable
    {
        /// <summary>
        /// Gets the folder of the process executable
        /// </summary>
        /// <returns></returns>
        public static string ExecutionFolder()
        {
            return System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        /// <summary>
        /// Gets the name of the process executable
        /// </summary>
        /// <returns></returns>
        public static string Name()
        {
            return Assembly.GetEntryAssembly().GetName().Name;
        }

        /// <summary>
        /// Get the assembly version of the process executable
        /// </summary>
        /// <returns></returns>
        public static Version AssemblyVersion()
        {
            Assembly asm = Assembly.GetEntryAssembly();
            AssemblyName an = asm.GetName();

            return an.Version;
        }
    }
}
