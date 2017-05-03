using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class HelpOptionAttribute : Attribute
    {
        public HelpOptionAttribute()
        {
        }
    }
}