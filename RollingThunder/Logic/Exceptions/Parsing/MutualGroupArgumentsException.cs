using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class MutualGroupArgumentsException : ParsingException
    {
        public string GroupName { get; }

        public IEnumerable<string> Arguments { get; }

        public MutualGroupArgumentsException(string groupName, IEnumerable<string> arguments)
        {
            this.GroupName = groupName;
            this.Arguments = arguments.ToArray();
        }

        public MutualGroupArgumentsException(string groupName, IEnumerable<string> arguments, string message) : base(message)
        {
            this.GroupName = groupName;
            this.Arguments = arguments.ToArray();
        }

        public MutualGroupArgumentsException(string groupName, IEnumerable<string> arguments, string message, Exception inner) : base(message, inner)
        {
            this.GroupName = groupName;
            this.Arguments = arguments.ToArray();
        }

        protected MutualGroupArgumentsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}