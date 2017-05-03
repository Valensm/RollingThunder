using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    internal class ArgumentGroup : ValueGroup
    {
        #region Properties

        public IReadOnlyCollection<string> Arguments => base.Values;

        public bool HasParameters => base.HasValues;

        #endregion Properties

        #region Ctors

        public ArgumentGroup(string name, IEnumerable<string> arguments = null)
            : base(name, arguments)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Cannot be empty.", nameof(name));
            }
        }

        #endregion Ctors

        #region Private Methods

        #endregion Private Methods

        #region Public Methods

        public void AddArgument(string value)
            => base.AddValue(value);

        public void RemoveArgument(string value)
            => base.RemoveValue(value);

        #endregion Public Methods
    }
}