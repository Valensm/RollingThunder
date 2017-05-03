using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    internal class HelpData
    {
        #region Properties

        public string Error { get; }

        public IReadOnlyCollection<IHelpDescriptor> Descriptors { get; }

        public HelpConfiguration HelpConfiguration { get; }

        public ParserConfiguration Configuration { get; }

        #endregion Properties

        #region Ctors

        public HelpData(IEnumerable<IHelpDescriptor> descriptors, string error, HelpConfiguration helpConfiguration, ParserConfiguration configuration)
        {
            if (descriptors == null)
            {
                throw new ArgumentNullException(nameof(descriptors));
            }

            if (helpConfiguration == null)
            {
                throw new ArgumentNullException(nameof(helpConfiguration));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.Descriptors = descriptors.ToArray();
            this.Error = error ?? string.Empty;
            this.Configuration = configuration;
            this.HelpConfiguration = helpConfiguration;
        }

        #endregion Ctors
    }
}