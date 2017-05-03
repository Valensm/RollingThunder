using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class NotEnoughArgumentValuesException : ParsingException
    {
        public string ParameterName { get; }

        public IEnumerable<string> Values { get; }

        public int MinCount { get; }

        public int CurrentCount { get; }

        public NotEnoughArgumentValuesException(string parameterName, int minCount, int currentCount, IEnumerable<string> values)
        {
            this.ParameterName = parameterName;
            this.Values = values.ToArray();
            this.MinCount = minCount;
            this.CurrentCount = currentCount;
        }

        public NotEnoughArgumentValuesException(string parameterName, int minCount, int currentCount, IEnumerable<string> values, string message) : base(message)
        {
            this.ParameterName = parameterName;
            this.Values = values.ToArray();
            this.MinCount = minCount;
            this.CurrentCount = currentCount;
        }

        public NotEnoughArgumentValuesException(string parameterName, int minCount, int currentCount, IEnumerable<string> values, string message, Exception inner) : base(message, inner)
        {
            this.ParameterName = parameterName;
            this.Values = values.ToArray();
            this.MinCount = minCount;
            this.CurrentCount = currentCount;
        }

        protected NotEnoughArgumentValuesException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}