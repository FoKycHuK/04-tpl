using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword.Tests
{
    [TestFixture]
    public class CrosswordTests
    {
        [Test]
        public void Correct_get_column()
        {
            var cs = new Crossword(new List<int>[10], new List<int>[10]);
            cs.Field[1, 3] = Cell.Black;
            var line = cs.GetLine(true, 3);
            Assert.AreEqual(Cell.Black, line[1]);
        }

        [Test]
        public void Correct_get_row()
        {
            var cs = new Crossword(new List<int>[10], new List<int>[10]);
            cs.Field[1, 3] = Cell.Black;
            var line = cs.GetLine(false, 1);
            Assert.AreEqual(Cell.Black, line[3]);
        }

        [Test]
        public void Correct_set_line()
        {
            var cs = new Crossword(new List<int>[10], new List<int>[10]);
            var line = new Cell[10];
            line[8] = Cell.Black;
            cs.SetLine(false, 6, line);
            Assert.AreEqual(Cell.Black, cs.Field[6, 8]);
        }
    }
}
