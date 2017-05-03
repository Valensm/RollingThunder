using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class VerbBagAttribute : Attribute
    {
        public VerbBagAttribute()
        {
        }
    }
}