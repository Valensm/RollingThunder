using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    internal interface IHelpDescriptor
    {
        #region Properties

        string HelpText { get; }

        bool IsVerb { get; }

        bool IsVerbBag { get; }

        bool IsDefaultVerb { get; }

        string ShortName { get; }

        string LongName { get; }

        Type Type { get; }

        bool IsRequired { get; }

        bool IsCollectionType { get; }

        bool IsBooleanType { get; }

        bool CanHaveValue { get; }

        int MaxValueCount { get; }

        int MinValueCount { get; }

        bool IsSimpleType { get; }

        bool IsHelp { get; }

        IEnumerable<string> MutualGroups { get; }

        IEnumerable<IHelpDescriptor> Descriptors { get; }

        string PropertyName { get; }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}