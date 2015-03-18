namespace JapaneseCrossword
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = new CrosswordSolver();
            var tracking = new BackTracker(solver);
            var crossword = FileWorker.ReadFromFile("TestFiles\\Winter.txt");
            var answer = tracking.GetAnswer(crossword);
            FileWorker.WriteToFile("lol.txt", answer.Field);
        }
    }
}
