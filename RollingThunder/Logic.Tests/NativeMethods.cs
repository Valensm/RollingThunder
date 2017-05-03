using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    internal static class NativeMethods
    {
        #region Private Methods

        [DllImport("shell32.dll", SetLastError = true)]
        private static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Splits commands line to arguments.
        /// This is a wrapper around WinApi function.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        /// <returns></returns>
        public static string[] CommandLineToArgs(string commandLine)
        {
            int argc;
            var argv = CommandLineToArgvW(commandLine, out argc);
            if (argv == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception();
            try
            {
                var args = new string[argc];
                for (var i = 0; i < args.Length; i++)
                {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p);
                }

                return args;
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }
        }

        #endregion Public Methods
    }
}