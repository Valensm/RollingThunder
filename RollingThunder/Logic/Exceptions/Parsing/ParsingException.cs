using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public abstract class ParsingException : ParserException
    {
        protected ParsingException()
        {
        }

        protected ParsingException(string message) : base(message)
        {
        }

        protected ParsingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ParsingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}