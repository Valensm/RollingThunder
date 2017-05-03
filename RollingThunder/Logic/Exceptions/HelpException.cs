using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class HelpException : ParserException
    {
        public HelpException()
        {
        }

        public HelpException(string message) : base(message)
        {
        }

        public HelpException(string message, Exception inner) : base(message, inner)
        {
        }

        protected HelpException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}