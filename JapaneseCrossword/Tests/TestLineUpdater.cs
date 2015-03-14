using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword.Tests
{
    [TestFixture]
    //todo: кажется, что возможных вариантов поведения гораздо больше, чем написано тестов. Неплохо было бы еще интересных добавить
    public class TestLineUpdater
    {
        [Test]
        public void Can_solve_when_full_info()
        {
            var updater = new LineUpdater(new Cell[10], new List<int>() { 10 });
            var newLine = updater.GetAnswer();
            for (var i = 0; i < newLine.Length; i++)
                Assert.IsTrue(newLine[i] == Cell.Black);
        }

        [Test]
        public void Can_solve_simple_middle()
        {
            var updater = new LineUpdater(new Cell[10], new List<int>() { 6 });
            var newLine = updater.GetAnswer();
            for (var i = 4; i < 6; i++)
                Assert.IsTrue(newLine[i] == Cell.Black);
        }

        [Test]
        public void Make_it_white_if_already_done()
        {
            var line = new Cell[10];
            line[7] = Cell.Black;
            var updater = new LineUpdater(line, new List<int>() { 1 });
            var newLine = updater.GetAnswer();
            for (var i = 0; i < line.Length; i++)
                if (i != 7)
                    Assert.IsTrue(newLine[i] == Cell.White);
        }
    }
}
