using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    abstract public class BaseCrosswordSolver : ICrosswordSolver
    {
        protected Queue<int> rowsToWork;
        protected Queue<int> columnsToWork;

        public SolutionStatus Solve(string inputFilePath, string outputFilePath)
        {
            Crossword crossword = null;

            try
            {
                crossword = FileWorker.ReadFromFile(inputFilePath);
            }
            catch
            {
                return SolutionStatus.BadInputFilePath;
            }

            crossword = SolveObviousLines(crossword);
            if (crossword == null)
                return SolutionStatus.IncorrectCrossword;

            try
            {
                FileWorker.WriteToFile(outputFilePath, crossword.Field);
            }
            catch
            {
                return SolutionStatus.BadOutputFilePath;
            }

            foreach (var value in crossword.Field)
                if (value == Cell.Unknown)
                    return SolutionStatus.PartiallySolved;
            return SolutionStatus.Solved;
        }

        public Crossword SolveObviousLines(Crossword crossword) //эта штука самостоятельна. отделил просто чтоб не копировать код и использовать ее в бек-трекинге.
        {
            rowsToWork = new Queue<int>();
            columnsToWork = new Queue<int>();

            for (var i = 0; i < crossword.RowCount; i++)
                rowsToWork.Enqueue(i);
            for (var i = 0; i < crossword.ColumnCount; i++)
                columnsToWork.Enqueue(i);

            var error = SolveAllTasks(crossword);
            if (error)
                return null;

            return crossword;
        }
        protected bool UpdateOneLine(Crossword crossword, bool isColumn, int index)
        {
            var otherLinesToWork = isColumn ? rowsToWork : columnsToWork;
            var line = crossword.GetLine(isColumn, index);
            var res = new LineUpdater(line, isColumn ? crossword.ColumnsBlocks[index] : crossword.RowsBlocks[index]).GetAnswer();
            if (res == null)
                return true;
            for (var i = 0; i < line.Length; i++)
                if (res[i] != line[i] && !otherLinesToWork.Contains(i))
                    otherLinesToWork.Enqueue(i);
            crossword.SetLine(isColumn, index, res);
            return false;
        }


        protected abstract bool SolveAllTasks(Crossword crossword);


    }
}
