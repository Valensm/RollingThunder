using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Wly.RollingThunder
{
    [TestClass]
    public class HelpScreenTests
    {
        [TestMethod]
        [TestCategory("Help Screen")]
        public void ParametersCommandLine()
        {
            string line = HelpsScreen<HS01>.GetCommandLineHelpLine(HelpsScreen<HS01>.GetHelpData(() => new HS01(), new HelpConfiguration(), "", new ParserConfiguration()), new HelpConfiguration(), new ParserConfiguration());
            Assert.AreEqual("Wly.RollingThunder.Logic.Tests.exe -P3|--P3 -P5|--P5 <P5> [-a|--aaa <P2>] [-P1|--P1 <P1>] [-P4|--P4 <P4 1> <P4 2> <P4 3> ...] [-IsHelp|--IsHelp]", line);
        }

        [TestMethod]
        [TestCategory("Help Screen")]
        public void VerbsCommandLine()
        {
            string line = HelpsScreen<HS02>.GetCommandLineHelpLine(HelpsScreen<HS02>.GetHelpData(() => new HS02(), new HelpConfiguration(), "", new ParserConfiguration()), new HelpConfiguration(), new ParserConfiguration());
            Assert.AreEqual("Wly.RollingThunder.Logic.Tests.exe <P3> <P5> [<IsHelp>] [<P1>] [<P2>] [<P4 1> <P4 2> <P4 3> ...]", line);
        }

        [TestMethod]
        [TestCategory("Help Screen")]
        public void VerbsEnumCommandLine()
        {
            string line = HelpsScreen<HS05>.GetCommandLineHelpLine(HelpsScreen<HS05>.GetHelpData(() => new HS05(), new HelpConfiguration(), "", new ParserConfiguration()), new HelpConfiguration(), new ParserConfiguration());
            Assert.AreEqual("Wly.RollingThunder.Logic.Tests.exe {Fine|Good|Poor}", line);
        }

        [TestMethod]
        [TestCategory("Help Screen")]
        public void EnumCommandLine()
        {
            string line = HelpsScreen<HS04>.GetCommandLineHelpLine(HelpsScreen<HS04>.GetHelpData(() => new HS04(), new HelpConfiguration(), "", new ParserConfiguration()), new HelpConfiguration(), new ParserConfiguration());
            Assert.AreEqual("Wly.RollingThunder.Logic.Tests.exe [-B1|--B1 <B1>] [-B2|--B2 <B2>] [-B3|--B3 <B3>] [-E1|--E1 {Fine|Good|Poor}]", line);
        }

        [TestMethod]
        [TestCategory("Help Screen")]
        public void AlternativesCommandLine()
        {
            string line = HelpsScreen<HS03>.GetCommandLineHelpLine(HelpsScreen<HS03>.GetHelpData(() => new HS03(), new HelpConfiguration(), "", new ParserConfiguration()), new HelpConfiguration(), new ParserConfiguration());
            Assert.AreEqual("Wly.RollingThunder.Logic.Tests.exe [-A1|--A1 (-P3|--P3 -P5|--P5 <P5> [-a|--aaa <P2>] [-P1|--P1 <P1>] [-P4|--P4 <P4 1> <P4 2> <P4 3> ...] [-IsHelp|--IsHelp])] [-A2|--A2 ([-B1|--B1 <B1>] [-B2|--B2 <B2>] [-B3|--B3 <B3>] [-E1|--E1 {Fine|Good|Poor}])]", line);
        }

        [TestMethod]
        [TestCategory("Help Screen")]
        public void FulHelpScreen()
        {
            var helpConfiguration = new HelpConfiguration()
            {
                ApplicationName = "My App",
                ApplicationVersion = "2.1.5",
                Description = "This is description",
                DescriptionHeader = "Details",
                ErrorHeader = "Errors",
                ParametersHeader = "More",
                UsageHeader = "Usage",
                ApplicationExecutable = "MyApp",
                ApplicationCopyright = "Copyright"
            };
            var lines = HelpsScreen<HS03>.GetHelpScreenLines(HelpsScreen<HS03>.GetHelpData(() => new HS03(), helpConfiguration, "", new ParserConfiguration())).ToArray();
            string helpScreen = string.Join(Environment.NewLine, lines);

            string expectedScreen = @"My App 2.1.5 Copyright

Details
    This is description

Usage
    MyApp [-A1|--A1 (-P3|--P3 -P5|--P5 <P5> [-a|--aaa <P2>] [-P1|--P1 <P1>] [-P4|--P4 <P4 1> <P4 2> <P4 3> ...] [-IsHelp|--IsHelp])] [-A2|--A2 ([-B1|--B1 <B1>] [-B2|--B2 <B2>] [-B3|--B3 <B3>] [-E1|--E1 {Fine|Good|Poor}])]

More
    -a|--aaa         - Optional.
    -A1|--A1         - Option 1. Enables/unlocks use of following parameters: -P1|--P1, -a|--aaa, -P3|--P3, -P4|--P4, -P5|--P5, -IsHelp|--IsHelp. Optional.
    -A2|--A2         - Second option. Enables/unlocks use of following parameters: -B1|--B1, -B2|--B2, -B3|--B3, -E1|--E1. Optional.
    -B1|--B1         - Mutual groups: Group1. Optional.
    -B2|--B2         - Optional.
    -B3|--B3         - Mutual groups: Group1, Group2. Optional.
    -E1|--E1         - Possible values: Fine, Good, Poor. Optional.
    -IsHelp|--IsHelp - This help screen. Optional.
    -P1|--P1         - Optional.
    -P3|--P3         - Switch, no value required. Required.
    -P4|--P4         - Collection which accepts at least 0 and at maximum 2147483647 values. Optional.
    -P5|--P5         - Required.";

            Assert.AreEqual(expectedScreen, helpScreen);
        }
    }

    public class HS01
    {
        public int P1 { get; set; }

        [Name("a", "aaa")]
        public string P2 { get; set; }

        [Required]
        public bool P3 { get; set; }

        public IEnumerable<int> P4 { get; set; }

        [Required]
        public double P5 { get; set; }

        [HelpOption]
        public bool IsHelp { get; set; }
    }

    public class HS02
    {
        [Verb]
        public int P1 { get; set; }

        [Name("a", "aaa")]
        [Verb]
        public string P2 { get; set; }

        [Required]
        [Verb]
        public bool P3 { get; set; }

        [VerbBag]
        public IEnumerable<int> P4 { get; set; }

        [Required]
        [DefaultVerb]
        public double P5 { get; set; }

        [HelpOption]
        [Verb]
        public bool IsHelp { get; set; }
    }

    public class HS03
    {
        [HelpText("Option 1.")]
        public HS01 A1 { get; set; }

        [HelpText("Second option.")]
        public HS04 A2 { get; set; }
    }

    public class HS04
    {
        [MutualGroup("Group1")]
        public int B1 { get; set; }

        public int B2 { get; set; }

        [MutualGroup("Group1")]
        [MutualGroup("Group2")]
        public int B3 { get; set; }

        public HSE01 E1 { get; set; }
    }

    public enum HSE01
    {
        Fine, Good, Poor
    }

    internal class HS05
    {
        [Required]
        [Verb]
        public HSE01 Action { get; set; }
    }
}