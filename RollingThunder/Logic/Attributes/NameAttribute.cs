using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class NameAttribute : Attribute
    {
        public string ShortName { get; }

        public string LongName { get; }

        public string PropertyName { get; }

        public NameAttribute(string shortName)
            : this(shortName, null, null)
        {
        }

        public NameAttribute(string shortName, string longName)
            : this(shortName, longName, null)
        {
        }

        public NameAttribute(string shortName, string longName, string propertyName)
        {
            if (shortName == null)
            {
                throw new ArgumentNullException(nameof(shortName));
            }
            if (String.IsNullOrEmpty(shortName))
            {
                throw new ArgumentException("Cannot be empty.", nameof(shortName));
            }
            this.ShortName = shortName;
            this.LongName = string.IsNullOrEmpty(longName) ? shortName : longName;
            this.PropertyName = propertyName ?? string.Empty;
        }
    }
}