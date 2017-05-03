using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class InvalidVerbBagDefinitionException : ParserDefinitionException
    {
        public IEnumerable<string> Bags { get; }

        public InvalidVerbBagDefinitionException(IEnumerable<string> bags)
        {
            this.Bags = bags.ToArray();
        }

        public InvalidVerbBagDefinitionException(IEnumerable<string> bags, string message) : base(message)
        {
            this.Bags = bags.ToArray();
        }

        public InvalidVerbBagDefinitionException(IEnumerable<string> bags, string message, Exception inner) : base(message, inner)
        {
            this.Bags = bags.ToArray();
        }

        protected InvalidVerbBagDefinitionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}