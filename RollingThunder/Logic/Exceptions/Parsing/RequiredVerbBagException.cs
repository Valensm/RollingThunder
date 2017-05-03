using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class RequiredVerbBagException : ParsingException
    {
        public string VerbBagName { get; }

        public RequiredVerbBagException(string verbBagName)
        {
            this.VerbBagName = verbBagName;
        }

        public RequiredVerbBagException(string verbBagName, string message) : base(message)
        {
            this.VerbBagName = verbBagName;
        }

        public RequiredVerbBagException(string verbBagName, string message, Exception inner) : base(message, inner)
        {
            this.VerbBagName = verbBagName;
        }

        protected RequiredVerbBagException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}