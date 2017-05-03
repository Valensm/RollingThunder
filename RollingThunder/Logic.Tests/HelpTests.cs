using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Wly.RollingThunder
{
    [TestClass]
    public class HelpTests
    {
        [TestMethod]
        [TestCategory("Help")]
        public void HelpBoolProperty()
        {
            string[] args = "-p1 22 -helpme -p2 33".ToArgs();
            var result = new Parser<TH01>(() => new TH01()).Parse(args);
            Assert.AreEqual(true, result.HelpMe, "help");
            Assert.AreEqual(0, result.P1, "p1");
            Assert.AreEqual(null, result.P2, "p2");
        }

        [TestMethod]
        [TestCategory("Help")]
        public void HelpStringProperty()
        {
            string[] args = "-p1 22 -h blah -p2 33".ToArgs();
            var result = new Parser<TH02>(() => new TH02()).Parse(args);
            Assert.AreEqual("blah", result.HelpMe);
            Assert.AreEqual(0, result.P1, "p1");
            Assert.AreEqual(null, result.P2, "p2");
        }

        [TestMethod]
        [TestCategory("Help")]
        public void HelpIntProperty()
        {
            string[] args = "-p1 22 --help 10 -p2 33".ToArgs();
            var result = new Parser<TH03>(() => new TH03()).Parse(args);
            Assert.AreEqual(10, result.HelpMe);
            Assert.AreEqual(0, result.P1, "p1");
            Assert.AreEqual(null, result.P2, "p2");
        }

        [TestMethod]
        [TestCategory("Help")]
        [ExpectedException(typeof(HelpException))]
        public void HelpException()
        {
            string[] args = "-p1 22 -helpme -p2 33".ToArgs();
            var result = new Parser<TH01>(() => new TH01(), new ParserConfiguration() { ThrowHelpException = true }).Parse(args);
        }

        [TestMethod]
        [TestCategory("Help")]
        [ExpectedException(typeof(HelpException))]
        public void MultipleHelpOptionsWithValue()
        {
            string[] args = "-p1 22 --helpme 5 -p2 33".ToArgs();
            var result = new Parser<TH04>(() => new TH04(), new ParserConfiguration() { ThrowHelpException = true }).Parse(args);
        }

        [TestMethod]
        [TestCategory("Help")]
        [ExpectedException(typeof(UnassignableParameterException))]
        public void MultipleHelpOptionsWrongValue()
        {
            string[] args = "-p1 22 --helpme a -p2 33".ToArgs();
            var result = new Parser<TH04>(() => new TH04(), new ParserConfiguration() { ThrowHelpException = true }).Parse(args);
        }

        [TestMethod]
        [TestCategory("Help")]
        [ExpectedException(typeof(HelpException))]
        public void MultipleHelpOptionsBool()
        {
            string[] args = "-p1 22 -h -p2 33".ToArgs();
            var result = new Parser<TH04>(() => new TH04(), new ParserConfiguration() { ThrowHelpException = true }).Parse(args);
        }

        [TestMethod]
        [TestCategory("Help")]
        [ExpectedException(typeof(HelpException))]
        public void InvalidHelpCommandLine()
        {
            string[] args = "blah -we 22 -helpme -x2 33 --bar".ToArgs();
            var result = new Parser<TH01>(() => new TH01(), new ParserConfiguration() { ThrowHelpException = true }).Parse(args);
        }
    }

    public class TH01
    {
        public int P1 { get; set; }

        public string P2 { get; set; }

        [HelpOption]
        public bool HelpMe { get; set; }
    }

    public class TH02
    {
        public int P1 { get; set; }

        public string P2 { get; set; }

        [HelpOption]
        [Name("h", "help")]
        public string HelpMe { get; set; }
    }

    public class TH03
    {
        public int P1 { get; set; }

        public string P2 { get; set; }

        [HelpOption]
        [Name("h", "help")]
        public int HelpMe { get; set; }
    }

    internal class TH04
    {
        [HelpOption]
        [Name("h", "help")]
        public bool Help1 { get; set; }

        [HelpOption]
        [Name("?", "helpme")]
        public int Help2 { get; set; }

        public int P1 { get; set; }

        public string P2 { get; set; }

        [DefaultVerb]
        public int V1 { get; set; }
    }
}