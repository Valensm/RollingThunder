using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wly.RollingThunder.Validators
{
    internal class ValidationResult
    {
        #region Fields

        #endregion Fields

        #region Properties

        public bool IsValid { get; }

        public string ErrorText { get; }

        #endregion Properties

        #region Ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        /// <param name="errorText">The error text.</param>
        public ValidationResult(bool isValid, string errorText)
        {
            this.IsValid = isValid;
            this.ErrorText = errorText ?? string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// Represents invalid result.
        /// </summary>
        /// <param name="errorText">The error text.</param>
        public ValidationResult(string errorText)
            : this(false, errorText)

        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// Represents valid result.
        /// </summary>
        public ValidationResult()
            : this(true, string.Empty)
        {
        }

        #endregion Ctors

        #region Private Methods

        #endregion Private Methods

        #region Public Methods

        #endregion Public Methods
    }
}