using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class RequiredDefaultVerbException : ParsingException
    {
        public string DefaultVerbName { get; }

        public RequiredDefaultVerbException(string defaultVerbName)
        {
            this.DefaultVerbName = defaultVerbName;
        }

        public RequiredDefaultVerbException(string defaultVerbName, string message) : base(message)
        {
            this.DefaultVerbName = defaultVerbName;
        }

        public RequiredDefaultVerbException(string defaultVerbName, string message, Exception inner) : base(message, inner)
        {
            this.DefaultVerbName = defaultVerbName;
        }

        protected RequiredDefaultVerbException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}