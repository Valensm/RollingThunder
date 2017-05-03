using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wly.RollingThunder;

namespace Example
{
    public class ProxySettings
    {
        #region Properties

        [Name("url", "proxyurl")]
        [Required]
        [HelpText("Proxy url.")]
        public string ProxyUrl { get; set; }

        [Name("port", "proxyport")]
        [Required]
        [HelpText("Proxy port.")]
        public int ProxyPort { get; set; }

        [Name("user", "proxyuser")]
        [HelpText("Proxy user name.")]
        public string ProxyUser { get; set; }

        [Name("pwd", "proxypassword")]
        [HelpText("Proxy password.")]
        public string ProxyPassword { get; set; }

        #endregion Properties
    }
}