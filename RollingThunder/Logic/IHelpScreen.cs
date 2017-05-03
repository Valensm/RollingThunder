using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    public interface IHelpScreen
    {
        #region Properties

        IEnumerable<string> Lines { get; }

        string Error { get; }

        #endregion Properties
    }
}