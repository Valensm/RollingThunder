using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Wly.RollingThunder
{
    [TestClass]
    public class DescriptorAttributeTests
    {
        [TestMethod]
        [TestCategory("Descriptor Attributes")]
        public void None()
        {
            var descriptors = Descriptor.AllFromInstance<DAA>(new DAA()).ToList();
            var descriptor = descriptors[0];
            Assert.AreEqual("P1", descriptor.ShortName, "ShortName");
            Assert.AreEqual("P1", descriptor.LongName, "LongName");
            Assert.AreEqual("P1", descriptor.PropertyName, "PropertyName");
            Assert.AreEqual(false, descriptor.IsDefaultVerb, "DefaultVerb");
            Assert.AreEqual(false, descriptor.IsIgnored, "Ignored");
            Assert.AreEqual(false, descriptor.IsRequired, "Required");
            Assert.AreEqual(false, descriptor.IsVerb, "Verb");
            Assert.AreEqual(false, descriptor.IsVerbBag, "VerbBag");
            Assert.AreEqual(0, descriptor.MutualGroups.Count(), "Groups");
            Assert.AreEqual(string.Empty, descriptor.Helptext, "HelpText");
        }

        [TestMethod]
        [TestCategory("Descriptor Attributes")]
        public void AllBasic()
        {
            var descriptors = Descriptor.AllFromInstance<DAA1>(new DAA1()).ToList();
            Assert.AreEqual(4, descriptors.Count, "Count");
            var descriptor = descriptors[0];
            Assert.AreEqual("x", descriptor.ShortName, "ShortName");
            Assert.AreEqual("yyy", descriptor.LongName, "LongName");
            Assert.AreEqual("P1", descriptor.PropertyName, "PropertyName");
            Assert.AreEqual(true, descriptor.IsRequired, "Required");
            Assert.AreEqual(0, descriptor.MutualGroups.Count(), "Groups");
            Assert.AreEqual("help", descriptor.Helptext, "HelpText");

            Assert.AreEqual(true, descriptors[1].IsDefaultVerb, "DefaultVerb");
            Assert.AreEqual(true, descriptors[2].IsVerb, "Verb");
            Assert.AreEqual(true, descriptors[3].IsVerbBag, "VerbBag");
        }
    }

    internal class DAA
    {
        public string P1 { get; set; }
    }

    internal class DAA1
    {
        [Name("x", "yyy")]
        [Required("It is required")]
        [HelpText("help")]
        public string P1 { get; set; }

        [DefaultVerb]
        public int P2 { get; set; }

        [Verb]
        public int P3 { get; set; }

        [VerbBag]
        public int P4 { get; set; }

        [Ignore]
        public int P5 { get; set; }
    }
}