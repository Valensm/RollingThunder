using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Wly.RollingThunder
{
    [TestClass]
    public class MutualGroupTests
    {
        [TestMethod]
        [TestCategory("Mutual")]
        public void MultipleGroups_Pass()
        {
            string[] args = "-p1 22 -p2 33".ToArgs();
            var result = new Parser<MG1>(() => new MG1()).Parse(args);
            Assert.AreEqual(22, result.P1, "Value P1");
            Assert.AreEqual(33, result.P2, "Value P2");
        }

        [TestMethod]
        [TestCategory("Mutual")]
        [ExpectedException(typeof(MutualGroupArgumentsException))]
        public void MultipleGroups_Fail()
        {
            string[] args = "-p1 22 -p2 33 -p3 44".ToArgs();
            var result = new Parser<MG1>(() => new MG1()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Mutual")]
        public void InnerGroupIsInactive_Pass()
        {
            string[] args = "-p1 22 -p2 33 -In1 -In2".ToArgs();
            var result = new Parser<MG1>(() => new MG1()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Mutual")]
        [ExpectedException(typeof(MutualGroupArgumentsException))]
        public void InnerGroupIsInactive_Fail()
        {
            string[] args = "-p1 22 -p2 33 -In1 -px1 44".ToArgs();
            var result = new Parser<MG1>(() => new MG1()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Mutual")]
        public void InnerGroupIsActive_Pass()
        {
            string[] args = "-p1 22 -p2 33 -In1 -px3 123 -In2".ToArgs();
            var result = new Parser<MG1>(() => new MG1()).Parse(args);
        }
    }

    internal class MG1
    {
        [MutualGroup("G1")]
        public int P1 { get; set; }

        [MutualGroup("G2")]
        public int P2 { get; set; }

        [MutualGroup("G1")]
        public int P3 { get; set; }

        public int P4 { get; set; }

        public MG2 In1 { get; set; }

        public MG3 In2 { get; set; }
    }

    internal class MG2
    {
        [MutualGroup("G1")]
        public int PX1 { get; set; }

        [MutualGroup("G2")]
        public int PX2 { get; set; }

        [MutualGroup("G3")]
        public int PX3 { get; set; }
    }

    internal class MG3
    {
        [MutualGroup("G1")]
        public int PY1 { get; set; }

        [MutualGroup("G2")]
        public int PY2 { get; set; }

        [MutualGroup("G3")]
        public int PY3 { get; set; }
    }
}