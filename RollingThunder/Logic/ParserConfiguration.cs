using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    public class ParserConfiguration
    {
        #region Properties

        public string ShortNamePrefix { get; set; }

        public string LongNamePrefix { get; set; }

        public string DefaultRequiredErrorMessage { get; set; }

        public bool ThrowHelpException { get; set; }

        #endregion Properties

        #region Ctors

        public ParserConfiguration()
        {
            this.ShortNamePrefix = "-";
            this.LongNamePrefix = "--";
            this.DefaultRequiredErrorMessage = "Please specify value for '{0}' ('{1}')";
            this.ThrowHelpException = false;
        }

        #endregion Ctors
    }
}