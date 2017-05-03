using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class CannotSatisfyRequiredDefaultVerbAndBagException : ParsingException
    {
        public string Verb { get; }

        public string DefaultVerbName { get; }

        public string VerbBagName { get; }

        public CannotSatisfyRequiredDefaultVerbAndBagException(string verb, string defaultVerbName, string verbBagName)
        {
            this.Verb = verb;
            this.DefaultVerbName = defaultVerbName;
            this.VerbBagName = verbBagName;
        }

        public CannotSatisfyRequiredDefaultVerbAndBagException(string verb, string defaultVerbName, string verbBagName, string message) : base(message)
        {
            this.Verb = verb;
            this.DefaultVerbName = defaultVerbName;
            this.VerbBagName = verbBagName;
        }

        public CannotSatisfyRequiredDefaultVerbAndBagException(string verb, string defaultVerbName, string verbBagName, string message, Exception inner) : base(message, inner)
        {
            this.Verb = verb;
            this.DefaultVerbName = defaultVerbName;
            this.VerbBagName = verbBagName;
        }

        protected CannotSatisfyRequiredDefaultVerbAndBagException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}