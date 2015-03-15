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

            try
            {
                crossword = IterateLineLook(crossword);
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

        public Crossword IterateLineLook(Crossword crossword) //эта штука самостоятельна. отделил просто чтоб не копировать код и использовать ее в бек-трекинге.
        {
            rowsToWork = new Queue<int>();
            columnsToWork = new Queue<int>();

            for (var i = 0; i < crossword.RowCount; i++)
                rowsToWork.Enqueue(i);
            for (var i = 0; i < crossword.ColumnCount; i++)
                columnsToWork.Enqueue(i);

            Start(crossword);

            return crossword;
        }
        protected void UpdateOneLine(Crossword crossword, bool isColumn, int index)
        {
            var otherLinesToWork = isColumn ? rowsToWork : columnsToWork;
            var line = crossword.GetLine(isColumn, index);
            var res = new LineUpdater(line, isColumn ? crossword.ColumnsBlocks[index] : crossword.RowsBlocks[index]).GetAnswer();
            for (var i = 0; i < line.Length; i++)
                if (res[i] != line[i] && !otherLinesToWork.Contains(i))
                    otherLinesToWork.Enqueue(i);
            crossword.SetLine(isColumn, index, res);
        }


        protected abstract void Start(Crossword crossword);


    }
}
