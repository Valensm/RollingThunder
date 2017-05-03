using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class RequiredAttribute : Attribute
    {
        public string ErrorText { get; }

        public RequiredAttribute(string errorText)
        {
            this.ErrorText = errorText ?? string.Empty;
        }

        public RequiredAttribute()
            : this(null)
        {
        }
    }
}