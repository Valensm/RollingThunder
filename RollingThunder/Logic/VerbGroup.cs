using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    internal class VerbGroup : ValueGroup
    {
        #region Fields

        #endregion Fields

        #region Properties

        public IReadOnlyCollection<string> Verbs => base.Values;

        public bool HasVerbs => base.HasValues;

        #endregion Properties

        #region Ctors

        public VerbGroup(IEnumerable<string> verbs = null) : base(string.Empty, verbs)
        {
        }

        #endregion Ctors

        #region Private Methods

        #endregion Private Methods

        #region Public Methods

        public void AddVerb(string verb)
            => base.AddValue(verb);

        public void RemoveVerb(string verb)
            => base.RemoveValue(verb);

        #endregion Public Methods
    }
}