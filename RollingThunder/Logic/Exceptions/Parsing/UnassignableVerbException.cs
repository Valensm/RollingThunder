using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class UnassignableVerbException : ParsingException
    {
        public string VerbName { get; }

        public IEnumerable<string> Verbs { get; }

        public UnassignableVerbException(string verbName, IEnumerable<string> verbs)
        {
            this.VerbName = verbName;
            this.Verbs = verbs.ToArray();
        }

        public UnassignableVerbException(string verbName, IEnumerable<string> verbs, string message) : base(message)
        {
            this.VerbName = verbName;
            this.Verbs = verbs.ToArray();
        }

        public UnassignableVerbException(string verbName, IEnumerable<string> verbs, string message, Exception inner) : base(message, inner)
        {
            this.VerbName = verbName;
            this.Verbs = verbs.ToArray();
        }

        protected UnassignableVerbException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}