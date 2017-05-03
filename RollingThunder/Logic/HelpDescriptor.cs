using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    internal class HelpDescriptor : IHelpDescriptor
    {
        #region Fields

        private readonly Descriptor descriptor;

        #endregion Fields

        #region Properties

        public string HelpText => this.descriptor.Helptext;

        public bool IsVerb => this.descriptor.IsVerb;

        public bool IsVerbBag => this.descriptor.IsVerbBag;

        public bool IsDefaultVerb => this.descriptor.IsDefaultVerb;

        public string ShortName => this.descriptor.ShortName;

        public string LongName => this.descriptor.LongName;

        public Type Type => this.descriptor.Type;

        public bool IsRequired => this.descriptor.IsRequired;

        public bool IsCollectionType => this.descriptor.IsCollectionType;

        public bool IsBooleanType => this.descriptor.IsBool;

        public bool CanHaveValue => this.descriptor.CanHaveValue;

        public int MaxValueCount => this.descriptor.MaxValuesCount;

        public int MinValueCount => this.descriptor.MinValuesCount;

        public bool IsSimpleType => this.descriptor.IsSimpleType;

        public bool IsHelp => this.descriptor.IsHelp;

        public IEnumerable<string> MutualGroups => this.descriptor.MutualGroups;

        public IEnumerable<IHelpDescriptor> Descriptors { get; }

        public string PropertyName => this.descriptor.PropertyName;

        #endregion Properties

        #region Ctors

        internal HelpDescriptor(Descriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }
            this.descriptor = descriptor;
            this.Descriptors = this.descriptor.Descriptors.Select(d => new HelpDescriptor(d)).ToArray();
        }

        #endregion Ctors

        #region Private Methods

        #endregion Private Methods

        #region Public Methods

        #endregion Public Methods
    }
}