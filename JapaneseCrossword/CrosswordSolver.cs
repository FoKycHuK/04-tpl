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

            MakeFirstRaw(cs);

            try { FileWorker.WriteToFile(outputFilePath, cs.field); }
            catch { return SolutionStatus.BadOutputFilePath; }

            return SolutionStatus.IncorrectCrossword;
        }

        void MakeFirstRaw(Crossword cs)
        {
            var a = new UpdateLine(cs.GetLine(false, 0), cs.rows[0]);

            cs.SetLine(false, 0, a.GetAnswer());
        }
    }
}