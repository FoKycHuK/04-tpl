//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JapaneseCrossword.Tests
//{
//    [TestFixture]
//    //todo: кажется, что возможных вариантов поведения гораздо больше, чем написано тестов. Неплохо было бы еще интересных добавить
//    public class TestLineUpdater
//    {
//        void UpdateOneLine(Crossword cs, 
//        [Test]
//        public void Can_solve_when_full_info()
//        {
//            var cs = new Crossword(new List<int>[] { new List<int>() { 10 } }, new List<int>[10]);
//            var queue = new Queue<int>();
//            LineUpdater.UpdateOneLine(cs, false, 0, queue);
//            for(var i = 0; i < cs.ColumnCount; i++)
//            {
//                Assert.IsTrue(cs.Field[0, i] == Cell.Black);
//                Assert.IsTrue(queue.Contains(i));
//            }
//        }

//        [Test]
//        public void Can_solve_simple_middle()
//        {
//            var cs = new Crossword(new List<int>[] { new List<int>() { 6 } }, new List<int>[10]);
//            var queue = new Queue<int>();
//            LineUpdater.UpdateOneLine(cs, false, 0, queue);
//            for (var i = 4; i < 6; i++)
//            {
//                Assert.IsTrue(cs.Field[0, i] == Cell.Black);
//                Assert.IsTrue(queue.Contains(i));
//            }
//        }

//        [Test]
//        public void Make_it_white_if_already_done()
//        {
//            var cs = new Crossword(new List<int>[] { new List<int>() { 1 } }, new List<int>[10]);
//            cs.Field[0, 7] = Cell.Black;
//            var queue = new Queue<int>();
//            LineUpdater.UpdateOneLine(cs, false, 0, queue);
//            for (var i = 0; i < 10; i++)
//            {
//                if (i != 7)
//                {
//                    Assert.IsTrue(cs.Field[0, i] == Cell.White);
//                    Assert.IsTrue(queue.Contains(i));
//                }
//            }
//        }
//    }
//}
