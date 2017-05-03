using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class AmbiguousParameterException : ParsingException
    {
        public string ParameterName { get; }

        public AmbiguousParameterException(string parameterName)
        {
            this.ParameterName = parameterName;
        }

        public AmbiguousParameterException(string parameterName, string message) : base(message)
        {
            this.ParameterName = parameterName;
        }

        public AmbiguousParameterException(string parameterName, string message, Exception inner) : base(message, inner)
        {
            this.ParameterName = parameterName;
        }

        protected AmbiguousParameterException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}