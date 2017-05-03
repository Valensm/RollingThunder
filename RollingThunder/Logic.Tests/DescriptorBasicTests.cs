using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Wly.RollingThunder
{
    [TestClass]
    public class DescriptorBasicTests
    {
        [TestMethod]
        [TestCategory("Descriptor")]
        public void Empty()
        {
            var descriptors = Descriptor.AllFromInstance<A>(new A()).ToList();
            Assert.AreEqual(0, descriptors.Count);
        }

        [TestMethod]
        [TestCategory("Descriptor")]
        public void Simple()
        {
            var descriptors = Descriptor.AllFromInstance<B>(new B()).ToList();
            Assert.AreEqual(4, descriptors.Count, "Count");
            Assert.AreEqual("P1", descriptors[0].ShortName, "Name1");
            Assert.AreEqual("P2", descriptors[1].ShortName, "Name2");
            Assert.AreEqual("P3", descriptors[2].ShortName, "Name3");
            Assert.AreEqual("P4", descriptors[3].ShortName, "Name4");
            Assert.AreEqual(0, descriptors[0].Descriptors.Count(), "Count1");
            Assert.AreEqual(0, descriptors[1].Descriptors.Count(), "Count2");
            Assert.AreEqual(0, descriptors[2].Descriptors.Count(), "Count3");
            Assert.AreEqual(0, descriptors[3].Descriptors.Count(), "Count4");
        }

        [TestMethod]
        [TestCategory("Descriptor")]
        public void Complex()
        {
            var descriptors = Descriptor.AllFromInstance<C>(new C()).ToList();
            Assert.AreEqual(4, descriptors.Count, "Count");
            Assert.AreEqual("P1", descriptors[0].ShortName, "NameP1");
            Assert.AreEqual("P2", descriptors[1].ShortName, "NameP2");
            Assert.AreEqual("P3", descriptors[2].ShortName, "NameP3");
            Assert.AreEqual("P4", descriptors[3].ShortName, "NameP4");
            Assert.AreEqual(0, descriptors[0].Descriptors.Count(), "CountP1");
            Assert.AreEqual(0, descriptors[1].Descriptors.Count(), "CountP2");
            Assert.AreEqual(0, descriptors[2].Descriptors.Count(), "CountP3");
            Assert.AreEqual(2, descriptors[3].Descriptors.Count(), "CountP4");

            var innerDescriptors = descriptors[3].Descriptors.ToList();

            Assert.AreEqual("P1", innerDescriptors[0].ShortName, "Name1");
            Assert.AreEqual("P2", innerDescriptors[1].ShortName, "Name2");
            Assert.AreEqual(0, innerDescriptors[0].Descriptors.Count(), "Count1");
            Assert.AreEqual(0, innerDescriptors[1].Descriptors.Count(), "Count2");
        }
    }

    public class A { }

    public class B
    {
        public string P1 { get; set; }

        public int P2 { get; set; }

        public decimal P3 { get; set; }

        public bool P4 { get; set; }
    }

    public class C
    {
        public string P1 { get; set; }

        public int P2 { get; set; }

        public decimal P3 { get; set; }

        public D P4 { get; set; }
    }

    public class D
    {
        public double P1 { get; set; }

        public B P2 { get; set; }
    }
}