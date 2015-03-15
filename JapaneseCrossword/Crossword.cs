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
        public readonly Cell[,] Field;
        public readonly List<int>[] RowsBlocks;
        public readonly List<int>[] ColumnsBlocks;
        public readonly int RowCount;
        public readonly int ColumnCount;

        public Crossword(List<int>[] rows, List<int>[] columns, Cell[,] field = null)
        {
            this.RowsBlocks = rows;
            this.ColumnsBlocks = columns;
            RowCount = rows.Length;
            ColumnCount = columns.Length;
            Field = field != null ? field : new Cell[RowCount, ColumnCount];
        }

        public Cell[] GetLine(bool isColumn, int index)
        {
            var ans = new Cell[isColumn ? RowCount : ColumnCount];
            for (var i = 0; i < ans.Length; i++)
            {
                var x = isColumn ? i : index;
                var y = isColumn ? index : i;
                ans[i] = Field[x, y];
            }
            return ans;
        }

        public void SetLine(bool isColumn, int index, Cell[] line)
        {
            for (var i = 0; i < line.Length; i++)
            {
                var x = isColumn ? i : index;
                var y = isColumn ? index : i;
                Field[x, y] = line[i];
            }
        }

        public Crossword Copy()
        {
            var copy = new Crossword(RowsBlocks, ColumnsBlocks,(Cell[,])Field.Clone());
            return copy;
        }
    }
}
