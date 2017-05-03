using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Wly.RollingThunder
{
    [TestClass]
    public class SplitTests
    {
        [TestMethod]
        [TestCategory("System Split")]
        public void CanSplit()
        {
            var args = "app.exe v1 \"help me\" foo".ToArgs();
        }

        [TestMethod]
        [TestCategory("System Split")]
        public void SplitsCorrectly()
        {
            var args = "app.exe v1 \"help me\" foo".ToArgs();

            Assert.AreEqual(4, args.Length, "Count");
            Assert.AreEqual("app.exe", args[0], "App");
            Assert.AreEqual("v1", args[1], "1st");
            Assert.AreEqual("help me", args[2], "Quotes");
            Assert.AreEqual("foo", args[3], "3rd");
        }

        [TestMethod]
        [TestCategory("System Split")]
        public void ExtensionSplitsCorrectly()
        {
            string line = "v1 \"help me\" foo";
            var args = line.ToArgs();
            Assert.AreEqual(3, args.Length, "Count");
            Assert.AreEqual("v1", args[0], "1st");
            Assert.AreEqual("help me", args[1], "Quotes");
            Assert.AreEqual("foo", args[2], "3rd");
        }
    }
}