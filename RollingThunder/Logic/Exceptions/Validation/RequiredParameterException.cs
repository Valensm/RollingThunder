using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class RequiredParameterException : ValidationException
    {
        public string ParameterName { get; }

        public RequiredParameterException(string parameterName)
        {
            this.ParameterName = parameterName;
        }

        public RequiredParameterException(string parameterName, string message) : base(message)
        {
            this.ParameterName = parameterName;
        }

        public RequiredParameterException(string parameterName, string message, Exception inner) : base(message, inner)
        {
            this.ParameterName = parameterName;
        }

        protected RequiredParameterException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}