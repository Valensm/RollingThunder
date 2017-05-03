using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class AmbiguousNameDefinitionException : ParserDefinitionException
    {
        public IEnumerable<string> Names { get; }

        public AmbiguousNameDefinitionException(IEnumerable<string> names)
        {
            this.Names = names.ToArray();
        }

        public AmbiguousNameDefinitionException(IEnumerable<string> names, string message) : base(message)
        {
            this.Names = names.ToArray();
        }

        public AmbiguousNameDefinitionException(IEnumerable<string> names, string message, Exception inner) : base(message, inner)
        {
            this.Names = names.ToArray();
        }

        protected AmbiguousNameDefinitionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}