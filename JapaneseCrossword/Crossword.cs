using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    public enum Cell
    {
        Unknown,
        Black,
        White
    }

    public class Crossword
    {
        public readonly Cell[,] field;
        public readonly List<int>[] rows;
        public readonly List<int>[] columns;
        public readonly int rowCount;
        public readonly int columnCount;

        public Crossword(List<int>[] rows, List<int>[] columns)
        {
            this.rows = rows;
            this.columns = columns;
            rowCount = rows.Length;
            columnCount = columns.Length;
            field = new Cell[rowCount, columnCount];
        }
    }
}
