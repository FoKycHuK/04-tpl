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
        protected Crossword crossword;

        public SolutionStatus Solve(string inputFilePath, string outputFilePath)
        {
            try
            {
                crossword = FileWorker.ReadFromFile(inputFilePath);
            }
            catch
            {
                return SolutionStatus.BadInputFilePath;
            }

            rowsToWork = new Queue<int>();
            columnsToWork = new Queue<int>();

            for (var i = 0; i < crossword.RowCount; i++)
                rowsToWork.Enqueue(i);
            for (var i = 0; i < crossword.ColumnCount; i++)
                columnsToWork.Enqueue(i);

            try
            {
                Start();
            }
            catch (IncorrectLineUpdaterInputDataException)
            {
                return SolutionStatus.IncorrectCrossword;
            }

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

        protected void UpdateOneLine(bool isColumn, int index)
        {
            var otherLinesToWork = isColumn ? rowsToWork : columnsToWork;
            var line = crossword.GetLine(isColumn, index);
            var res = new LineUpdater(line, isColumn ? crossword.ColumnsBlocks[index] : crossword.RowsBlocks[index]).GetAnswer();
            for (var i = 0; i < line.Length; i++)
                if (res[i] != line[i] && !otherLinesToWork.Contains(i))
                    otherLinesToWork.Enqueue(i);
            crossword.SetLine(isColumn, index, res);
        }


        protected abstract void Start();


    }
}
