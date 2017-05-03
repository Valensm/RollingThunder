using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    public static class Extensions
    {
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Ctors

        #endregion Ctors

        #region Private Methods

        #endregion Private Methods

        #region Public Methods

        internal static object GetDefaultValue(this Type type)
        {
            if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
            {
                return Activator.CreateInstance(type);
            }
            else
            {
                return null;
            }
        }

        internal static TResult FirstOrDefaultOf<TResult, TSource>(this IEnumerable<TSource> data)
            where TSource : class
            where TResult : class
        {
            return data.Where(a => a is TResult).Select(a => a as TResult).FirstOrDefault();
        }

        internal static bool Implements<TInterface>(this Type type)
        {
            return typeof(TInterface).IsAssignableFrom(type);
        }

        internal static bool Implements<TInterface>(this object target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            return typeof(TInterface).IsAssignableFrom(target.GetType());
        }

        internal static bool ImplementsGenericDefinition(this Type targetType, Type genericInterfaceType)
        {
            if (genericInterfaceType == null)
            {
                throw new ArgumentNullException(nameof(genericInterfaceType));
            }
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }
            if (!genericInterfaceType.IsGenericType)
            {
                throw new ArgumentException("Must be generic type", nameof(genericInterfaceType));
            }
            if (!genericInterfaceType.IsGenericTypeDefinition)
            {
                throw new ArgumentException("Must be generic type definition", nameof(genericInterfaceType));
            }
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == genericInterfaceType)
            {
                return true;
            }
            else
            {
                return targetType.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == genericInterfaceType);
            }
        }

        public static T Parse<T>(this string[] args, Func<T> instanceFactory, ParserConfiguration configuration)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return new Parser<T>(instanceFactory, configuration).Parse(args);
        }

        public static T Parse<T>(this string[] args, ParserConfiguration configuration)
            where T : new()
            => args.Parse<T>(() => new T(), configuration);

        public static T Parse<T>(this string[] args)
            where T : new()
            => args.Parse<T>(() => new T(), new ParserConfiguration());

        public static T Parse<T>(this string[] args, Func<T> instanceFactory)
            => args.Parse<T>(instanceFactory, new ParserConfiguration());

        public static void WriteHelpScreen<T>(this System.IO.TextWriter writer, Func<T> instanceFactory, string error, HelpConfiguration helpConfiguration, ParserConfiguration configuration)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            new Parser<T>(instanceFactory, configuration).HelpScreen(error, helpConfiguration).Write(writer);
        }

        public static void WriteHelpScreen<T>(this System.IO.TextWriter writer, Func<T> instanceFactory, string error, HelpConfiguration helpConfiguration)
            => writer.WriteHelpScreen<T>(instanceFactory, error, helpConfiguration, new ParserConfiguration());

        public static void WriteHelpScreen<T>(this System.IO.TextWriter writer, Func<T> instanceFactory, string error)
           => writer.WriteHelpScreen<T>(instanceFactory, error, new HelpConfiguration());

        public static void WriteHelpScreen<T>(this System.IO.TextWriter writer, Func<T> instanceFactory)
            => writer.WriteHelpScreen<T>(instanceFactory, string.Empty);

        public static void WriteHelpScreen<T>(this System.IO.TextWriter writer, string error, HelpConfiguration helpConfiguration, ParserConfiguration configuration)
            where T : new()
            => WriteHelpScreen<T>(writer, () => new T(), error, helpConfiguration, configuration);

        public static void WriteHelpScreen<T>(this System.IO.TextWriter writer, string error, HelpConfiguration helpConfiguration)
             where T : new()
            => writer.WriteHelpScreen<T>(error, helpConfiguration, new ParserConfiguration());

        public static void WriteHelpScreen<T>(this System.IO.TextWriter writer, string error)
            where T : new()
            => writer.WriteHelpScreen<T>(error, new HelpConfiguration());

        public static void WriteHelpScreen<T>(this System.IO.TextWriter writer)
            where T : new()
            => writer.WriteHelpScreen<T>(string.Empty);

        public static void Write(this IHelpScreen helpScreen, System.IO.TextWriter writer)
        {
            if (helpScreen == null)
            {
                throw new ArgumentNullException(nameof(helpScreen));
            }

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (helpScreen.Lines != null)
            {
                foreach (string line in helpScreen.Lines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        #endregion Public Methods
    }
}