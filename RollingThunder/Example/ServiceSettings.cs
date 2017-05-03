using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wly.RollingThunder;

namespace Example
{
    public class ServiceSettings
    {
        #region Properties

        [DefaultVerb]
        [Required]
        [Name("u", "serviceurl")]
        [HelpText("Url of the service.")]
        public string Url { get; set; }

        [Name("p", "proxy")]
        [HelpText("Allows using proxy.")]
        public ProxySettings ProxySettings { get; set; }

        [Name("?")]
        [HelpOption]
        public bool Help { get; set; }

        [Name("h", "help")]
        [HelpOption]
        public bool Help2 { get; set; }

        #endregion Properties
    }
}