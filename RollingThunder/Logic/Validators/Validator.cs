using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wly.RollingThunder.Validators
{
    internal abstract class Validator
    {
        #region Fields

        #endregion Fields

        #region Properties

        public Type Type { get; }

        public string ErrorText { get; }

        #endregion Properties

        #region Ctors

        public Validator(Type type, string errorText)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            this.Type = type;
            this.ErrorText = errorText ?? string.Empty;
        }

        #endregion Ctors

        #region Private Methods

        #endregion Private Methods

        #region Public Methods

        public ValidationResult Validate(object value, string name)
        {
            return new Validators.ValidationResult(
                this.ValidateValue(value),
                string.Format(this.ErrorText ?? string.Empty, name, value ?? string.Empty)
                );
        }

        protected abstract bool ValidateValue(object value);

        #endregion Public Methods
    }
}