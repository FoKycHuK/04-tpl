using System;
using System.Collections.Generic;

namespace JapaneseCrossword
{
    public class CrosswordSolver : BaseCrosswordSolver
    {
        protected override bool SolveAllTasks(Crossword crossword)
        {
            while (rowsToWork.Count > 0 || columnsToWork.Count > 0)
            {
                while (rowsToWork.Count > 0)
                    if (UpdateOneLine(crossword, false, rowsToWork.Dequeue()))
                        return true;
                while (columnsToWork.Count > 0)
                    if (UpdateOneLine(crossword, true, columnsToWork.Dequeue()))
                        return true;
            }
            return false;
        }
    }
}