using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword.Tests
{
    [TestFixture]
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

        [Test]
        public void Can_place_long_right()
        {
            var line = new Cell[10];
            line[0] = Cell.Black;
            line[5] = Cell.Black;
            line[9] = Cell.Black;
            var updater = new LineUpdater(line, new List<int>() { 1, 1, 3 });
            var newLine = updater.GetAnswer();
            for (var i = 7; i < 10; i++)
                Assert.IsTrue(newLine[i] == Cell.Black);

        }

        [Test]
        public void Do_not_place_long_if_not_sure()
        {
            var line = new Cell[10];
            line[0] = Cell.Black;
            line[5] = Cell.Black;
            var updater = new LineUpdater(line, new List<int>() { 1, 1, 3 });
            var newLine = updater.GetAnswer();
            for (var i = 7; i < 10; i++)
                Assert.IsTrue(newLine[i] == Cell.Unknown);

        }

        [Test]
        public void Said_that_input_data_is_incorrect()
        {
            var line = new Cell[10];
            for (var i = 0; i < line.Length; i++)
                line[i] = Cell.White;
            var updater = new LineUpdater(line, new List<int>() { 1 });
            var newLine = updater.GetAnswer();
            if (newLine != null)
                Assert.Fail();
        }
    }
}
