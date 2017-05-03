using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using System.Linq;

namespace Wly.RollingThunder
{
    [TestClass]
    public class ParseMatchTests
    {
        [TestMethod]
        [TestCategory("Match")]
        public void DefaultVerb()
        {
            var args = "p1".ToArgs();
            var result = new Parser<PMT1>(() => new PMT1()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Match")]
        [ExpectedException(typeof(ArgumentNotSupportedException))]
        public void SingleVerbMissingArg()
        {
            var args = "1 -p".ToArgs();
            var result = new Parser<PMT2>(() => new PMT2()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Match")]
        [ExpectedException(typeof(NoDefaultVerbOrBagNotFoundException))]
        public void MissingDefaultVerb()
        {
            var args = "a".ToArgs();
            var result = new Parser<PMT3>(() => new PMT3()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Match")]
        [ExpectedException(typeof(NoVerbBagFoundException))]
        public void MissingVerbBag()
        {
            var args = "a b".ToArgs();
            var result = new Parser<PMT3>(() => new PMT3()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Match")]
        [ExpectedException(typeof(MultipleDefaultVerbDefinitionsException))]
        public void MultipleDefaultVerbs()
        {
            var args = "a".ToArgs();
            var result = new Parser<PMT4>(() => new PMT4()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Match")]
        [ExpectedException(typeof(MultipleVerbBagDefinitionsException))]
        public void MultipleVerbBags()
        {
            var args = "a b".ToArgs();
            var result = new Parser<PMT5>(() => new PMT5()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Match")]
        public void VerbAndArg()
        {
            var args = "1,5 -p2 3".ToArgs();
            var result = new Parser<PMT2>(() => new PMT2()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Match")]
        [ExpectedException(typeof(AmbiguousParameterException))]
        public void MultipleArgDefs()
        {
            var args = "p1 -p1".ToArgs();
            var result = new Parser<PMT6>(() => new PMT6()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Match")]
        public void VerbAndArgWithOptional()
        {
            var args = "7 -p2 1".ToArgs();
            var result = new Parser<PMT7>(() => new PMT7()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Match")]
        [ExpectedException(typeof(ArgumentNotSupportedException))]
        public void VerbAndArgIgnoredWithOptional()
        {
            var args = "a -p2".ToArgs();
            var result = new Parser<PMT8>(() => new PMT8()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Match")]
        public void VerbBag()
        {
            var args = "12".ToArgs();
            var result = new Parser<PMT9>(() => new PMT9()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Match")]
        public void SingleArg()
        {
            var args = "-p1 2".ToArgs();
            var result = new Parser<PMT3>(() => new PMT3()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Match")]
        public void VerbAndVerbBag()
        {
            var args = "1 -p3 2".ToArgs();
            var result = new Parser<PMT10>(() => new PMT10()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Match")]
        [ExpectedException(typeof(NonBoolVerbsException))]
        public void NonBoolVerb()
        {
            var args = "1 -p3 2".ToArgs();
            var result = new Parser<PMT11>(() => new PMT11()).Parse(args);
        }
    }

    internal class PMT1
    {
        [DefaultVerb]
        public bool P1 { get; set; }
    }

    internal class PMT2
    {
        [DefaultVerb]
        public decimal P1 { get; set; }

        public int P2 { get; set; }
    }

    internal class PMT3
    {
        public int P1 { get; set; }
    }

    internal class PMT4
    {
        [DefaultVerb]
        public int P1 { get; set; }

        [DefaultVerb]
        public int P2 { get; set; }
    }

    internal class PMT5
    {
        [VerbBag]
        public string[] P1 { get; set; }

        [VerbBag]
        public string[] P2 { get; set; }

        public PMT5()
        {
            this.P1 = new string[0];
            this.P2 = new string[0];
        }
    }

    internal class PMT6
    {
        [DefaultVerb]
        public bool P1 { get; set; }

        [Name("p1")]
        public double P2 { get; set; }
    }

    internal class PMT7
    {
        [DefaultVerb]
        public int P1 { get; set; }

        public int P2 { get; set; }

        public int P3 { get; set; }
    }

    internal class PMT8
    {
        [DefaultVerb]
        public string P1 { get; set; }

        [Ignore]
        public int P2 { get; set; }

        public int P3 { get; set; }
    }

    internal class PMT9
    {
        [VerbBag]
        public int[] P1 { get; set; }

        [Ignore]
        public int P2 { get; set; }

        public int P3 { get; set; }

        public PMT9()
        {
            this.P1 = new int[0];
        }
    }

    internal class PMT10
    {
        [VerbBag]
        public int[] P1 { get; set; }

        [Verb]
        public bool P2 { get; set; }

        public bool P3 { get; set; }

        public PMT10()
        {
            this.P1 = new int[0];
        }
    }

    internal class PMT11
    {
        [VerbBag]
        public int[] P1 { get; set; }

        [Verb]
        public int P2 { get; set; }

        public bool P3 { get; set; }

        public PMT11()
        {
            this.P1 = new int[0];
        }
    }
}