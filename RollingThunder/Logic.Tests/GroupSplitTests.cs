using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Wly.RollingThunder
{
    [TestClass]
    public class GroupSplitTests
    {
        [TestMethod]
        [TestCategory("Split")]
        public void EmptyGroup()
        {
            string[] args = "".ToArgs();
            var splitResult = SplitResult.FromArgs(args, new ParserConfiguration());

            Assert.AreEqual(true, splitResult.IsEmpty, "Empty");
            Assert.AreEqual(0, splitResult.ArgumentGroups.Count, "Argument Group Count");
            Assert.IsNull(splitResult.VerbGroup, "Verb Group");
        }

        [TestMethod]
        [TestCategory("Split")]
        public void SingleVerbGroup()
        {
            string[] args = "ahoj".ToArgs();
            var splitResult = SplitResult.FromArgs(args, new ParserConfiguration());
            Assert.IsNotNull(splitResult.VerbGroup, "Verb Group");
            Assert.AreEqual(0, splitResult.ArgumentGroups.Count, "Argument Group Count");
        }

        [TestMethod]
        [TestCategory("Split")]
        public void MultipleVerbGroup()
        {
            string[] args = "ahoj b c d".ToArgs();
            var splitResult = SplitResult.FromArgs(args, new ParserConfiguration());

            Assert.AreEqual(0, splitResult.ArgumentGroups.Count, "Argument Group Count");
            Assert.AreEqual(4, splitResult.VerbGroup.Values.Count, "Verbs Count");
        }

        [TestMethod]
        [TestCategory("Split")]
        public void MultipleGroups()
        {
            string[] args = "ahoj b c d -a -b".ToArgs();
            var splitResult = SplitResult.FromArgs(args, new ParserConfiguration());

            Assert.IsNotNull(splitResult.VerbGroup, "Verb Group");
            Assert.AreEqual(4, splitResult.VerbGroup.Values.Count, "Verbs Count");

            Assert.AreEqual(2, splitResult.ArgumentGroups.Count, "Argument Group Count");

            Assert.AreEqual("a", splitResult.ArgumentGroups.First().Name, "First Argument Group Name");
            Assert.AreEqual(0, splitResult.ArgumentGroups.First().Values.Count, "First Argument Group Count");

            Assert.AreEqual("b", splitResult.ArgumentGroups.Skip(1).First().Name, "Second Argument Group Name");
            Assert.AreEqual(0, splitResult.ArgumentGroups.Skip(1).First().Values.Count, "Second Argument Group Count");
        }

        [TestMethod]
        [TestCategory("Split")]
        public void MultipleGroupsMultipleValues()
        {
            string[] args = "ahoj b c d -a 3 4 -blah 5 6 7".ToArgs();
            var splitResult = SplitResult.FromArgs(args, new ParserConfiguration());

            Assert.IsNotNull(splitResult.VerbGroup, "Verb Group");
            Assert.AreEqual(4, splitResult.VerbGroup.Values.Count, "Verbs Count");

            Assert.AreEqual(2, splitResult.ArgumentGroups.Count, "Argument Group Count");

            Assert.AreEqual("a", splitResult.ArgumentGroups.First().Name, "First Argument Group Name");
            Assert.AreEqual(2, splitResult.ArgumentGroups.First().Values.Count, "First Argument Group Count");

            Assert.AreEqual("blah", splitResult.ArgumentGroups.Skip(1).First().Name, "Second Argument Group Name");
            Assert.AreEqual(3, splitResult.ArgumentGroups.Skip(1).First().Values.Count, "Second Argument Group Count");
        }

        [TestMethod]
        [TestCategory("Split")]
        public void MultipleGroupsMultipleValuesNoVerb()
        {
            string[] args = "-a 3 4 -blah 5 6 7".ToArgs();
            var splitResult = SplitResult.FromArgs(args, new ParserConfiguration());

            Assert.IsNull(splitResult.VerbGroup, "Verb Group");
            Assert.AreEqual(2, splitResult.ArgumentGroups.Count, "Argument Group Count");

            Assert.AreEqual("a", splitResult.ArgumentGroups.First().Name, "First Argument Group Name");
            Assert.AreEqual(2, splitResult.ArgumentGroups.First().Values.Count, "First Argument Group Count");

            Assert.AreEqual("blah", splitResult.ArgumentGroups.Skip(1).First().Name, "Second Argument Group Name");
            Assert.AreEqual(3, splitResult.ArgumentGroups.Skip(1).First().Values.Count, "Second Argument Group Count");
        }
    }
}