using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    public class BackTracker
    {
        BaseCrosswordSolver solver;

        public BackTracker(BaseCrosswordSolver solver)
        {
            this.solver = solver;
        }

        public Crossword GetAnswer(Crossword crossword)
        {
            if (solver.SolveObviousLines(crossword) == null)
                return null;
            Point unknown = null;
            for (var i = 0; i < crossword.RowCount; i++)
            {
                for (var j = 0; j < crossword.ColumnCount; j++)
                    if (crossword.Field[i, j] == Cell.Unknown)
                    {
                        unknown = new Point(i, j);
                        break;
                    }
                if (unknown != null)
                    break;
            }
            if (unknown == null)
                return crossword;
            var supposition = crossword.Copy();
            supposition.Field[unknown.X, unknown.Y] = Cell.White;
            var answer = GetAnswer(supposition);
            if (answer != null)
                return answer;
            supposition.Field[unknown.X, unknown.Y] = Cell.Black;
            return GetAnswer(supposition);
        }
    }
}
