using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    internal class HelpsScreen<T> : IHelpScreen
    {
        #region Properties

        public IEnumerable<string> Lines { get; }

        public string Error { get; }

        #endregion Properties

        #region Ctors

        public HelpsScreen(Func<T> instanceFactory, string error, HelpConfiguration helpConfiguration, ParserConfiguration configuration)
        {
            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }
            if (helpConfiguration == null)
            {
                throw new ArgumentNullException(nameof(helpConfiguration));
            }
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.Error = error;
            HelpData helpData = GetHelpData(instanceFactory, helpConfiguration, error, configuration);
            this.Lines = GetHelpScreenLines(helpData);
        }

        public HelpsScreen(Func<T> instanceFactory, string error, HelpConfiguration helpConfiguration) : this(instanceFactory, error, helpConfiguration, new ParserConfiguration())
        {
        }

        public HelpsScreen(Func<T> instanceFactory, string error) : this(instanceFactory, error, new HelpConfiguration(), new ParserConfiguration())
        {
        }

        public HelpsScreen(Func<T> instanceFactory) : this(instanceFactory, string.Empty, new HelpConfiguration(), new ParserConfiguration())
        {
        }

        #endregion Ctors

        #region Private Methods

        #region Command Line

        internal static string GetCommandLineHelpLine(HelpData data, HelpConfiguration helpConfiguration, ParserConfiguration configuration)
        {
            //Verbs - first verbs one by one, then bag expanded to a list '[<a1> <a2> <a3> ...]'
            //Parameters
            string requiredVerbPart = GetRequiredVerbsPart(data.Descriptors);
            string optionalVerbPart = GetOptionalVerbsPart(data.Descriptors);
            string requiredParametersPart = GetRequiredParametersPart(data.Descriptors, configuration);
            string optionalParametersPart = GetOptionalParametersPart(data.Descriptors, configuration);

            string[] values = new string[] { helpConfiguration.ApplicationExecutable, requiredVerbPart, optionalVerbPart, requiredParametersPart, optionalParametersPart };
            return string.Join(" ", values.Where(v => !string.IsNullOrWhiteSpace(v)));
        }

        private static string GetRequiredVerbsPart(IEnumerable<IHelpDescriptor> descriptors)
        {
            var verbs = descriptors
                .Where(d => (d.IsVerb || d.IsVerbBag || d.IsDefaultVerb) && d.IsRequired)
                .OrderBy(d => d.PropertyName)
                .Select(d => FormatVerbHelp(d))
                .ToArray();

            return string.Join(" ", verbs);
        }

        private static string GetOptionalVerbsPart(IEnumerable<IHelpDescriptor> descriptors)
        {
            var verbs = descriptors
               .Where(d => (d.IsVerb || d.IsVerbBag || d.IsDefaultVerb) && !d.IsRequired)
               .OrderBy(d => d.PropertyName)
               .Select(d => FormatVerbHelp(d))
               .ToArray();
            return string.Join(" ", verbs);
        }

        private static string FormatVerbHelp(IHelpDescriptor descriptor)
        {
            string prefix = descriptor.IsRequired ? string.Empty : "[";
            string suffix = descriptor.IsRequired ? string.Empty : "]";
            string result = string.Empty;
            if (descriptor.Type.IsEnum)
            {
                var values = descriptor.Type.GetEnumValues().Cast<object>();
                string valuesString = string.Join("|", values.Select(v => v.ToString()));
                result = $"{prefix}{{{valuesString}}}{suffix}";
            }
            else if (descriptor.IsCollectionType)
            {
                result = $"{prefix}<{descriptor.PropertyName} 1> <{descriptor.PropertyName} 2> <{descriptor.PropertyName} 3> ...{suffix}";
            }
            else
            {
                result = $"{prefix}<{descriptor.PropertyName}>{suffix}";
            }
            return result;
        }

        private static string GetRequiredParametersPart(IEnumerable<IHelpDescriptor> descriptors, ParserConfiguration configuration)
        {
            var parameters = descriptors
                .Where(d => !d.IsVerb && !d.IsVerbBag && !d.IsDefaultVerb && d.IsRequired)
                .OrderBy(d => d.ShortName)
                .OrderBy(d => d.IsHelp)
                .Select(d => FormatParameterHelp(d, configuration))
                .ToArray();

            return string.Join(" ", parameters);
        }

        private static string GetOptionalParametersPart(IEnumerable<IHelpDescriptor> descriptors, ParserConfiguration configuration)
        {
            var parameters = descriptors
                 .Where(d => !d.IsVerb && !d.IsVerbBag && !d.IsDefaultVerb && !d.IsRequired)
                 .OrderBy(d => d.ShortName)
                 .OrderBy(d => d.IsHelp)
                 .Select(d => FormatParameterHelp(d, configuration))
                 .ToArray();

            return string.Join(" ", parameters);
        }

        private static string FormatParameterHelp(IHelpDescriptor descriptor, ParserConfiguration configuration)
        {
            string prefix = descriptor.IsRequired ? string.Empty : "[";
            string suffix = descriptor.IsRequired ? string.Empty : "]";
            string parameterName = $"{configuration.ShortNamePrefix}{descriptor.ShortName}|{configuration.LongNamePrefix}{descriptor.LongName}";

            string result = string.Empty;
            if (descriptor.Descriptors.Any())
            {
                string requiredPart = GetRequiredParametersPart(descriptor.Descriptors, configuration);
                string optionalPart = GetOptionalParametersPart(descriptor.Descriptors, configuration);
                string[] parts = new string[] { requiredPart, optionalPart };
                return $"{prefix}{parameterName} ({string.Join(" ", parts.Where(a => !string.IsNullOrWhiteSpace(a)))}){suffix}";
            }
            else if (descriptor.Type.IsEnum)
            {
                var values = descriptor.Type.GetEnumValues().Cast<object>();
                string valuesString = string.Join("|", values.Select(v => v.ToString()));
                result = $"{prefix}{parameterName} {{{valuesString}}}{suffix}";
            }
            else if (descriptor.CanHaveValue)
            {
                result = $"{prefix}{parameterName} <{descriptor.PropertyName}>{suffix}";
            }
            else if (descriptor.IsCollectionType)
            {
                result = $"{prefix}{parameterName} <{descriptor.PropertyName} 1> <{descriptor.PropertyName} 2> <{descriptor.PropertyName} 3> ...{suffix}";
            }
            else
            {
                result = $"{prefix}{parameterName}{suffix}";
            }

            return result;
        }

        private static IEnumerable<IHelpDescriptor> Flatten(IEnumerable<IHelpDescriptor> descriptors)
        {
            return descriptors.Concat(descriptors.SelectMany(d => Flatten(d.Descriptors)));
        }

        #endregion Command Line

        private static IEnumerable<string> GetHelpScreenParametersHelpLines(HelpData data, HelpConfiguration helpConfiguration, ParserConfiguration configuration)
        {
            IHelpDescriptor[] descriptors = Flatten(data.Descriptors)
                .Where(d => !d.IsVerb && !d.IsVerbBag && !d.IsDefaultVerb)
                .OrderBy(d => d.ShortName)
                .ToArray();
            var lines = descriptors
                .Select(d => FormatParameterHelpDescriptor(d, configuration))
                //.Select(a => $"{a.Item2} - {a.Item2}")
                .ToArray();

            int indentCharsCount = GetIndentCharsCount(lines.Select(l => l.Item1), helpConfiguration.Indent.Length);

            return lines.Select(l => $"{l.Item1.PadRight(indentCharsCount, ' ')} {l.Item2}").ToArray();
        }

        private static int GetIndentCharsCount(IEnumerable<string> lines, int indentLength)
        {
            int maxLength = lines.Max(a => a.Length);
            return Convert.ToInt32(Math.Ceiling(maxLength / (double)indentLength)) * indentLength;
        }

        private static Tuple<string, string> FormatParameterHelpDescriptor(IHelpDescriptor descriptor, ParserConfiguration configuration)
        {
            List<string> parts = new List<string>();
            string leftPart = $"{configuration.ShortNamePrefix}{descriptor.ShortName}|{configuration.LongNamePrefix}{descriptor.LongName}";

            parts.Add("-");
            parts.Add(descriptor.HelpText);

            if (descriptor.IsHelp && string.IsNullOrWhiteSpace(descriptor.HelpText))
            {
                parts.Add($"This help screen.");
            }
            if (!descriptor.IsHelp)
            {
                if (descriptor.Type.IsEnum)
                {
                    string[] values = descriptor.Type.GetEnumValues().Cast<object>().Select(a => a.ToString()).ToArray();
                    string valuesString = string.Join(", ", values);
                    parts.Add($"Possible values: {valuesString}.");
                }
                if (descriptor.IsBooleanType)
                {
                    parts.Add($"Switch, no value required.");
                }
            }
            if (descriptor.Descriptors.Any())
            {
                string innerDescriptorsString = string.Join(", ", descriptor.Descriptors.Select(d => $"{configuration.ShortNamePrefix}{d.ShortName}|{configuration.LongNamePrefix}{d.LongName}"));
                parts.Add($"Enables/unlocks use of following parameters: {innerDescriptorsString}.");
            }
            if (descriptor.IsCollectionType)
            {
                int minimum = descriptor.IsRequired ? Math.Max(1, descriptor.MinValueCount) : descriptor.MinValueCount;
                parts.Add($"Collection which accepts at least {minimum} and at maximum {descriptor.MaxValueCount} values.");
            }
            if (descriptor.MutualGroups.Any())
            {
                string groupsString = string.Join(", ", descriptor.MutualGroups);
                parts.Add($"Mutual groups: {groupsString}.");
            }

            if (descriptor.IsRequired)
            {
                parts.Add("Required.");
            }
            else
            {
                parts.Add("Optional.");
            }
            return new Tuple<string, string>(leftPart, string.Join(" ", parts.Where(a => !string.IsNullOrWhiteSpace(a))));
        }

        internal static HelpData GetHelpData(Func<T> instanceFactory, HelpConfiguration helpConfiguration, string error, ParserConfiguration configuration)
        {
            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }
            T instance = instanceFactory();

            IEnumerable<Descriptor> descriptors = Descriptor.AllFromInstance<T>(instance);
            return new HelpData(descriptors.Where(d => !d.IsIgnored).Select(d => new HelpDescriptor(d)).ToArray(), error, helpConfiguration, configuration);
        }

        internal static IEnumerable<string> GetHelpScreenLines(HelpData data)
        {
            List<string> lines = new List<string>();

            if (!string.IsNullOrEmpty(data.Error))
            {
                if (!string.IsNullOrEmpty(data.HelpConfiguration.ErrorHeader))
                {
                    lines.Add(data.HelpConfiguration.ErrorHeader);
                }

                lines.Add($"{data.HelpConfiguration.Indent}{data.Error}");
                lines.Add(string.Empty);
            }

            if (!string.IsNullOrEmpty(data.HelpConfiguration.ApplicationName))
            {
                lines.Add($"{data.HelpConfiguration.ApplicationName} {data.HelpConfiguration.ApplicationVersion} {data.HelpConfiguration.ApplicationCopyright}");
                lines.Add(string.Empty);
            }

            if (!string.IsNullOrEmpty(data.HelpConfiguration.Description))
            {
                if (!string.IsNullOrEmpty(data.HelpConfiguration.DescriptionHeader))
                {
                    lines.Add(data.HelpConfiguration.DescriptionHeader);
                }

                lines.Add($"{data.HelpConfiguration.Indent}{data.HelpConfiguration.Description}");
                lines.Add(string.Empty);
            }

            if (!string.IsNullOrEmpty(data.HelpConfiguration.UsageHeader))
            {
                lines.Add(data.HelpConfiguration.UsageHeader);
            }

            string commandLine = GetCommandLineHelpLine(data, data.HelpConfiguration, data.Configuration);
            lines.Add($"{data.HelpConfiguration.Indent}{commandLine}");

            lines.Add(string.Empty);

            if (!string.IsNullOrEmpty(data.HelpConfiguration.ParametersHeader))
            {
                lines.Add(data.HelpConfiguration.ParametersHeader);
            }

            lines.AddRange(GetHelpScreenParametersHelpLines(data, data.HelpConfiguration, data.Configuration).Select(line => $"{data.HelpConfiguration.Indent}{line}"));

            return lines;
        }

        #endregion Private Methods
    }
}