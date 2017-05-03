using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class UnassignableVerbsException : ParsingException
    {
        public IEnumerable<string> Verbs { get; }

        public UnassignableVerbsException(IEnumerable<string> verbs)
        {
            this.Verbs = verbs.ToArray();
        }

        public UnassignableVerbsException(IEnumerable<string> verbs, string message) : base(message)
        {
            this.Verbs = verbs.ToArray();
        }

        public UnassignableVerbsException(IEnumerable<string> verbs, string message, Exception inner) : base(message, inner)
        {
            this.Verbs = verbs.ToArray();
        }

        protected UnassignableVerbsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}