using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    internal static class Splitter
    {
        #region Public Methods

        /// <summary>
        /// Splits command line to arguments.
        /// </summary>
        /// <param name="commandLine">The command line without an application name.</param>
        /// <returns>Arguments array.</returns>
        public static string[] ToArgs(this string commandLine)
        {
            return NativeMethods.CommandLineToArgs($"foo.exe {commandLine}").Skip(1).ToArray();
        }

        #endregion Public Methods
    }
}