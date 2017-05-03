using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class UnassignableBoolVerbException : UnassignableVerbException
    {
        public UnassignableBoolVerbException(string verbName, IEnumerable<string> verbs) : base(verbName, verbs)
        {
        }

        public UnassignableBoolVerbException(string verbName, IEnumerable<string> verbs, string message) : base(verbName, verbs, message)
        {
        }

        public UnassignableBoolVerbException(string verbName, IEnumerable<string> verbs, string message, Exception inner) : base(verbName, verbs, message, inner)
        {
        }

        protected UnassignableBoolVerbException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}