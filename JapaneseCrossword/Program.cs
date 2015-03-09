namespace JapaneseCrossword
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = new CrosswordSolver();
            solver.Solve("TestFiles\\Car.txt", "lol.txt");
        }
    }
}
