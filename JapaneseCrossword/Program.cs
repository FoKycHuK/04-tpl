namespace JapaneseCrossword
{
    class Program
    {
        static void Main(string[] args)
        {
            FileWorker.ReadFromFile("TestFiles\\Car.txt");
            var solver = new CrosswordSolver();
            solver.Solve("TestFiles\\Car.txt", "lol.txt");
        }
    }
}
