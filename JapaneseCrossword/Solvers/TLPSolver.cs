using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    public class TLPSolver : BaseCrosswordSolver
    {
        HashSet<Task> setWithCurTasks;
        Crossword crossword;

        protected override void Start(Crossword crossword)
        {
            this.crossword = crossword;
            setWithCurTasks = new HashSet<Task>();

            try
            {
                while (rowsToWork.Count > 0 || columnsToWork.Count > 0)
                {
                    ExecuteAllQueue(false);
                    ExecuteAllQueue(true);
                }
            }
            catch (AggregateException e)
            {
                throw e.InnerExceptions.Last();
            }
        }
        void ExecuteAllQueue(bool isColumn)
        {
            var queue = isColumn ? columnsToWork : rowsToWork;
            while (queue.Count > 0)
            {
                var index = queue.Dequeue();
                var task = new Task(() => UpdateOneLine(crossword, isColumn, index));
                task.Start();
                setWithCurTasks.Add(task);
            }
            Task.WaitAll(setWithCurTasks.ToArray());
            setWithCurTasks.Clear();
        }
    }
}
