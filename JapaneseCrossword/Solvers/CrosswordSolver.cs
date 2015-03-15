using System;
using System.Collections.Generic;

namespace JapaneseCrossword
{
    public class CrosswordSolver : BaseCrosswordSolver
    {
        protected override void Start(Crossword crossword)
        {
            while (rowsToWork.Count > 0 || columnsToWork.Count > 0)
            {
                while (rowsToWork.Count > 0)
                    UpdateOneLine(crossword, false, rowsToWork.Dequeue());
                while (columnsToWork.Count > 0)
                    UpdateOneLine(crossword, true, columnsToWork.Dequeue());
            }
        }
    }
}