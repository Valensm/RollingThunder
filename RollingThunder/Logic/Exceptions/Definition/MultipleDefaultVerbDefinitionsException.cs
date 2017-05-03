using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class MultipleDefaultVerbDefinitionsException : ParserDefinitionException
    {
        public IEnumerable<string> Verbs { get; }

        public MultipleDefaultVerbDefinitionsException(IEnumerable<string> verbs)
        {
            this.Verbs = verbs.ToArray();
        }

        public MultipleDefaultVerbDefinitionsException(IEnumerable<string> verbs, string message) : base(message)
        {
            this.Verbs = verbs.ToArray();
        }

        public MultipleDefaultVerbDefinitionsException(IEnumerable<string> verbs, string message, Exception inner) : base(message, inner)
        {
            this.Verbs = verbs.ToArray();
        }

        protected MultipleDefaultVerbDefinitionsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}