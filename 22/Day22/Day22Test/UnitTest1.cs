using NUnit.Framework;
using Day22;

namespace Day22Test
{
    public class Tests
    {

        [Test]
        public void Test1()
        {
            HandOfCards hoc = new HandOfCards(new int[] { });
            int res = hoc.PopTop();
            Assert.AreEqual(-1, res);
        }
        [Test]
        public void Test2()
        {
            HandOfCards hoc = new HandOfCards(new int[] { });
            hoc.AddToBottom(987);
            int res = hoc.PopTop();
            Assert.AreEqual(987, res);
        }
        [Test]
        public void Test3()
        {
            HandOfCards hoc = new HandOfCards(new int[] { 9, 8, 7 });
            int res1 = hoc.PopTop();
            int res2 = hoc.PopTop();
            int res3 = hoc.PopTop();
            int res4 = hoc.PopTop();
            Assert.AreEqual(9, res1);
            Assert.AreEqual(8, res2);
            Assert.AreEqual(7, res3);
            Assert.AreEqual(-1, res4);
        }
        [Test]
        public void Test4()
        {
            HandOfCards hoc = new HandOfCards(new int[] { 9, 8, 7 });
            int c = hoc.Count;
            Assert.AreEqual(3, c);
            hoc.AddToBottom(11);
            c = hoc.Count;
            Assert.AreEqual(4, c);
            hoc.PopTop();
            hoc.PopTop();
            Assert.AreEqual(2, hoc.Count);
            hoc.PopTop();
            hoc.PopTop();
            Assert.AreEqual(0, hoc.Count);
        }
        [Test]
        public void Test5()
        {
            HandOfCards hoc = new HandOfCards(new int[] { 9, 8, 7 });
            hoc.AddToBottom(987);
            int res = hoc.PopTop();
            Assert.AreEqual(9, res);
        }
    }
}