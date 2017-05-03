using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class ArgumentNotSupportedException : ParsingException
    {
        public string ArgumentName { get; }

        public ArgumentNotSupportedException(string argumentName)
        {
            this.ArgumentName = argumentName;
        }

        public ArgumentNotSupportedException(string argumentName, string message) : base(message)
        {
            this.ArgumentName = argumentName;
        }

        public ArgumentNotSupportedException(string argumentName, string message, Exception inner) : base(message, inner)
        {
            this.ArgumentName = argumentName;
        }

        protected ArgumentNotSupportedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}