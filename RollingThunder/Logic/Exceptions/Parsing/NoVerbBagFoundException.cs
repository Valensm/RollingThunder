using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class NoVerbBagFoundException : ParsingException
    {
        public NoVerbBagFoundException()
        {
        }

        public NoVerbBagFoundException(string message) : base(message)
        {
        }

        public NoVerbBagFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NoVerbBagFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}