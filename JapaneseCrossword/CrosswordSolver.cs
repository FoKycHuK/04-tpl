using System;
using System.Collections.Generic;

namespace JapaneseCrossword
{
    public class CrosswordSolver : ICrosswordSolver
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
                    while (rowsToWork.Count > 0)
                        UpdateOneLine(cs, false, rowsToWork.Dequeue());
                    while (columnsToWork.Count > 0)
                        UpdateOneLine(cs, true, columnsToWork.Dequeue());
                }
            }
            catch (ArgumentException) { return SolutionStatus.IncorrectCrossword; }

            try { FileWorker.WriteToFile(outputFilePath, cs.field); }
            catch (ArgumentException) { return SolutionStatus.BadOutputFilePath; }

            foreach (var value in cs.field)
                if (value == Cell.Unknown)
                    return SolutionStatus.PartiallySolved;
            return SolutionStatus.Solved;
        }

        void UpdateOneLine(Crossword cs, bool isColumn, int index)
        {
            var line = cs.GetLine(isColumn, index);
            var res = new UpdateLine(line, isColumn ? cs.columns[index] : cs.rows[index]).GetAnswer();
            for (var i = 0; i < line.Length; i++)
                if (res[i] != line[i])
                {
                    if (isColumn && !rowsToWork.Contains(i))
                        rowsToWork.Enqueue(i);
                    if (!isColumn && !columnsToWork.Contains(i))
                        columnsToWork.Enqueue(i);
                }
            cs.SetLine(isColumn, index, res);
        }
    }
}