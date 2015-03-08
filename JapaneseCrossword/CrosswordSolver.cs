using System;

namespace JapaneseCrossword
{
    public class CrosswordSolver : ICrosswordSolver
    {
        public SolutionStatus Solve(string inputFilePath, string outputFilePath)
        {
            Crossword cs = null;
            try { cs = FileWorker.ReadFromFile(inputFilePath); }
            catch { return SolutionStatus.BadInputFilePath; }



            try { FileWorker.WriteToFile(outputFilePath, cs.field); }
            catch { return SolutionStatus.BadOutputFilePath; }

            return SolutionStatus.IncorrectCrossword;
        }
    }
}