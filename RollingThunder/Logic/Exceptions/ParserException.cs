using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public abstract class ParserException : Exception
    {
        protected ParserException()
        {
        }

        protected ParserException(string message) : base(message)
        {
        }

        protected ParserException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ParserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}