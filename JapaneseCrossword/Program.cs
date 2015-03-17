﻿namespace JapaneseCrossword
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = new CrosswordSolver();
            var tracking = new BackTracker(solver);
            //todo: руками проверять, конечно, хорошо, но где тесты на BackTracking?
            var crossword = FileWorker.ReadFromFile("TestFiles\\Winter.txt");
            var answer = tracking.GetAnswerWithBackTracking(crossword);
            FileWorker.WriteToFile("lol.txt", answer.Field);
        }
    }
}
