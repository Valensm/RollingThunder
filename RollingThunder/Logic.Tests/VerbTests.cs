using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wly.RollingThunder
{
    [TestClass]
    public class VerbTests
    {
        [TestMethod]
        [TestCategory("Verb")]
        public void VerbBag()
        {
            string[] args = "r a b".ToArgs();
            var result = new Parser<V1>(() => new V1()).Parse(args);
            Assert.AreEqual(true, Enumerable.SequenceEqual(args, result.P1), "Values");
        }

        [TestMethod]
        [TestCategory("Verb")]
        public void DefaultVerb()
        {
            string[] args = "1".ToArgs();
            var result = new Parser<V2>(() => new V2()).Parse(args);
            Assert.AreEqual(1, result.P1, "Value");
        }

        [TestMethod]
        [TestCategory("Verb")]
        public void DefaultVerbAndBag()
        {
            string[] args = "100 a b c".ToArgs();
            var result = new Parser<V3>(() => new V3()).Parse(args);
            Assert.AreEqual(true, Enumerable.SequenceEqual(new string[] { "a", "b", "c" }, result.P1), "Values");
            Assert.AreEqual(100, result.P2, "Value");
        }

        [TestMethod]
        [TestCategory("Verb")]
        public void BoolVerbsAndBag()
        {
            string[] args = "100 a b c p2 test".ToArgs();
            var result = new Parser<V4>(() => new V4()).Parse(args);
            Assert.AreEqual(true, Enumerable.SequenceEqual(new string[] { "100", "a", "b", "c" }, result.P1), "Values");
            Assert.AreEqual(true, result.P2, "Value P2");
            Assert.AreEqual(false, result.Pokus, "Value Pokus");
        }

        [TestMethod]
        [TestCategory("Verb")]
        public void BoolVerbsAndBagConflict()
        {
            string[] args = "pokus test 1 1.5 best -p3 6".ToArgs();
            var result = new Parser<V5>(() => new V5()).Parse(args);
            Assert.AreEqual(true, Enumerable.SequenceEqual(new string[] { "1.5", "best" }, result.P1), "Values");
            Assert.AreEqual(1, result.P2, "Value P2");
            Assert.AreEqual(true, result.Test, "Value Test");
            Assert.AreEqual(true, result.Pokus, "Value Pokus");
        }
    }

    internal class V1
    {
        [VerbBag]
        public IEnumerable<string> P1 { get; set; }

        public V1()
        {
            this.P1 = new string[0];
        }
    }

    internal class V2
    {
        [DefaultVerb]
        public int P1 { get; set; }
    }

    internal class V3
    {
        [VerbBag]
        public IEnumerable<string> P1 { get; set; }

        [DefaultVerb]
        public int P2 { get; set; }

        public V3()
        {
            this.P1 = new string[0];
        }
    }

    internal class V4
    {
        [VerbBag]
        public IEnumerable<string> P1 { get; set; }

        [DefaultVerb]
        public bool P2 { get; set; }

        [Verb]
        public bool Test { get; set; }

        [Verb]
        public bool Pokus { get; set; }

        public V4()
        {
            this.P1 = new string[0];
        }
    }

    internal class V5
    {
        [VerbBag]
        public IEnumerable<string> P1 { get; set; }

        [DefaultVerb]
        public int P2 { get; set; }

        [Verb]
        public bool Test { get; set; }

        [Verb]
        public bool Pokus { get; set; }

        public int P3 { get; set; }

        public V5()
        {
            this.P1 = new string[0];
        }
    }
}