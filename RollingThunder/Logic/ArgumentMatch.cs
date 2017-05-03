using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    internal class ArgumentMatch
    {
        #region Properties

        public Descriptor Descriptor { get; }

        public ArgumentGroup Group { get; }

        #endregion Properties

        #region Ctors

        public ArgumentMatch(ArgumentGroup group, Descriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            this.Descriptor = descriptor;
            this.Group = group;
        }

        #endregion Ctors

        #region Private Methods

        private static void MoveNonassignableVerbsFromArgumentToVerbGroup(VerbGroup verbGroup, ArgumentGroup argumentGroup, string[] assignableVerbs)
        {
            string[] removedVerbs = RemoveNonassignableVerbsFromArgumentGroup(assignableVerbs, argumentGroup);
            if (removedVerbs.Length > 0)
            {
                foreach (string newVerb in removedVerbs)
                {
                    verbGroup.AddVerb(newVerb);
                }
            }
        }

        private static string[] RemoveNonassignableVerbsFromArgumentGroup(string[] assignableVerbs, ArgumentGroup group)
        {
            string[] itemsToRemove = group.Arguments.Except(assignableVerbs).ToArray();
            if (itemsToRemove.Length > 0)
            {
                foreach (var itemToRemove in itemsToRemove)
                {
                    group.RemoveArgument(itemToRemove);
                }
            }
            return itemsToRemove;
        }

        private static Descriptor MatchArgToDescriptor(ArgumentGroup argGroup, IEnumerable<Descriptor> descriptors)
        {
            Descriptor[] matchedDescriptors = descriptors.Where(d => d.EqualsName(argGroup.Name)).ToArray();
            if (matchedDescriptors.Length == 1)
            {
                return matchedDescriptors.First();
            }
            else if (matchedDescriptors.Length > 1)
            {
                //We have multiple mappings - don't know which is valid.
                throw new AmbiguousParameterException(argGroup.Name, $"Multiple definitions were found for argument '{argGroup.Name}'.");
            }
            return null;
        }

        private static string[] GetAssignableVerbsForGroup(ArgumentGroup group, Descriptor descriptor)
        {
            int max = descriptor.MaxValuesCount;
            IEnumerable<string> verbs = group.Arguments;

            if (descriptor.IsValueAssignable(verbs))
            {
                return verbs.ToArray();
            }
            else
            {
                return verbs.Where(v => descriptor.IsValueAssignable(v)).Take(max).ToArray();
            }
        }

        private static void ThrowIfHaveUnmatchedGroup(IReadOnlyCollection<ArgumentGroup> unMatchedGroups, ParserConfiguration parserConfiguration)
        {
            if (unMatchedGroups.Count > 0)
            {
                ArgumentGroup unmatchedGroup = unMatchedGroups.First();

                //We have some argument which is not defined is descriptors.
                throw new ArgumentNotSupportedException(unmatchedGroup.Name, $"Argument '{parserConfiguration.ShortNamePrefix}{unmatchedGroup.Name}' is not supported.");
            }
        }

        private static void ThrowIfCannotMatch(VerbGroup verbGroup, ArgumentGroup argumentGroup, Descriptor matchedDescriptor, string[] assignableVerbs, ParserConfiguration parserConfiguration)
        {
            ThrowIfHaveTooManyArgumentValues(argumentGroup, matchedDescriptor, assignableVerbs, parserConfiguration);
            ThrowIfHaveInvalidType(argumentGroup, matchedDescriptor, assignableVerbs, parserConfiguration);
            ThrowIfNotEnoughPArgumentValues(verbGroup, argumentGroup, matchedDescriptor, parserConfiguration);
        }

        private static void ThrowIfHaveTooManyArgumentValues(ArgumentGroup argumentGroup, Descriptor matchedDescriptor, string[] assignableVerbs, ParserConfiguration parserConfiguration)
        {
            if (assignableVerbs.Length > matchedDescriptor.MaxValuesCount)
            {
                //Error - too many values.
                string assignableVerbsString = string.Join(", ", assignableVerbs.Select(v => $"'{v}'"));
                assignableVerbsString = string.IsNullOrWhiteSpace(assignableVerbsString) ? string.Empty : $" Values: {assignableVerbsString}).";
                throw new TooManyArgumentValuesException(matchedDescriptor.ShortName, matchedDescriptor.MaxValuesCount, argumentGroup.Arguments.Count, assignableVerbs, $"Argument '{parserConfiguration.ShortNamePrefix}{matchedDescriptor.ShortName}' allows only {matchedDescriptor.MaxValuesCount} values, but have {argumentGroup.Arguments.Count} values.{assignableVerbsString}");
            }
        }

        private static void ThrowIfHaveInvalidType(ArgumentGroup argumentGroup, Descriptor matchedDescriptor, string[] assignableVerbs, ParserConfiguration parserConfiguration)
        {
            if (assignableVerbs.Length < matchedDescriptor.MinValuesCount)
            {
                //Error - matched by name, but failed by type
                string[] unAssignableVerbs = argumentGroup.Arguments.Except(assignableVerbs).ToArray();
                string unAssignableVerbsString = string.Join(", ", unAssignableVerbs.Select(v => $"'{v}'"));
                unAssignableVerbsString = string.IsNullOrWhiteSpace(unAssignableVerbsString) ? string.Empty : $" Values: {unAssignableVerbsString}).";
                throw new InvalidArgumentTypeException(matchedDescriptor.ShortName, unAssignableVerbs, $"Cannot assign values to argument '{parserConfiguration.ShortNamePrefix}{matchedDescriptor.ShortName}', maybe types don't match.{unAssignableVerbsString}");
            }
        }

        private static void ThrowIfNotEnoughPArgumentValues(VerbGroup verbGroup, ArgumentGroup argumentGroup, Descriptor matchedDescriptor, ParserConfiguration parserConfiguration)
        {
            if (argumentGroup.Arguments.Count < matchedDescriptor.MinValuesCount)
            {
                //Error - matched by name, but dont' have enough values.
                string allVerbsString = string.Join(", ", verbGroup.Verbs.Select(v => $"'{v}'"));
                allVerbsString = string.IsNullOrWhiteSpace(allVerbsString) ? string.Empty : $" Values: {allVerbsString}).";
                throw new NotEnoughArgumentValuesException(matchedDescriptor.ShortName, matchedDescriptor.MinValuesCount, argumentGroup.Arguments.Count, verbGroup.Verbs, $"Argument '{parserConfiguration.ShortNamePrefix}{matchedDescriptor.ShortName}' needs {matchedDescriptor.MinValuesCount} values, but have only {argumentGroup.Arguments.Count} values.{allVerbsString}");
            }
        }

        private static IEnumerable<ArgumentMatch> MatchArgumentsToInnerDescriptors(VerbGroup verbGroup, List<Descriptor> matchedDescriptors, List<ArgumentGroup> unMatchedGroups, ParserConfiguration parserConfiguration)
        {
            if (matchedDescriptors.Count > 0)
            {
                //can take inner descriptors
                //TODO: What if mutual group is defined? What if inner is required, but not found?
                IEnumerable<ArgumentMatch> matches = MatchArgumentsToDescriptors(unMatchedGroups, verbGroup, matchedDescriptors.SelectMany(d => d.Descriptors), parserConfiguration);
                matchedDescriptors.AddRange(matches.Select(m => m.Descriptor));
                foreach (ArgumentGroup groupToRemove in matches.Select(m => m.Group))
                {
                    if (unMatchedGroups.Contains(groupToRemove))
                    {
                        unMatchedGroups.Remove(groupToRemove);
                    }
                }
                return matches;
            }
            else
            {
                return new ArgumentMatch[0];
            }
        }

        private static List<ArgumentMatch> MatchArgumentsToDescriptors(
            IEnumerable<ArgumentGroup> argumentGroups,
            VerbGroup verbGroup,
            IEnumerable<Descriptor> descriptors,
            ParserConfiguration parserConfiguration,
            List<ArgumentGroup> unMatchedGroups,
            List<Descriptor> matchedDescriptors)
        {
            List<ArgumentMatch> result = new List<ArgumentMatch>();
            foreach (ArgumentGroup argumentGroup in argumentGroups)
            {
                Descriptor matchedDescriptor = MatchArgToDescriptor(argumentGroup, descriptors);

                if (matchedDescriptor != null)
                {
                    string[] assignableVerbs = GetAssignableVerbsForGroup(argumentGroup, matchedDescriptor);

                    ThrowIfCannotMatch(verbGroup, argumentGroup, matchedDescriptor, assignableVerbs, parserConfiguration);

                    MoveNonassignableVerbsFromArgumentToVerbGroup(verbGroup, argumentGroup, assignableVerbs);

                    matchedDescriptors.Add(matchedDescriptor);
                    result.Add(new ArgumentMatch(argumentGroup, matchedDescriptor));
                }
                else
                {
                    //add the group to not-macthed list.
                    unMatchedGroups.Add(argumentGroup);
                }
            }
            return result;
        }

        #endregion Private Methods

        #region Public Methods

        public static IEnumerable<ArgumentMatch> MatchArgumentsToDescriptors(
            IEnumerable<ArgumentGroup> argumentGroups,
            VerbGroup verbGroup,
            IEnumerable<Descriptor> descriptors,
            ParserConfiguration parserConfiguration)
        {
            if (argumentGroups == null)
            {
                throw new ArgumentNullException(nameof(argumentGroups));
            }
            if (verbGroup == null)
            {
                throw new ArgumentNullException(nameof(verbGroup));
            }
            if (descriptors == null)
            {
                throw new ArgumentNullException(nameof(descriptors));
            }
            if (parserConfiguration == null)
            {
                throw new ArgumentNullException(nameof(parserConfiguration));
            }

            List<ArgumentGroup> unMatchedGroups = new List<ArgumentGroup>();
            List<Descriptor> matchedDescriptors = new List<Descriptor>();

            List<ArgumentMatch> result = MatchArgumentsToDescriptors(argumentGroups, verbGroup, descriptors, parserConfiguration, unMatchedGroups, matchedDescriptors);

            ArgumentMatch[] matches = MatchArgumentsToInnerDescriptors(verbGroup, matchedDescriptors, unMatchedGroups, parserConfiguration).ToArray();
            if (matches.Length > 0)
            {
                result.AddRange(matches);
            }

            ThrowIfHaveUnmatchedGroup(unMatchedGroups, parserConfiguration);

            return result;
        }

        #endregion Public Methods
    }
}