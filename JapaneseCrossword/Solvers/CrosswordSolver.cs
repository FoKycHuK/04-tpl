using System;
using System.Collections.Generic;

namespace JapaneseCrossword
{
    public class CrosswordSolver : BaseCrosswordSolver
    {
        protected override void Start()
        {
            while (rowsToWork.Count > 0 || columnsToWork.Count > 0)
            {
                while (rowsToWork.Count > 0)
                    UpdateOneLine(false, rowsToWork.Dequeue());
                while (columnsToWork.Count > 0)
                    UpdateOneLine(true, columnsToWork.Dequeue());
            }
        }
    }
}