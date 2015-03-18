using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    public class TLPSolver : BaseCrosswordSolver
    {
        HashSet<Task<bool>> setWithCurTasks;
        Crossword crossword;

        protected override bool SolveAllTasks(Crossword crossword)
        {
            this.crossword = crossword;
            setWithCurTasks = new HashSet<Task<bool>>();

            while (rowsToWork.Count > 0 || columnsToWork.Count > 0)
            {
                if (ExecuteAllQueue(false))
                    return true;
                if (ExecuteAllQueue(true))
                    return true;
            }
            return false;
        }
        bool ExecuteAllQueue(bool isColumn)
        {
            var queue = isColumn ? columnsToWork : rowsToWork;
            while (queue.Count > 0)
            {
                var index = queue.Dequeue();
                var task = Task.Run(() => UpdateOneLine(crossword, isColumn, index));
                setWithCurTasks.Add(task);
            }
            Task.WaitAll(setWithCurTasks.ToArray());
            foreach (var value in setWithCurTasks)
                if (value.Result)
                    return true;
            setWithCurTasks.Clear();
            return false;
        }
    }
}
