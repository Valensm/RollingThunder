using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class UnassignableParameterException : ParsingException
    {
        public string ParameterName { get; }

        public string Value { get; set; }

        public UnassignableParameterException(string parameterName, string value)
        {
            this.ParameterName = parameterName;
            this.Value = value;
        }

        public UnassignableParameterException(string parameterName, string value, string message) : base(message)
        {
            this.ParameterName = parameterName;
            this.Value = value;
        }

        public UnassignableParameterException(string parameterName, string value, string message, Exception inner) : base(message, inner)
        {
            this.ParameterName = parameterName;
            this.Value = value;
        }

        protected UnassignableParameterException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}