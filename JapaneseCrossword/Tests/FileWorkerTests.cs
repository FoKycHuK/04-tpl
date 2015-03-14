using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    [TestFixture]
    public class FileWorkerTests
    {
        [Test]
        public void Correct_load_file()
        {
            var cs = FileWorker.ReadFromFile("TestFiles\\SampleInput.txt");
            var sb = new StringBuilder();
            foreach (var list in cs.RowsBlocks)
                foreach (var value in list)
                    sb.Append(value);
            foreach (var list in cs.ColumnsBlocks)
                foreach (var value in list)
                    sb.Append(value);
            Assert.AreEqual(sb.ToString(), "2112");
        }

        [Test]
        public void Correct_write_to_file()
        {
            var fileName = Path.GetRandomFileName();
            var field = new Cell[2, 2];
            field[0, 0] = Cell.Black;
            field[1, 0] = Cell.White;
            FileWorker.WriteToFile(fileName, field);
            Assert.AreEqual("*?\r\n.?\r\n", File.ReadAllText(fileName));
        }
    }
}
