using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public abstract class ValidationException : ParsingException
    {
        protected ValidationException()
        {
        }

        protected ValidationException(string message) : base(message)
        {
        }

        protected ValidationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}