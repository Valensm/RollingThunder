using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class NoDefaultVerbOrBagNotFoundException : ParsingException
    {
        public NoDefaultVerbOrBagNotFoundException()
        {
        }

        public NoDefaultVerbOrBagNotFoundException(string message) : base(message)
        {
        }

        public NoDefaultVerbOrBagNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NoDefaultVerbOrBagNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}