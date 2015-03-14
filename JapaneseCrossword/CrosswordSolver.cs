using System;
using System.Collections.Generic;

namespace JapaneseCrossword
{
    public class CrosswordSolver1 : ICrosswordSolver // non-TLP
    {
        Queue<int> rowsToWork;
        Queue<int> columnsToWork;

        public SolutionStatus Solve(string inputFilePath, string outputFilePath)
        {
            //todo: cs - неговорящее название, не отображает семантику. Переменных из нескольких букв стоит избегать
            Crossword cs = null;
            //todo: поправил форматирование, изменить остальные единообразно, читать стает проще
            try
            {
                cs = FileWorker.ReadFromFile(inputFilePath);
            }
            catch
            {
                return SolutionStatus.BadInputFilePath;
            }

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
                        LineUpdater.UpdateOneLine(cs, false, rowsToWork.Dequeue(), columnsToWork);
                    while (columnsToWork.Count > 0)
                        LineUpdater.UpdateOneLine(cs, true, columnsToWork.Dequeue(), rowsToWork);
                }
            }
            catch (ArgumentException) { return SolutionStatus.IncorrectCrossword; }

            try { FileWorker.WriteToFile(outputFilePath, cs.field); }
            //todo: тут может вылететь не только ArgumentException, другие исключения тоже соответствуют описанию BadOutputFilePath
            catch (ArgumentException) { return SolutionStatus.BadOutputFilePath; }

            foreach (var value in cs.field)
                if (value == Cell.Unknown)
                    return SolutionStatus.PartiallySolved;
            return SolutionStatus.Solved;
        }
    }
}