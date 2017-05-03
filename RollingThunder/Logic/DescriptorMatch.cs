using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    internal class DescriptorMatch
    {
        #region Properties

        public VerbMatch VerbMatch { get; }

        public IReadOnlyCollection<ArgumentMatch> ArgumentMatches { get; }

        public IReadOnlyCollection<Descriptor> AllDescriptors { get; }

        #endregion Properties

        #region Ctors

        public DescriptorMatch(VerbMatch verbMatch, IEnumerable<ArgumentMatch> argMatches)
        {
            this.VerbMatch = verbMatch;
            this.ArgumentMatches = argMatches == null ? new ArgumentMatch[0] : argMatches.ToArray();
            Descriptor[] verbDescriptors = verbMatch == null ? new Descriptor[0] : verbMatch.Descriptors.ToArray();
            Descriptor[] argDescriptors = argMatches.Select(m => m.Descriptor).ToArray();
            this.AllDescriptors = verbDescriptors.Concat(argDescriptors).ToArray();
        }

        #endregion Ctors

        #region Public Methods

        public static DescriptorMatch MatchDescriptorsToGroups(SplitResult splitResult, IEnumerable<Descriptor> descriptors, ParserConfiguration configuration)
        {
            if (splitResult == null)
            {
                throw new ArgumentNullException(nameof(splitResult));
            }
            if (descriptors == null)
            {
                throw new ArgumentNullException(nameof(descriptors));
            }
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            VerbGroup verbGroup = splitResult.VerbGroup ?? new VerbGroup();

            IEnumerable<ArgumentMatch> argumentMatches = ArgumentMatch.MatchArgumentsToDescriptors(splitResult.ArgumentGroups, verbGroup, descriptors, configuration);
            VerbMatch verbMatch = VerbMatch.MatchVerbsToDescriptors(verbGroup, descriptors, configuration);

            return new DescriptorMatch(verbMatch, argumentMatches);
        }

        #endregion Public Methods
    }
}