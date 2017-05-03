using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wly.RollingThunder;

namespace Example
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                try
                {
                    ServiceSettings settings = args.Parse<ServiceSettings>(new ParserConfiguration() { ThrowHelpException = true });

                    Console.Out.WriteLine("Performing operation with following settings:");
                    Console.Out.Print(settings);
                }
                catch (HelpException)
                {
                    Console.Error.WriteHelpScreen<ServiceSettings>();
                }
                catch (ParserException e)
                {
                    Console.Error.WriteHelpScreen<ServiceSettings>(e.Message);
                }
            }
            catch (Exception ex)
            {
                #region General Exception Handling

                Console.Error.WriteLine("**************************  E X C E P T I O N  **************************");
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.ToString());
                Console.Error.WriteLine("*************************************************************************");

                #endregion General Exception Handling
            }

#if DEBUG
            Console.Out.WriteLine();
            Console.Out.WriteLine("Press <ENTER> for exit.");
            Console.In.ReadLine();
#endif
        }
    }
}