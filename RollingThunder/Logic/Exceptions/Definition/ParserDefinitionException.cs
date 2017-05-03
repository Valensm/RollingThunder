using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public abstract class ParserDefinitionException : ParserException
    {
        protected ParserDefinitionException()
        {
        }

        protected ParserDefinitionException(string message) : base(message)
        {
        }

        protected ParserDefinitionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ParserDefinitionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}