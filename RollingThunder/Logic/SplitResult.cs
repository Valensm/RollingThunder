using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    internal class SplitResult
    {
        #region Fields

        #endregion Fields

        #region Properties

        public IReadOnlyCollection<ArgumentGroup> ArgumentGroups { get; }

        public VerbGroup VerbGroup { get; }

        public bool IsEmpty => (this.VerbGroup == null || this.VerbGroup.Values.Count == 0) && this.ArgumentGroups.Count == 0;

        public bool HasVerbGroup { get; set; }

        #endregion Properties

        #region Ctors

        public SplitResult(VerbGroup verbGroup, IEnumerable<ArgumentGroup> argumentGroups)
        {
            this.VerbGroup = verbGroup;
            this.ArgumentGroups = argumentGroups.ToArray();
        }

        #endregion Ctors

        #region Private Methods

        #endregion Private Methods

        #region Public Methods

        public static SplitResult FromArgs(IEnumerable<string> args, ParserConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            VerbGroup verbGroup = null;
            ValueGroup lastGroup = null;
            List<ArgumentGroup> argumentGroups = new List<ArgumentGroup>();

            foreach (string arg in args)
            {
                bool createNewGroup = false;
                string newGroupName = string.Empty;

                bool startsWithShortPrefix = arg.StartsWith(configuration.ShortNamePrefix);
                bool startsWithLongPrefix = arg.StartsWith(configuration.LongNamePrefix);
                if (startsWithLongPrefix || startsWithShortPrefix)
                {
                    newGroupName = string.Empty;
                    if (startsWithLongPrefix)
                    {
                        newGroupName = arg.Substring(configuration.LongNamePrefix.Length);
                    }
                    else if (startsWithShortPrefix)
                    {
                        newGroupName = arg.Substring(configuration.ShortNamePrefix.Length);
                    }

                    createNewGroup = true;
                }

                if (createNewGroup)
                {
                    //need to create new argument group, add to result

                    ArgumentGroup newArgumentGroup = new ArgumentGroup(newGroupName);
                    argumentGroups.Add(newArgumentGroup);

                    //new group -> last used for adding next values.
                    lastGroup = newArgumentGroup;
                }
                else
                {
                    if (lastGroup == null)
                    {
                        //we are on very beginning and values belongs to a verb group.
                        //so need to create verb group and store it for later use.
                        verbGroup = new VerbGroup();
                        lastGroup = verbGroup;
                    }

                    //add value to previously created new group, verb/argument group, don't care the type.
                    lastGroup.AddValue(arg);
                }
            }
            return new SplitResult(verbGroup, argumentGroups);
        }

        #endregion Public Methods
    }
}