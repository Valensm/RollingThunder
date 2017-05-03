using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Wly.RollingThunder
{
    [TestClass]
    public class ParseEmptyLine
    {
        [TestMethod]
        [TestCategory("Parse")]
        [ExpectedException(typeof(RequiredParameterException))]
        public void RequiredSimple()
        {
            var args = "".ToArgs();
            var result = new Parser<PEL1>(() => new PEL1()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Parse")]
        [ExpectedException(typeof(RequiredParameterException))]
        public void RequiredInner()
        {
            var args = "".ToArgs();
            var result = new Parser<PEL2>(() => new PEL2()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Parse")]
        public void NotRequiredInnerRequired()
        {
            var args = "".ToArgs();
            var result = new Parser<PEL3>(() => new PEL3()).Parse(args);
        }
    }

    internal class PEL1
    {
        [Name("pp")]
        [Required("Is required '{0}'.")]
        public string P1 { get; set; }
    }

    internal class PEL2
    {
        public int P1 { get; set; }

        public int P2 { get; set; }

        [Required]
        public PEL1 P3 { get; set; }
    }

    internal class PEL3
    {
        public int P1 { get; set; }

        public int P2 { get; set; }

        public PEL1 P3 { get; set; }
    }
}