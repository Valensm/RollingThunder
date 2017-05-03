using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class InvalidVerbDefinitionException : ParserDefinitionException
    {
        public IEnumerable<string> Verbs { get; }

        public InvalidVerbDefinitionException(IEnumerable<string> verbs)
        {
            this.Verbs = verbs.ToArray();
        }

        public InvalidVerbDefinitionException(IEnumerable<string> verbs, string message) : base(message)
        {
            this.Verbs = verbs.ToArray();
        }

        public InvalidVerbDefinitionException(IEnumerable<string> verbs, string message, Exception inner) : base(message, inner)
        {
            this.Verbs = verbs.ToArray();
        }

        protected InvalidVerbDefinitionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}