using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class AmbiguousVerbException : ParsingException
    {
        public string Verb { get; }

        public AmbiguousVerbException(string verb)
        {
            this.Verb = verb;
        }

        public AmbiguousVerbException(string verb, string message) : base(message)
        {
            this.Verb = verb;
        }

        public AmbiguousVerbException(string verb, string message, Exception inner) : base(message, inner)
        {
            this.Verb = verb;
        }

        protected AmbiguousVerbException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}