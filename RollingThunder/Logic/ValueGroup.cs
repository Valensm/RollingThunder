using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    internal abstract class ValueGroup
    {
        #region Fields

        private List<string> values;

        #endregion Fields

        #region Properties

        public IReadOnlyCollection<string> Values => this.values;

        public string Name { get; }

        public bool HasName => !string.IsNullOrWhiteSpace(this.Name);

        public bool HasValues => this.values.Count > 0;

        public bool IsEmpty => !this.HasValues && !this.HasName;

        public bool IsVerbOnlyGroup => string.IsNullOrWhiteSpace(this.Name);

        #endregion Properties

        #region Ctors

        public ValueGroup(string name = null, IEnumerable<string> values = null)
        {
            this.Name = name ?? string.Empty;
            this.values = values == null ? new List<string>() : values.ToList();
        }

        #endregion Ctors

        #region Private Methods

        #endregion Private Methods

        #region Public Methods

        public void AddValue(string value)
        {
            this.values.Add(value);
        }

        public void RemoveValue(string value)
        {
            this.values.Remove(value);
        }

        #endregion Public Methods
    }
}