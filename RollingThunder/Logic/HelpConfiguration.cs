using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    public class HelpConfiguration
    {
        #region Properties

        public string ApplicationName { get; set; }

        public string ApplicationExecutable { get; set; }

        public string ApplicationVersion { get; set; }

        public string ApplicationCopyright { get; set; }

        public string ErrorHeader { get; set; }

        public string UsageHeader { get; set; }

        public string DescriptionHeader { get; set; }

        public string ParametersHeader { get; set; }

        public string Description { get; set; }

        public string Indent { get; set; }

        #endregion Properties

        #region Ctors

        public HelpConfiguration()
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly() ?? Assembly.GetExecutingAssembly();
            string assemblyName = assembly.GetName().Name;
            Version version = assembly.GetName().Version;

            this.ApplicationExecutable = string.IsNullOrEmpty(assemblyName) ? string.Empty : $"{assemblyName}.exe";
            this.ApplicationName = AttributeValue<AssemblyTitleAttribute, string>(assembly, t => t.Title) ?? String.Empty;
            this.ApplicationVersion = version?.ToString() ?? String.Empty;
            this.ErrorHeader = "Error:";
            this.UsageHeader = "Usage:";
            this.DescriptionHeader = "Description:";
            this.ParametersHeader = "Parameters:";
            this.Description = AttributeValue<AssemblyDescriptionAttribute, string>(assembly, t => t.Description) ?? String.Empty;
            this.ApplicationCopyright = AttributeValue<AssemblyCopyrightAttribute, string>(assembly, t => t.Copyright) ?? String.Empty;
            this.Indent = "    ";
        }

        #endregion Ctors

        #region Private Methods

        private static TResult AttributeValue<TAttribute, TResult>(System.Reflection.Assembly assembly, Func<TAttribute, TResult> selector)
        {
            return assembly.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().Select(selector).FirstOrDefault();
        }

        #endregion Private Methods
    }
}