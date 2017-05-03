using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class UnassignableDefaultVerbException : UnassignableVerbException
    {
        public UnassignableDefaultVerbException(string verbName, IEnumerable<string> verbs) : base(verbName, verbs)
        {
        }

        public UnassignableDefaultVerbException(string verbName, IEnumerable<string> verbs, string message) : base(verbName, verbs, message)
        {
        }

        public UnassignableDefaultVerbException(string verbName, IEnumerable<string> verbs, string message, Exception inner) : base(verbName, verbs, message, inner)
        {
        }

        protected UnassignableDefaultVerbException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}