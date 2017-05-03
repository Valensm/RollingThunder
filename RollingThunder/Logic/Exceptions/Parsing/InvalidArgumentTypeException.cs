using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class InvalidArgumentTypeException : ParsingException
    {
        public string ArgumentName { get; }

        public IEnumerable<string> PossibleValues { get; }

        public InvalidArgumentTypeException(string argumentName, IEnumerable<string> possibleValues)
        {
            this.ArgumentName = argumentName;
            this.PossibleValues = possibleValues.ToArray();
        }

        public InvalidArgumentTypeException(string argumentName, IEnumerable<string> possibleValues, string message) : base(message)
        {
            this.ArgumentName = argumentName;
            this.PossibleValues = possibleValues.ToArray();
        }

        public InvalidArgumentTypeException(string argumentName, IEnumerable<string> possibleValues, string message, Exception inner) : base(message, inner)
        {
            this.ArgumentName = argumentName;
            this.PossibleValues = possibleValues.ToArray();
        }

        protected InvalidArgumentTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}