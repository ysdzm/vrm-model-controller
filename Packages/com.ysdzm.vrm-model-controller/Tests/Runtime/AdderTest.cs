using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Ysdzm.VrmModelController.Tests
{
    public class AdderTest
    {
        [TestCase(1, 2, 3)]
        [TestCase(-1, 2, 1)]
        [TestCase(0, 0, 0)]
        public void AdderAddTest(int x, int y, int expected)
        {
            var adder = new Adder();
            var actual = adder.Add(x, y);
            Assert.AreEqual(expected, actual, $"x: {x}, y: {y}");
        }
    }
}
