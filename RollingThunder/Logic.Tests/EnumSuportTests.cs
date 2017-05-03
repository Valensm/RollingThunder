using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Wly.RollingThunder
{
    [TestClass]
    public class EnumSuportTests
    {
        [TestMethod]
        [TestCategory("Enum")]
        public void EnumParameter()
        {
            var args = "-p1 foo -p2 good".ToArgs();
            var result = new Parser<ESTC1>(() => new ESTC1()).Parse(args);
            Assert.AreEqual(Enum1.Good, result.P2, "P2");
            Assert.AreEqual("foo", result.P1, "P1");
        }

        [TestMethod]
        [TestCategory("Enum")]
        public void NullableEnumParameter()
        {
            var args = "-p1 foo -p4 good".ToArgs();
            var result = new Parser<ESTC1>(() => new ESTC1()).Parse(args);
            Assert.AreEqual(Enum1.Good, result.P4, "P4");
            Assert.AreEqual("foo", result.P1, "P1");
        }

        [TestMethod]
        [TestCategory("Enum")]
        [ExpectedException(typeof(InvalidArgumentTypeException))]
        public void EnumParameter_Fail()
        {
            var args = "-p1 foo -p2 blah".ToArgs();
            var result = new Parser<ESTC1>(() => new ESTC1()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Enum")]
        public void EnumCollectionParameter()
        {
            var args = "-px foo -p3 percent10 percent20 percent90 -p1 boo".ToArgs();
            var result = new Parser<ESTC1>(() => new ESTC1()).Parse(args);
            Assert.IsTrue(Enumerable.SequenceEqual(result.P3.ToArray(), new Enum1[] { Enum1.Percent10, Enum1.Percent20, Enum1.Percent90 }), "P3");
            Assert.AreEqual("foo", result.Px, "Px");
            Assert.AreEqual("boo", result.P1, "P1");
        }

        [TestMethod]
        [TestCategory("Enum")]
        public void EnumVerb()
        {
            var args = "good -p1 foo -p2 blah".ToArgs();
            var result = new Parser<ESTC2>(() => new ESTC2()).Parse(args);
            Assert.AreEqual(Enum1.Good, result.V1, "V1");
            Assert.AreEqual("foo", result.P1, "P1");
            Assert.AreEqual("blah", result.P2, "P2");
        }

        [TestMethod]
        [TestCategory("Enum")]
        public void EnumVerbCollection()
        {
            var args = "good percent10 percent90 unknown -p1 foo -p2 blah".ToArgs();
            var result = new Parser<ESTC3>(() => new ESTC3()).Parse(args);
            Assert.IsTrue(Enumerable.SequenceEqual(result.Verbs.ToArray(), new Enum1[] { Enum1.Good, Enum1.Percent10, Enum1.Percent90, Enum1.Unknown }), "Vebrs");
            Assert.AreEqual("foo", result.P1, "P1");
            Assert.AreEqual("blah", result.P2, "P2");
        }
    }

    public class ESTC1
    {
        public string P1 { get; set; }

        public Enum1 P2 { get; set; }

        public string Px { get; set; }

        public IEnumerable<Enum1> P3 { get; set; }

        public Enum1? P4 { get; set; }
    }

    internal class ESTC2
    {
        public string P1 { get; set; }

        public string P2 { get; set; }

        [DefaultVerb]
        public Enum1 V1 { get; set; }
    }

    internal class ESTC3
    {
        public string P1 { get; set; }

        public string P2 { get; set; }

        [VerbBag]
        public IEnumerable<Enum1> Verbs { get; set; }
    }
}