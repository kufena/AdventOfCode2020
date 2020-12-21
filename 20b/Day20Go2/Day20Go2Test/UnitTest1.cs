using Day20Go2;
using NUnit.Framework;

namespace Day20Go2Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            string[] rows = new string[] { "123", "456", "789" };
            Tile t = new Tile(1);
            t.Init(rows);
            Assert.AreEqual(1, t.num);
            Assert.AreEqual(8, t.states.Count);
            Assert.AreEqual("456", t.states[0].rows[1]);
            Assert.AreEqual("321", t.states[1].top);

        }
    }
}