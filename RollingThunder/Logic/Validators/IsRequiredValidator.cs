using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder.Validators
{
    internal class IsRequiredValidator : Validator
    {
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Ctors

        public IsRequiredValidator(Type type, string errorText)
            : base(type, errorText)
        {
        }

        #endregion Ctors

        #region Private Methods

        #endregion Private Methods

        #region Public Methods

        protected override bool ValidateValue(object value)
        {
            if (value == null)
            {
                return false;
            }
            else if (value.GetType() == typeof(string))
            {
                return !string.IsNullOrEmpty(value as string);
            }
            else
            {
                return true;
            }
        }

        #endregion Public Methods
    }
}