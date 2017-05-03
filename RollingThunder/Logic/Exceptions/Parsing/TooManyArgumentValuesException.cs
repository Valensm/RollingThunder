using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [Serializable]
    public class TooManyArgumentValuesException : ParsingException
    {
        public string ParameterName { get; }

        public IEnumerable<string> Values { get; }

        public int MaxCount { get; }

        public int CurrentCount { get; }

        public TooManyArgumentValuesException(string parameterName, int maxCount, int currentCount, IEnumerable<string> values)
        {
            this.ParameterName = parameterName;
            this.Values = values.ToArray();
            this.MaxCount = maxCount;
            this.CurrentCount = currentCount;
        }

        public TooManyArgumentValuesException(string parameterName, int maxCount, int currentCount, IEnumerable<string> values, string message) : base(message)
        {
            this.ParameterName = parameterName;
            this.Values = values.ToArray();
            this.MaxCount = maxCount;
            this.CurrentCount = currentCount;
        }

        public TooManyArgumentValuesException(string parameterName, int maxCount, int currentCount, IEnumerable<string> values, string message, Exception inner) : base(message, inner)
        {
            this.ParameterName = parameterName;
            this.Values = values.ToArray();
            this.MaxCount = maxCount;
            this.CurrentCount = currentCount;
        }

        protected TooManyArgumentValuesException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}