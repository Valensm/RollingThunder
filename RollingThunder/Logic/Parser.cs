using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    public sealed class Parser<T>
    {
        #region Fields

        private readonly ParserConfiguration parserConfiguration;
        private readonly Func<T> instanceFactory;

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Ctors

        public Parser(Func<T> instanceFactory, ParserConfiguration parserConfiguration)
        {
            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }
            if (parserConfiguration == null)
            {
                throw new ArgumentNullException(nameof(parserConfiguration));
            }

            this.parserConfiguration = parserConfiguration;
            this.instanceFactory = instanceFactory;
        }

        public Parser(Func<T> instanceFactory)
            : this(instanceFactory, new ParserConfiguration())
        {
        }

        #endregion Ctors

        #region Private Methods

        #region Checks

        private static void ThrowIfWrongDefintionSematics(IEnumerable<Descriptor> descriptors)
        {
            descriptors = Descriptor.Flatten(descriptors);

            ThrowIfVerbIsDefinedAsVerbBag(descriptors);
            ThrowIfVerbBagIsNotCollectionType(descriptors);
            ThrowIfHaveMultipleDefaultVerbs(descriptors);
            ThrowIfHaveMultipleVerbBags(descriptors);
            ThrowIfHaveShortNameDuplicates(descriptors);
            ThrowIfHaveLongNameDuplicates(descriptors);
            ThrowIfVerbAreNotBool(descriptors);
        }

        private static void ThrowIfVerbIsDefinedAsVerbBag(IEnumerable<Descriptor> descriptors)
        {
            Descriptor[] invalidVerbDescriptors = descriptors.Where(d => d.MaxValuesCount > 1 && d.IsVerb).ToArray(); // verbs bags marked as verbs.
            if (invalidVerbDescriptors.Length > 0)
            {
                var namesString = string.Join(", ", invalidVerbDescriptors.Select(d => $"'{d.ShortName}'"));
                var names = invalidVerbDescriptors.Select(d => d.ShortName);
                throw new InvalidVerbDefinitionException(names, $"Verb(s) {namesString} is(are) in fact bag(s). Change all [Verb] properties whose type implement IEnumerable<> to [VerbBag].");
            }
        }

        private static void ThrowIfVerbBagIsNotCollectionType(IEnumerable<Descriptor> descriptors)
        {
            Descriptor[] invalidVerbBagDescriptors = descriptors.Where(d => d.MaxValuesCount <= 1 && d.IsVerbBag).ToArray(); //bags not implement IEnumerbale<>.
            if (invalidVerbBagDescriptors.Length > 0)
            {
                var names = invalidVerbBagDescriptors.Select(d => d.ShortName);
                var namesString = string.Join(", ", invalidVerbBagDescriptors.Select(d => $"'{d.ShortName}'"));
                throw new InvalidVerbBagDefinitionException(names, $"Verb bag(s) {namesString} don't implement IEnumerable<>. Change all verb bags to an implementation of IEnumerable<>.");
            }
        }

        private static void ThrowIfHaveMultipleDefaultVerbs(IEnumerable<Descriptor> descriptors)
        {
            Descriptor[] defaultVerbDescriptors = descriptors.Where(d => d.MaxValuesCount == 1 && d.IsDefaultVerb).ToArray();
            if (defaultVerbDescriptors.Length > 1)
            {
                var namesString = string.Join(", ", defaultVerbDescriptors.Select(d => $"'{d.ShortName}'"));
                var names = defaultVerbDescriptors.Select(d => d.ShortName);
                throw new MultipleDefaultVerbDefinitionsException(names, $"Multiple [DefaultVerb] definitions ({namesString}). Only single occurrence of [DefaultVerb] is allowed.");
            }
        }

        private static void ThrowIfHaveMultipleVerbBags(IEnumerable<Descriptor> descriptors)
        {
            Descriptor[] verbBagDescriptors = descriptors.Where(d => d.MaxValuesCount > 1 && d.IsVerbBag).ToArray();

            if (verbBagDescriptors.Length > 1)
            {
                var namesString = string.Join(", ", verbBagDescriptors.Select(d => $"'{d.ShortName}'"));
                var names = verbBagDescriptors.Select(d => d.ShortName);
                throw new MultipleVerbBagDefinitionsException(names, $"Multiple [VerbBag] definitions ({namesString}). Only single occurrence of [VerbBag] is allowed.");
            }
        }

        private static void ThrowIfHaveShortNameDuplicates(IEnumerable<Descriptor> descriptors)
        {
            var shortNameDuplicates = descriptors.GroupBy(d => d.ShortName, StringComparer.InvariantCultureIgnoreCase)
                           .Select(a => new { Key = a.Key, Count = a.Count(), Duplicates = a.Select(d => d.ShortName).ToArray() })
                           .Where(a => a.Count > 1)
                           .ToArray();
            if (shortNameDuplicates.Length > 0)
            {
                string[] shortNameDuplicateNames = shortNameDuplicates.SelectMany(a => a.Duplicates.Select(b => $"'{b}'")).ToArray();
                string[] names = shortNameDuplicates.Select(a => a.Key).ToArray();
                throw new AmbiguousNameDefinitionException(names, $"Multiple short name definitions found: {string.Join(", ", shortNameDuplicateNames)}.");
            }
        }

        private static void ThrowIfHaveLongNameDuplicates(IEnumerable<Descriptor> descriptors)
        {
            var longNameDuplicates = descriptors.GroupBy(d => d.LongName, StringComparer.InvariantCultureIgnoreCase)
                .Select(a => new { Key = a.Key, Count = a.Count(), Duplicates = a.Select(d => d.LongName).ToArray() })
                .Where(a => a.Count > 1)
                .ToArray();
            if (longNameDuplicates.Length > 0)
            {
                string[] longNameDuplicateNames = longNameDuplicates.SelectMany(a => a.Duplicates.Select(b => $"'{b}'")).ToArray();
                string[] names = longNameDuplicates.Select(a => a.Key).ToArray();
                throw new AmbiguousNameDefinitionException(names, $"Multiple long name definitions found: {string.Join(", ", longNameDuplicateNames)}.");
            }
        }

        private static void ThrowIfVerbAreNotBool(IEnumerable<Descriptor> descriptors)
        {
            Descriptor[] verbDescriptors = descriptors.Where(d => d.MaxValuesCount == 1 && d.IsVerb).ToArray();
            if (verbDescriptors.Length > 0)
            {
                //this means that those verbs behaves like bag of enums. Those verbs must be bool.
                Descriptor[] nonBoolVerbs = verbDescriptors.Where(d => !d.IsBool).ToArray();
                String[] nonBoolVerbNamesString = nonBoolVerbs.Select(d => $"'{d.ShortName}': {d.Type.Name}").ToArray();
                var names = nonBoolVerbs.Select(d => d.ShortName).ToArray();
                throw new NonBoolVerbsException(names, $"All verbs have to Bool types: {string.Join(", ", nonBoolVerbNamesString)}.");
            }
        }

        private static void ThrowIfWrongParsingSemantics(VerbMatch match)
        {
            //single [Verb] is filled with an value from args. Can be required. It is like verb bag, but with a single value.
            //??? Multiple [Verb] is like Enum - white list of values. Can be required or not. If one is required, it means - provide one value from the white list.
            //Multiple [Verb] defs means - check whether the verb is present - so it doesn't make sense to have other than Bool types for those. Or maybe try to cast to the type of the property.
            //Verb bag captures all values from args. Can be required. Then it means at least one value is required.
            //Combination of bag/verb prioritizes [Verb] properties. The rest is put into a bag. But if a verb is required and bag as well and only one value is specified, then it is error for the bag.
            //Bag can be real Enum type - errors during adding to the bag may occur - casting the the Enum type.
            //Default verb (A) and another verb (B) means max 2 values and one of them must have exact value=B, one is optional - anything - it is filled to property A. There is challenge with combination of required and this situation.

            Descriptor[] verbDescriptors = match.Descriptors.Where(d => d.IsVerb && !d.IsDefaultVerb).ToArray();
            Descriptor defaultVerbDescriptor = match.Descriptors.FirstOrDefault(d => d.IsDefaultVerb);
            Descriptor verbBagDescriptor = match.Descriptors.FirstOrDefault(d => d.IsVerbBag);

            string firstVerb = match.Group.Verbs.FirstOrDefault() ?? string.Empty;

            if (match.VerbCount == 1 && verbBagDescriptor == null && defaultVerbDescriptor == null && verbDescriptors.Length > 0)
            {
                //Error - don't know where to put the single verb
                //No Default verb, no verb bag, multiple verbs, but have single verb value.
                throw new AmbiguousVerbException(firstVerb, $"Have single verb '{firstVerb}', in definition there is no bag nor default verb defined, but there are multiple verbs defined. Don't know where to put verb '{firstVerb}'.");
            }

            if (match.VerbCount == 0 && defaultVerbDescriptor != null && defaultVerbDescriptor.IsRequired)
            {
                //Error - single verb is required - error.
                throw new RequiredDefaultVerbException(defaultVerbDescriptor.ShortName, $"Have no verb, but in definition there is default verb '{defaultVerbDescriptor.ShortName}' which is required.");
            }

            if (match.VerbCount == 1 && verbBagDescriptor != null && verbBagDescriptor.IsRequired && defaultVerbDescriptor != null && defaultVerbDescriptor.IsRequired)
            {
                //Error - Don't know where to put - bag or default?
                throw new CannotSatisfyRequiredDefaultVerbAndBagException(
                    firstVerb,
                    defaultVerbDescriptor.ShortName,
                    verbBagDescriptor.ShortName,
                    $"Have single verb '{firstVerb}', but in definition there are bag '{verbBagDescriptor.ShortName}' and default verb '{defaultVerbDescriptor.ShortName}' defined, and both are required."
                    );
            }

            if (match.VerbCount == 0 && verbBagDescriptor != null && verbBagDescriptor.IsRequired)
            {
                //Error - bag is required, but don't have verbs.
                throw new RequiredVerbBagException(verbBagDescriptor.ShortName, $"Have no verb, but in definition there is bag '{verbBagDescriptor.ShortName}', which is required.");
            }
        }

        private static void ThrowIfDuplicateGroupsExist(SplitResult splitResult)
        {
            string[] duplicateGroupsNames = splitResult.ArgumentGroups.GroupBy(g => g.Name)
                .Where(a => a.Count() > 1)
                .Select(a => a.Key)
                .ToArray();

            if (duplicateGroupsNames.Length > 0)
            {
                string namesString = string.Join(", ", duplicateGroupsNames.Select(a => $"'{a}'"));
                throw new DuplicateArgumentsException(duplicateGroupsNames, $"Have duplicate argument(s) {namesString}.");
            }
        }

        private static void ThrowIfMutualGroupsConflict(IEnumerable<Descriptor> matchedDescriptors)
        {
            if (matchedDescriptors.Count() > 0)
            {
                var allMutualGroups = matchedDescriptors
                    .SelectMany(d => d.MutualGroups)
                    .Distinct()
                    .ToArray();
                foreach (var group in allMutualGroups)
                {
                    var conflictingDescriptors = matchedDescriptors.Where(d => d.MutualGroups.Contains(group)).ToArray();
                    var argumentNames = conflictingDescriptors.Select(d => d.ShortName).ToArray();
                    var conflictingShortNames = conflictingDescriptors.Select(d => $"'{d.ShortName}'").ToArray();
                    var conflictingShortNamesString = string.Join(", ", conflictingShortNames);
                    if (conflictingDescriptors.Length > 1)
                    {
                        throw new MutualGroupArgumentsException(group, argumentNames, $"Mutual exclusive group '{group}' has multiple active arguments: {conflictingShortNamesString}.");
                    }
                }
            }
        }

        private static void ThrowIfRequiredIsMissing(IEnumerable<Descriptor> matchedDescriptors, IEnumerable<Descriptor> descriptors, ParserConfiguration configuration)
        {
            //find all required args with no corresponding matches.
            Descriptor[] requiredDescriptors = descriptors.Where(d => d.IsRequired).ToArray();
            string[] matchedDescriptorsNames = matchedDescriptors.Select(m => m.ShortName).ToArray();
            Descriptor[] missingDescriptors = requiredDescriptors.Where(d => !matchedDescriptorsNames.Contains(d.ShortName)).ToArray();
            if (missingDescriptors.Length > 0)
            {
                Descriptor missignDescriptor = missingDescriptors.First();
                throw new RequiredParameterException(missignDescriptor.ShortName, GetRequiredErrorText(missignDescriptor, configuration));

                //foreach (Descriptor missignDescriptor in missingDescriptors)
                //{
                //    errorContext.AddError(new ErrorItem(100, GetRequiredErrorText(missignDescriptor, configuration), missignDescriptor));
                //    CheckRequired(matchedDescriptors, missignDescriptor.Descriptors, configuration, errorContext);
                //}
            }
        }

        private static string GetRequiredErrorText(Descriptor descriptor, ParserConfiguration configuration)
        {
            string errorText = string.Empty;
            var requiredValidator = descriptor.Validators
                       .Where(v => v is Validators.IsRequiredValidator)
                       .Cast<Validators.IsRequiredValidator>()
                       .FirstOrDefault();
            if (requiredValidator != null)
            {
                errorText = requiredValidator.Validate(null, descriptor.ShortName).ErrorText;
            }
            if (string.IsNullOrEmpty(errorText))
            {
                //don't have error message, take the default
                errorText = string.Format(configuration.DefaultRequiredErrorMessage, descriptor.ShortName, descriptor.LongName);
            }
            return errorText;
        }

        #endregion Checks

        private static void Process(DescriptorMatch descriptorMatch, SplitResult splitResult, IEnumerable<Descriptor> descriptors, ParserConfiguration parserConfiguration)
        {
            if (!splitResult.IsEmpty)
            {
                ThrowIfMutualGroupsConflict(descriptorMatch.AllDescriptors);

                Descriptor[] allWithMatchedIncludingInnerDescriptors = descriptorMatch.AllDescriptors
                    .SelectMany(d => d.Descriptors)
                    .Concat(descriptorMatch.AllDescriptors)
                    .Concat(descriptors)
                    .GroupBy(d => d)
                    .Select(d => d.First())
                    .ToArray();

                ThrowIfRequiredIsMissing(descriptorMatch.AllDescriptors, allWithMatchedIncludingInnerDescriptors, parserConfiguration);

                if (descriptorMatch.VerbMatch != null)
                {
                    ThrowIfWrongParsingSemantics(descriptorMatch.VerbMatch);
                    ProcessVerbs(descriptorMatch.VerbMatch);
                }
                if (descriptorMatch.ArgumentMatches.Count > 0)
                {
                    ProcessArguments(descriptorMatch.ArgumentMatches);
                }
            }
            else
            {
                //don't have anything - empty command line
                //handle required verbs and/or args
                ThrowIfRequiredIsMissing(new Descriptor[0], descriptors, parserConfiguration);
            }
        }

        #region Verbs

        private static bool TryAssingVerbValue(VerbGroup verbGroup, Descriptor verbDescriptor, bool thowExceptionIfError)
        {
            //try to find a verb by the descriptor type and assign.
            //if nothing found - error.
            string defaultValue;
            if (verbDescriptor.IsBool)
            {
                //match verb=name
                //if not found->error
                string foundVerb = verbGroup.Verbs.FirstOrDefault(v => verbDescriptor.EqualsName(v));
                if (foundVerb != null)
                {
                    verbDescriptor.AssignValue(true);
                    verbGroup.RemoveVerb(foundVerb);
                    return true;
                }
                else if (thowExceptionIfError)
                {
                    //Error - no possible value was found.
                    throw new UnassignableBoolVerbException(verbDescriptor.ShortName, verbGroup.Verbs, $"No matching value found for bool verb '{verbDescriptor.ShortName}' from verbs {string.Join(", ", verbGroup.Verbs)}.");
                }
            }
            else if (verbDescriptor.IsDefaultVerb)
            {
                //match first assignable value
                if (TryFindValueForDescriptor(verbGroup.Verbs, verbDescriptor, out defaultValue))
                {
                    verbDescriptor.AssignValue(defaultValue);
                    verbGroup.RemoveVerb(defaultValue);
                    return true;
                }
                else if (thowExceptionIfError)
                {
                    //Error - no possible value was found.
                    throw new UnassignableDefaultVerbException(verbDescriptor.ShortName, verbGroup.Verbs, $"No value found for verb '{verbDescriptor.ShortName}' from verbs {string.Join(", ", verbGroup.Verbs)}.");
                }
            }
            return false;
        }

        private static IEnumerable<Descriptor> TryFindDescriptorsAndAssingValue(VerbGroup verbGroup, Descriptor[] verbDescriptors)
        {
            //take verbs and try to find matches and assign them.
            List<Descriptor> assignedDescriptors = new List<Descriptor>(verbDescriptors.Length);
            foreach (Descriptor verbDescriptor in verbDescriptors)
            {
                if (TryAssingVerbValue(verbGroup, verbDescriptor, false))
                {
                    assignedDescriptors.Add(verbDescriptor);
                }
            }

            return assignedDescriptors;
        }

        private static bool TryFindValueForDescriptor(IEnumerable<string> verbs, Descriptor descriptor, out string value)
        {
            value = verbs.FirstOrDefault(v => descriptor.IsValueAssignable(v));
            return value != null;
        }

        private static bool TryAssignValueCollectionToVerbBag(VerbGroup verbGroup, Descriptor verbBagDescriptor)
        {
            //and the rest to the bag.
            string[] values = verbGroup.Verbs.ToArray();
            if (verbBagDescriptor.IsValueAssignable(values))
            {
                verbBagDescriptor.AssignValue(values);
                foreach (string value in values)
                {
                    verbGroup.RemoveVerb(value);
                }
                return true;
            }

            return false;
        }

        private static void ProcessVerbs(VerbMatch match)
        {
            Descriptor[] nonDefaultVerbDescriptors = match.Descriptors.Where(d => d.IsVerb && !d.IsDefaultVerb).ToArray();

            Descriptor defaultVerbDescriptor = match.Descriptors.FirstOrDefault(d => d.IsDefaultVerb);
            Descriptor verbBagDescriptor = match.Descriptors.FirstOrDefault(d => d.IsVerbBag);

            bool hasDefaultVerb = defaultVerbDescriptor != null;
            bool hasNonDefaultVerbs = nonDefaultVerbDescriptors.Length > 0;
            bool hasVerbBag = verbBagDescriptor != null;

            bool hasSingleMatch = match.VerbCount == 1;

            if (hasSingleMatch && !hasNonDefaultVerbs)
            {
                if (hasDefaultVerb && !hasVerbBag)
                {
                    //Try set the value to default verb descriptor.
                    TryAssingVerbValue(match.Group, defaultVerbDescriptor, true);
                }

                if (!hasDefaultVerb && hasVerbBag)
                {
                    //Try set the value to verb bag.
                    TryAssignValueCollectionToVerbBag(match.Group, verbBagDescriptor);
                }
            }

            bool hasMatches = match.VerbCount > 0;

            if (hasMatches)
            {
                if (hasDefaultVerb)
                {
                    //take first to the default.
                    TryAssingVerbValue(match.Group, defaultVerbDescriptor, true);
                }

                if (hasNonDefaultVerbs)
                {
                    //take verbs and try to find matches and assign them.
                    TryFindDescriptorsAndAssingValue(match.Group, nonDefaultVerbDescriptors);
                }

                if (hasVerbBag)
                {
                    //and the rest to the bag.
                    TryAssignValueCollectionToVerbBag(match.Group, verbBagDescriptor);
                }

                ThrowIfHaveUnassignableValues(match);
            }
        }

        private static void ThrowIfHaveUnassignableValues(VerbMatch match)
        {
            if (match.VerbCount > 0)
            {
                string verbsString = string.Join(", ", match.Group.Verbs.Select(v => $"'{v}'"));
                verbsString = string.IsNullOrWhiteSpace(verbsString) ? string.Empty : $" Verbs: {verbsString}";
                //Error - have something remaining
                throw new UnassignableVerbsException(match.Group.Verbs, $"Have {match.VerbCount} verbs remaining, but don't know where to put it.{verbsString}");
            }
        }

        #endregion Verbs

        #region Arguments

        private static void ProcessArguments(IEnumerable<ArgumentMatch> matches)
        {
            foreach (ArgumentMatch match in matches)
            {
                AssignArgumentValue(match);
            }
        }

        private static void AssignArgumentValue(ArgumentMatch match)
        {
            Descriptor matchedDescriptor = match.Descriptor;
            string[] assignableVerbs = match.Group.Arguments.ToArray();
            //Assign value(s) to arg
            if (matchedDescriptor.IsCollectionType)
            {
                matchedDescriptor.AssignValue(assignableVerbs);
            }
            else if (matchedDescriptor.IsBool)
            {
                matchedDescriptor.AssignValue(true);
            }
            else if (matchedDescriptor.IsSimpleType)
            {
                if (assignableVerbs.Length > 0)
                {
                    matchedDescriptor.AssignValue(assignableVerbs[0]);
                }
            }
            else
            {
                //some complex type
            }
        }

        private static bool IsHelpRequested(SplitResult splitResult, IEnumerable<Descriptor> descriptors)
        {
            var helpDescriptors = descriptors.Where(d => d.IsHelp).ToArray();
            if (helpDescriptors.Length == 0)
            {
                return false;
            }

            var pairs = splitResult.ArgumentGroups
                .Select(g => new { Group = g, Descriptor = helpDescriptors.FirstOrDefault(d => d.EqualsName(g.Name)) })
                .Where(a => a.Descriptor != null)
                .ToArray();

            foreach (var pair in pairs)
            {
                try
                {
                    object value = null;
                    if (pair.Group.Arguments.Count > 1)
                    {
                        value = pair.Group.Arguments.AsEnumerable();
                    }
                    else if (pair.Group.Arguments.Count == 0)
                    {
                        if (pair.Descriptor.IsBool)
                        {
                            value = true;
                        }
                        else
                        {
                            value = null;
                        }
                    }
                    else
                    {
                        value = pair.Group.Arguments.First();
                    }

                    pair.Descriptor.AssignValue(value);
                }
                catch (Exception ex)
                {
                    string valueString = string.Join(", ", pair.Group.Arguments);
                    throw new UnassignableParameterException(pair.Descriptor.ShortName, valueString, $"Cannot assign value(s) {valueString} to parameter '{pair.Descriptor.ShortName}'.", ex);
                }
            }

            return helpDescriptors.Length > 0;
        }

        #endregion Arguments

        #endregion Private Methods

        #region Public Methods

        public T Parse(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            T instance = this.instanceFactory();

            IEnumerable<Descriptor> descriptors = Descriptor.AllFromInstance<T>(instance);
            SplitResult splitResult = SplitResult.FromArgs(args, this.parserConfiguration);

            if (!IsHelpRequested(splitResult, descriptors))
            {
                DescriptorMatch descriptorMatch = DescriptorMatch.MatchDescriptorsToGroups(splitResult, descriptors, this.parserConfiguration);
                ThrowIfWrongDefintionSematics(descriptors);
                ThrowIfDuplicateGroupsExist(splitResult);
                Process(descriptorMatch, splitResult, descriptors, this.parserConfiguration);
            }
            else if (this.parserConfiguration.ThrowHelpException)
            {
                throw new HelpException();
            }
            return instance;
        }

        public IHelpScreen HelpScreen(string error, HelpConfiguration helpConfigration)
        {
            return new HelpsScreen<T>(this.instanceFactory, error, helpConfigration, this.parserConfiguration);
        }

        public IHelpScreen HelpScreen(string error)
            => this.HelpScreen(error, new HelpConfiguration());

        public IHelpScreen HelpScreen()
            => this.HelpScreen(string.Empty);

        #endregion Public Methods
    }
}