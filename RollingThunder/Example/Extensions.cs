using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    internal static class Extensions
    {
        #region Public Methods

        public static void Print(this object o)
        {
            Console.Out.WriteLine(o.GetValuesString());
        }

        public static void Print(this System.IO.TextWriter writer, object subject)
        {
            writer.WriteLine(subject.GetValuesString());
        }

        public static string GetValuesString(this object o, int indentLevel = 0)
        {
            string indent = String.Join("", Enumerable.Repeat("    ", indentLevel));
            return string.Join(
                Environment.NewLine,
                o.GetType()
                    .GetProperties()
                    .Select(p =>
                    {
                        object value = p.GetValue(o);
                        if (value == null)
                        {
                            return $"{indent}{p.Name} = <NULL>";
                        }
                        else if (p.PropertyType == typeof(string) || !p.PropertyType.IsClass)
                        {
                            return $"{indent}{p.Name} = {value.ToString()}";
                        }
                        else
                        {
                            return $"{indent}{p.Name}:{Environment.NewLine}{value.GetValuesString(indentLevel + 1)}";
                        }
                    })
               );
        }

        #endregion Public Methods
    }
}