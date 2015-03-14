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
        //todo: есть общие правила форматирования для c#, все что Public обычно пишется в большой буквы.
        public readonly Cell[,] field;
        //todo: когда используешь rows и columns ну никак не очевидно, что внутри лежат описания блоков. rowBlocks/rowGroups гораздо круче читались бы 
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

        public Cell[] GetLine(bool isColumn, int index)
        {
            var ans = new Cell[isColumn ? rowCount : columnCount];
            for (var i = 0; i < ans.Length; i++)
            {
                var x = isColumn ? i : index;
                var y = isColumn ? index : i;
                ans[i] = field[x, y];
            }
            return ans;
        }

        public void SetLine(bool isColumn, int index, Cell[] line)
        {
            for (var i = 0; i < line.Length; i++)
            {
                var x = isColumn ? i : index;
                var y = isColumn ? index : i;
                field[x, y] = line[i];
            }
        }
    }
}
