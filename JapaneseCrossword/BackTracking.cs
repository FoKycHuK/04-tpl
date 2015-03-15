using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    public class BackTracking
    {
        Crossword res;
        ICrosswordSolver solver;

        public BackTracking(Crossword crossword)
        {
            solver = new CrosswordSolver();
            res = crossword;
        }

        public Crossword GetAnswer()
        {
            Try(0, 0);
            return null;
        }

        void Try(int x, int y)
        {

        }
    }
}
