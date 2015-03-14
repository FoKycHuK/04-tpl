using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    public class CrosswordSolver : ICrosswordSolver // TLP
    {
        Queue<int> rowsToWork;
        Queue<int> columnsToWork;

        public SolutionStatus Solve(string inputFilePath, string outputFilePath)
        {
            Crossword cs = null;
            try { cs = FileWorker.ReadFromFile(inputFilePath); }
            catch { return SolutionStatus.BadInputFilePath; }

            rowsToWork = new Queue<int>();
            columnsToWork = new Queue<int>();
            for (var i = 0; i < cs.rowCount; i++)
                rowsToWork.Enqueue(i);
            for (var i = 0; i < cs.columnCount; i++)
                columnsToWork.Enqueue(i);

            try
            {
                while (rowsToWork.Count > 0 || columnsToWork.Count > 0)
                {
                    var set = new HashSet<Task>();
                    //todo: два раза написано одно и тоже, стоит вынести общий код в один метод
                    while (rowsToWork.Count > 0)
                    {
                        var index = rowsToWork.Dequeue();
                        var task = new Task(() => LineUpdater.UpdateOneLine(cs, false, index, columnsToWork));
                        task.Start();
                        set.Add(task);
                    }
                    Task.WaitAll(set.ToArray());
                    set.Clear();
                    while (columnsToWork.Count > 0)
                    {
                        var index = columnsToWork.Dequeue();
                        var task = new Task(() => LineUpdater.UpdateOneLine(cs, true, index, rowsToWork));
                        task.Start();
                        set.Add(task);
                    }
                    Task.WaitAll(set.ToArray());
                    set.Clear();
                }
            }
            catch (AggregateException e)
            {
                foreach (var exception in e.InnerExceptions)
                    if (exception.GetType() != typeof(ArgumentException))
                        throw;
                return SolutionStatus.IncorrectCrossword;
            }


            try { FileWorker.WriteToFile(outputFilePath, cs.field); }
            catch (ArgumentException) { return SolutionStatus.BadOutputFilePath; }

            foreach (var value in cs.field)
                if (value == Cell.Unknown)
                    return SolutionStatus.PartiallySolved;
            return SolutionStatus.Solved;
        }
    }
}
