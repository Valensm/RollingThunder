using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Wly.RollingThunder
{
    [TestClass]
    public class ReflectionTests
    {
        [TestMethod]
        [TestCategory("Reflection")]
        public void Implements()
        {
            Assert.IsTrue(typeof(System.IO.Stream).Implements<IDisposable>());
            Assert.IsTrue(typeof(List<string>).Implements<IEnumerable<object>>());
            Assert.IsFalse(typeof(List<string>).Implements<IEnumerable<int>>());
        }

        [TestMethod]
        [TestCategory("Reflection")]
        public void ImplementsGenericDefinition()
        {
            Assert.IsTrue(typeof(List<string>).ImplementsGenericDefinition(typeof(IEnumerable<>)));
        }
    }
}