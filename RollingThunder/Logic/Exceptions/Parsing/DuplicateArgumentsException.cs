using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class DuplicateArgumentsException : ParsingException
    {
        public IEnumerable<string> Names { get; }

        public DuplicateArgumentsException(IEnumerable<string> names)
        {
            this.Names = names.ToArray();
        }

        public DuplicateArgumentsException(IEnumerable<string> names, string message) : base(message)
        {
            this.Names = names.ToArray();
        }

        public DuplicateArgumentsException(IEnumerable<string> names, string message, Exception inner) : base(message, inner)
        {
            this.Names = names.ToArray();
        }

        protected DuplicateArgumentsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}