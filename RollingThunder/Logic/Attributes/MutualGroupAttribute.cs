using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public sealed class MutualGroupAttribute : Attribute
    {
        public string MutualGroupName { get; }

        public MutualGroupAttribute(string mutualGroupName)
        {
            if (mutualGroupName == null)
            {
                throw new ArgumentNullException(nameof(mutualGroupName));
            }
            if (String.IsNullOrEmpty(mutualGroupName))
            {
                throw new ArgumentException("Cannot be empty.", nameof(mutualGroupName));
            }
            this.MutualGroupName = mutualGroupName;
        }
    }
}