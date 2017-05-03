using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Wly.RollingThunder
{
    [TestClass]
    public class ParseBasic
    {
        [TestMethod]
        [TestCategory("Parse")]
        public void Empty()
        {
            var args = "".ToArgs();
            var result = new Parser<A1>(() => new A1()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Parse")]
        public void EmptyLine()
        {
            var args = "".ToArgs();
            var result = new Parser<B1>(() => new B1()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Parse")]
        public void VerbAndArg()
        {
            var args = "a -p2 1".ToArgs();
            var result = new Parser<BB1>(() => new BB1()).Parse(args);
        }

        [TestMethod]
        [TestCategory("Parse")]
        [ExpectedException(typeof(RequiredParameterException))]
        public void RequiredArg()
        {
            var args = "1".ToArgs();
            var result = new Parser<C1>(() => new C1()).Parse(args);
        }
    }

    internal class A1 { }

    internal class B1
    {
        public string P1 { get; set; }

        public int P2 { get; set; }
    }

    internal class BB1
    {
        [DefaultVerb]
        public string P1 { get; set; }

        public int P2 { get; set; }
    }

    internal class C1
    {
        [VerbBag]
        public int[] P1 { get; set; }

        [Required]
        public int P2 { get; set; }

        public int P3 { get; set; }

        public C1()
        {
            this.P1 = new int[0];
        }
    }
}