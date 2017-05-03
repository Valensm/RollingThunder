using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    internal class VerbMatch
    {
        #region Properties

        public VerbGroup Group { get; }

        public Descriptor[] Descriptors { get; }

        public int VerbCount => this.Group?.Verbs?.Count ?? 0;

        #endregion Properties

        #region Ctors

        public VerbMatch(VerbGroup group, IEnumerable<Descriptor> descriptors)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            if (descriptors == null)
            {
                throw new ArgumentNullException(nameof(descriptors));
            }
            if (descriptors.Count() == 0)
            {
                throw new ArgumentException("Cannot be empty.", nameof(descriptors));
            }
            this.Group = group;
            this.Descriptors = descriptors.ToArray();
        }

        #endregion Ctors

        #region Public Methods

        public static VerbMatch MatchVerbsToDescriptors(VerbGroup verbGroup, IEnumerable<Descriptor> descriptors, ParserConfiguration configuration)
        {
            if (verbGroup == null)
            {
                throw new ArgumentNullException(nameof(verbGroup));
            }
            if (descriptors == null)
            {
                throw new ArgumentNullException(nameof(descriptors));
            }
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            VerbMatch verbMatch = null;

            if (verbGroup.Verbs.Count > 0)
            {
                Descriptor[] allVerbDescriptors = descriptors.Where(d => d.IsDefaultVerb || d.IsVerb || d.IsVerbBag).ToArray();
                Descriptor defaultVerbDescriptor = descriptors.FirstOrDefault(d => d.IsDefaultVerb);
                Descriptor verbBagDescriptor = descriptors.FirstOrDefault(d => d.IsVerbBag);
                int verbCount = verbGroup.Verbs.Count;

                //multiple default verbs and bags should be covered by static analysis.
                if (verbCount == 1)
                {
                    if (defaultVerbDescriptor == null)
                    {
                        if (verbBagDescriptor == null)
                        {
                            throw new NoDefaultVerbOrBagNotFoundException("No default verb or verb bag definition was found.");
                        }
                    }
                }
                else
                {
                    if (verbBagDescriptor == null)
                    {
                        throw new NoVerbBagFoundException("No verb bag definition was found.");
                    }
                }

                verbMatch = new VerbMatch(verbGroup, allVerbDescriptors);
            }
            return verbMatch;
        }

        #endregion Public Methods
    }
}