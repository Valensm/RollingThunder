using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    internal class Descriptor
    {
        #region Fields

        private IEnumerable<Attribute> attributes;

        #endregion Fields

        #region Properties

        public object Target { get; }

        public PropertyInfo PropertyInfo { get; }

        public Type Type { get; }

        public string PropertyName { get; private set; }

        public string ShortName { get; private set; }

        public string LongName { get; private set; }

        public bool IsIgnored { get; private set; }

        public bool IsRequired { get; private set; }

        public Action<object> Setter { get; private set; }

        public Func<object> Getter { get; private set; }

        public IList<Validators.Validator> Validators { get; private set; }

        public bool IsVerb { get; private set; }

        public bool IsDefaultVerb { get; private set; }

        public bool IsVerbBag { get; private set; }

        public string Helptext { get; private set; }

        public Descriptor[] Descriptors { get; private set; }

        public bool IsCollectionType { get; }

        public bool CanHaveValue { get; }

        public bool IsBool { get; }

        public bool IsSimpleType { get; }

        public int MaxValuesCount
        {
            get
            {
                if (this.CanHaveValue && !this.IsCollectionType)
                {
                    return 1;
                }
                else if (!this.IsCollectionType)
                {
                    return 0;
                }
                else
                {
                    return int.MaxValue;
                }
            }
        }

        public int MinValuesCount
        {
            get
            {
                if (this.CanHaveValue && !this.IsCollectionType)
                {
                    return 1;
                }
                else if (!this.IsCollectionType)
                {
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool IsHelp { get; private set; }

        public bool WasValueSet { get; private set; }

        public IEnumerable<string> MutualGroups { get; private set; }

        #endregion Properties

        #region Ctors

        public Descriptor(PropertyInfo propertyInfo, object target, int level, int maxLevel)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }
            this.PropertyInfo = propertyInfo;
            this.Target = target;
            this.PropertyName = propertyInfo.Name;

            this.ShortName = propertyInfo.Name;
            this.LongName = this.ShortName;
            this.Type = propertyInfo.PropertyType;
            this.IsCollectionType = IsCollection(this.Type);
            this.CanHaveValue = CanTypeHaveValue(this.Type);
            this.IsBool = IsBoolType(this.Type);
            this.IsSimpleType = IsSimple(this.Type);

            this.Validators = new List<Validators.Validator>();
            this.Helptext = string.Empty;
            this.Setter = value => { propertyInfo.SetValue(this.Target, value); this.WasValueSet = true; };
            this.Getter = () => propertyInfo.GetValue(this.Target);

            this.attributes = InitializeAttributes(this.PropertyInfo);
            this.HandleAttributes();

            this.CheckForConstraintsAndThrow();

            if (this.Getter() == null)
            {
                this.Setter(CreateInstance(propertyInfo.PropertyType));
            }

            this.Descriptors = GetDescriptors(propertyInfo.PropertyType, this.Getter(), level, maxLevel);
        }

        #endregion Ctors

        #region Private Methods

        private void CheckForConstraintsAndThrow()
        {
            if (this.IsVerb && this.IsDefaultVerb)
            {
                throw new NotSupportedException("Combination of [DefaultVerb] and [Verb] doesn't make sense.");
            }
            else if (this.IsVerbBag && this.IsVerb)
            {
                throw new NotSupportedException("Combination of [VerbBag] and [Verb] doesn't make sense.");
            }
            else if (this.IsVerbBag && this.IsDefaultVerb)
            {
                throw new NotSupportedException("Combination of [VerbBag] and [DefaultVerb] doesn't make sense.");
            }
        }

        private static bool IsSimple(Type type)
        {
            Type[] simpleTypes = new[]
            {
                typeof(string), typeof(char),typeof(char?),
                typeof(int), typeof(uint), typeof(short), typeof(ushort), typeof(long), typeof(ulong),
                typeof(int?), typeof(uint?), typeof(short?), typeof(ushort?), typeof(long?), typeof(ulong?),
                typeof(byte), typeof(sbyte),
                typeof(byte?), typeof(sbyte?),
                typeof(float), typeof(double), typeof(decimal),
                typeof(float?), typeof(double?), typeof(decimal?),
                typeof(bool), typeof(bool?),
                typeof(DateTime), typeof(DateTime?)
            };

            return simpleTypes.Contains(type) || type.IsEnum || IsNullableGeneric(type);
        }

        private static bool IsNullableGeneric(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static List<Attribute> InitializeAttributes(PropertyInfo pi)
        {
            return new List<Attribute>(
                pi.GetCustomAttributes<NameAttribute>().Cast<Attribute>()
                .Concat(pi.GetCustomAttributes<RequiredAttribute>().Cast<Attribute>())
                .Concat(pi.GetCustomAttributes<IgnoreAttribute>().Cast<Attribute>())
                .Concat(pi.GetCustomAttributes<VerbAttribute>().Cast<Attribute>())
                .Concat(pi.GetCustomAttributes<VerbBagAttribute>().Cast<Attribute>())
                .Concat(pi.GetCustomAttributes<DefaultVerbAttribute>().Cast<Attribute>())
                .Concat(pi.GetCustomAttributes<HelpTextAttribute>().Cast<Attribute>())
                .Concat(pi.GetCustomAttributes<HelpOptionAttribute>().Cast<Attribute>())
                .Concat(pi.GetCustomAttributes<MutualGroupAttribute>().Cast<MutualGroupAttribute>())
            );
        }

        private void HandleAttributes()
        {
            this.HandleName();
            this.HandleIsRequired();
            this.HandleIgnored();
            this.HandleVerb();
            this.HandleVerbBag();
            this.HandleDefaultVerb();
            this.HandleHelpText();
            this.HandleMutualGroups();
            this.HandleHelp();
        }

        private void HandleName()
        {
            var nameAttribute = this.attributes.FirstOrDefaultOf<NameAttribute, Attribute>();
            if (nameAttribute != null)
            {
                this.ShortName = nameAttribute.ShortName;
                if (!string.IsNullOrEmpty(nameAttribute.LongName))
                {
                    this.LongName = nameAttribute.LongName;
                }
                if (!string.IsNullOrEmpty(nameAttribute.PropertyName))
                {
                    this.PropertyName = nameAttribute.PropertyName;
                }
            }
        }

        private void HandleHelp()
        {
            var helpAttribute = this.attributes.FirstOrDefaultOf<HelpOptionAttribute, Attribute>();
            this.IsHelp = helpAttribute != null;
        }

        private void HandleIsRequired()
        {
            var isRequiredAttribute = this.attributes.FirstOrDefaultOf<RequiredAttribute, Attribute>();
            if (isRequiredAttribute != null)
            {
                this.Validators.Add(new Validators.IsRequiredValidator(this.Type, isRequiredAttribute.ErrorText));
                this.IsRequired = true;
            }
        }

        private void HandleIgnored()
        {
            var ignoreAttribute = this.attributes.FirstOrDefaultOf<IgnoreAttribute, Attribute>();
            if (ignoreAttribute != null)
            {
                this.IsIgnored = true;
            }
        }

        private void HandleVerb()
        {
            var verbAttribute = this.attributes.FirstOrDefaultOf<VerbAttribute, Attribute>();
            if (verbAttribute != null)
            {
                this.IsVerb = true;
            }
        }

        private void HandleVerbBag()
        {
            var verbBagAttribute = this.attributes.FirstOrDefaultOf<VerbBagAttribute, Attribute>();
            if (verbBagAttribute != null)
            {
                this.IsVerbBag = true;
            }
        }

        private void HandleDefaultVerb()
        {
            var defaultVerbAttribute = this.attributes.FirstOrDefaultOf<DefaultVerbAttribute, Attribute>();
            if (defaultVerbAttribute != null)
            {
                this.IsDefaultVerb = true;
            }
        }

        private void HandleHelpText()
        {
            var helpTextAttribute = this.attributes.FirstOrDefaultOf<HelpTextAttribute, Attribute>();
            if (helpTextAttribute != null)
            {
                this.Helptext = helpTextAttribute.HelpText;
            }
        }

        private void HandleMutualGroups()
        {
            this.MutualGroups = this.attributes.Where(a => a is MutualGroupAttribute)
                .Cast<MutualGroupAttribute>()
                .Select(a => a.MutualGroupName).ToArray();
        }

        private static bool IsCollection(Type type)
        {
            return type.IsArray || (type.IsGenericType && type.ImplementsGenericDefinition(typeof(IEnumerable<>)));
        }

        private static bool CanTypeHaveValue(Type type)
        {
            return IsSimple(type) && type != typeof(bool);
        }

        private static bool IsBoolType(Type type)
        {
            return type == typeof(bool);
        }

        private static object CreateInstance(Type type)
        {
            if (type == typeof(string))
            {
                return null;
            }
            else if (type.IsArray)
            {
                return Array.CreateInstance(type.GetElementType(), 0);
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>) || type.ImplementsGenericDefinition(typeof(IDictionary<,>)))
            {
                Type[] argumentTypes = type.GetGenericArguments().ToArray();
                Type listType = typeof(Dictionary<,>).MakeGenericType(argumentTypes);

                return Activator.CreateInstance(listType);
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || type.ImplementsGenericDefinition(typeof(IEnumerable<>)))
            {
                Type argumentType = type.GetGenericArguments().FirstOrDefault();
                Type listType = typeof(List<>).MakeGenericType(argumentType);

                return Activator.CreateInstance(listType);
            }
            else if (IsNullableGeneric(type))
            {
                object argumentInstance = Activator.CreateInstance(type.GetGenericArguments().First());
                return Activator.CreateInstance(type, argumentInstance);
            }
            else
            {
                return Activator.CreateInstance(type);
            }
        }

        private static object ChangeType(object value, Type targetType)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            Type valueType = value == null ? typeof(object) : value.GetType();

            string message = $"Cannot change type of {(value == null ? "N/A" : value.GetType().Name)} to {targetType.Name}.";
            try
            {
                if (targetType.IsEnum && valueType == typeof(string))
                {
                    try
                    {
                        return Enum.Parse(targetType, (string)value, true);
                    }
                    catch (ArgumentException e)
                    {
                        throw new InvalidCastException(message, e);
                    }
                }
                else if (IsNullableGeneric(targetType))
                {
                    return Activator.CreateInstance(targetType, ChangeType(value, targetType.GetGenericArguments().First()));
                }
                else
                {
                    return Convert.ChangeType(value, targetType, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch (InvalidCastException e)
            {
                throw new InvalidCastException(message, e);
            }
            catch (FormatException e)
            {
                throw new InvalidCastException(message, e);
            }
            catch (OverflowException e)
            {
                throw new InvalidCastException(message, e);
            }
        }

        #endregion Private Methods

        #region Public Methods

        public static IEnumerable<Descriptor> AllFromInstance<T>(T instance)
        {
            return GetDescriptors(typeof(T), instance, 0, 1);
        }

        public static Descriptor[] GetDescriptors(Type type, object target, int level, int maxLevel)
        {
            if ((level > maxLevel) || IsSimple(type) || IsCollection(type))
            {
                return new Descriptor[0];
            }
            else
            {
                if (target == null)
                {
                    try
                    {
                        target = CreateInstance(type);
                    }
                    catch (Exception ex)
                    {
                        throw new TypeInitializationException($"Cannot create instance of type '{type.ToString()}'. You should provide object with all inner instances or to have parameter less constructor for those types.", ex);
                    }
                }

                return type.GetProperties()
                        .Select(p => new Descriptor(p, target, level + 1, maxLevel))
                        .Where(d => !d.IsIgnored)
                        .ToArray();
            }
        }

        public bool IsValueAssignable(object value)
        {
            //   T:System.InvalidCastException:
            //     This conversion is not supported. -or-value is null and conversionType is a value
            //     type.-or-value does not implement the System.IConvertible interface.
            //
            //   T:System.FormatException:
            //     value is not in a format recognized by conversionType.
            //
            //   T:System.OverflowException:
            //     value represents a number that is out of the range of conversionType.
            try
            {
                object collectionvalue;
                if (TryGetAssignableCollectionValue(value, out collectionvalue))
                {
                    return true;
                }
                //if (this.IsCollectionType && this.Type.IsAssignableFrom(value.GetType()))
                //{
                //    return true;
                //}
                ChangeType(value, this.Type);
                return true;
            }
            catch (InvalidCastException)
            {
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }
            return false;
        }

        public bool TryGetAssignableCollectionValue(object value, out object collectionOutValue)
        {
            if (this.IsCollectionType && IsCollection(value.GetType()))
            {
                var collectionValue = value as System.Collections.IEnumerable;

                Type argumentType = this.Type.IsArray ? this.Type.GetElementType() : this.Type.GetGenericArguments().FirstOrDefault();
                Type listType = typeof(List<>).MakeGenericType(argumentType);
                System.Collections.IList result = Activator.CreateInstance(listType) as System.Collections.IList;
                if (argumentType != null && collectionValue != null)
                {
                    foreach (var item in collectionValue)
                    {
                        result.Add(ChangeType(item, argumentType));
                    }
                    if (this.Type.IsGenericType && (this.Type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || this.Type.ImplementsGenericDefinition(typeof(IEnumerable<>))))
                    {
                        var type = typeof(List<>).MakeGenericType(argumentType);
                        collectionOutValue = Activator.CreateInstance(type, result);
                        return true;
                    }
                    else
                    {
                        if (this.Type.IsArray)
                        {
                            Array newArray = Array.CreateInstance(argumentType, result.Count);
                            result.CopyTo(newArray, 0);
                            collectionOutValue = newArray;
                            return true;
                        }
                        else
                        {
                            collectionOutValue = Activator.CreateInstance(this.Type, result);
                            return true;
                        }
                    }
                }
                else
                {
                    //return false;
                }
            }
            else
            {
                //return false;
            }

            collectionOutValue = null;
            return false;
        }

        public void AssignValue(object value)
        {
            string valueString = value == null ? "[NULL]" : value.ToString();
            if (value != null && this.IsCollectionType)
            {
                if (IsCollection(value.GetType()))
                {
                    try
                    {
                        object assignableValue;
                        if (TryGetAssignableCollectionValue(value, out assignableValue))
                        {
                            this.Setter(assignableValue);
                        }
                        else
                        {
                            this.Setter(ChangeType(value, this.Type));
                        }
                        //if (this.Type.IsAssignableFrom(value.GetType()))
                        //{
                        //    this.Setter(value);
                        //}
                        //else
                        //{
                        //    this.Setter(Convert.ChangeType(value, this.Type));
                        //}
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidCastException($"Error during setting non-collection value '{valueString}' to collection property of type '{this.Type.Name}'.", ex);
                    }
                }
                else
                {
                    throw new InvalidCastException($"Cannot set non-collection value '{valueString}' to collection property of type '{this.Type.Name}'.");
                }
            }
            else
            {
                if (this.IsValueAssignable(value))
                {
                    try
                    {
                        this.Setter(ChangeType(value, this.Type));
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidCastException($"Error during setting value '{valueString}' to property of type '{this.Type.Name}'.", ex);
                    }
                }
                else
                {
                    throw new InvalidCastException($"Cannot set value '{valueString}' to property of type '{this.Type.Name}'.");
                }
            }
        }

        public bool EqualsName(string name)
        {
            return string.Compare(name, this.ShortName, true) == 0
                || string.Compare(name, this.LongName, true) == 0;
        }

        public static IEnumerable<Descriptor> Flatten(IEnumerable<Descriptor> descriptors)
        {
            return descriptors.Concat(descriptors.SelectMany(d => Flatten(d.Descriptors)));
        }

        #endregion Public Methods
    }
}