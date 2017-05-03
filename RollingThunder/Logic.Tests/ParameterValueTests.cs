using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wly.RollingThunder
{
    [TestClass]
    public class ParameterValueTests
    {
        [TestMethod]
        [TestCategory("Parameters")]
        public void Empty()
        {
            var result = "".ToArgs().Parse<PVT01>();
        }

        [TestMethod]
        [TestCategory("Parameters")]
        public void Basic()
        {
            var result = "  -p10 10 20  -p8 1.8 1.9 2.4   -p7 1.2 1.3 1.4  -p5 5.5 7 1.2 -p4 5 6 7 -p3 ahoj -p2 1.5 -p1 10   ".ToArgs().Parse<PVT01>();
            Assert.AreEqual(10, result.P1, "P1");
            Assert.AreEqual(1.5, result.P2, "P2");
            Assert.AreEqual("ahoj", result.P3, "P3");
            Assert.IsTrue(Enumerable.SequenceEqual(result.P4, new int[] { 5, 6, 7 }), "P4");
            Assert.IsTrue(Enumerable.SequenceEqual(result.P5, new double[] { 5.5, 7, 1.2 }), "P5");
            Assert.IsTrue(Enumerable.SequenceEqual(result.P7, new float[] { 1.2F, 1.3F, 1.4F }), "P7");
            Assert.IsTrue(Enumerable.SequenceEqual(result.P8, new decimal[] { 1.8m, 1.9m, 2.4m }), "P8");
            Assert.IsTrue(Enumerable.SequenceEqual(result.P10.ToArray(), new int[] { 10, 20 }), "P10");
        }

        [TestMethod]
        [TestCategory("Parameters")]
        public void InnerSimple()
        {
            var result = "-in1 -p1 10 -i1 50 -s1 text".ToArgs().Parse<PVT01>();
            Assert.AreEqual(10, result.P1, "P1");
            Assert.AreEqual(50, result.In1.I1, "Set1.I1");
            Assert.AreEqual("text", result.In1.S1, "Set1.S1");
        }

        [TestMethod]
        [TestCategory("Parameters")]
        public void InnerSimpleTypes()
        {
            var result = " -S2 blah -in1 -p1 10 -in2 -i1 50 -s1 text".ToArgs().Parse<PVT01>();
            Assert.AreEqual(result.P1, 10, "P1");
            Assert.AreEqual(result.In1.I1, 50, "Set1.I1");
            Assert.AreEqual(result.In1.S1, "text", "Set1.S1");
            Assert.AreEqual(result.In2.S2, "blah", "Set2.S2");
        }

        [TestMethod]
        [TestCategory("Parameters")]
        [ExpectedException(typeof(ArgumentNotSupportedException))]
        public void InnerSimpleNegative()
        {
            var result = "-in2 -p1 10 -i1 50 -s1 text".ToArgs().Parse<PVT01>();
        }

        [TestMethod]
        [TestCategory("Parameters")]
        public void InnerSimpleRequired()
        {
            var result = "-in3 -p1 10 -i3 50 -s3 text".ToArgs().Parse<PVT01>();
            Assert.AreEqual(10, result.P1, "P1");
            Assert.AreEqual(50, result.In3.I3, "Set3.I3");
            Assert.AreEqual("text", result.In3.S3, "Set3.S3");
        }

        [TestMethod]
        [TestCategory("Parameters")]
        [ExpectedException(typeof(RequiredParameterException))]
        public void InnerSimpleRequiredNegative()
        {
            var result = "-in3 -p1 10 -string3 text".ToArgs().Parse<PVT01>();
        }

        [TestMethod]
        [TestCategory("Parameters")]
        [ExpectedException(typeof(AmbiguousNameDefinitionException))]
        public void ShortNameConflictNegative()
        {
            var result = "".ToArgs().Parse<Set4>();
        }

        [TestMethod]
        [TestCategory("Parameters")]
        [ExpectedException(typeof(AmbiguousNameDefinitionException))]
        public void LongNameConflictNegative()
        {
            var result = "".ToArgs().Parse<Set5>();
        }

        [TestMethod]
        [TestCategory("Parameters")]
        [ExpectedException(typeof(DuplicateArgumentsException))]
        public void ArgConflictNegative()
        {
            var result = "-p1 11 -p2 22 -p1 33".ToArgs().Parse<PVT01>();
        }
    }

    internal class PVT01
    {
        public int P1 { get; set; }

        public double P2 { get; set; }

        public string P3 { get; set; }

        public IEnumerable<int> P4 { get; set; }

        public IEnumerable<double> P5 { get; set; }

        public IEnumerable<DateTime> P6 { get; set; }

        public List<float> P7 { get; set; }

        public IList<decimal> P8 { get; set; }

        public int[] P9 { get; set; }

        public IList<int> P10 { get; set; }

        public IDictionary<string, int> P11 { get; set; }

        public Dictionary<string, decimal> P12 { get; set; }

        public Set1 In1 { get; set; }

        public Set2 In2 { get; set; }

        public Set3 In3 { get; set; }
    }

    internal class Set1
    {
        public int I1 { get; set; }

        public string S1 { get; set; }
    }

    internal class Set2
    {
        public string S2 { get; set; }

        public int I2 { get; set; }
    }

    internal class Set3
    {
        [Name("S3", "String3")]
        public string S3 { get; set; }

        [Required]
        public int I3 { get; set; }
    }

    internal class Set4
    {
        [Name("I3", "String3")]
        public string S3 { get; set; }

        [Name("I3", "Int3")]
        public int I3 { get; set; }
    }

    internal class Set5
    {
        [Name("S3", "String3")]
        public string S3 { get; set; }

        [Name("I3", "String3")]
        public int I3 { get; set; }
    }
}