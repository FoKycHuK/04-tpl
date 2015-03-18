using System.IO;
using NUnit.Framework;
using System.Diagnostics;
using System.Collections.Generic;

namespace JapaneseCrossword
{
    [TestFixture]
    public class CrosswordSolverTests
    {
        private TLPSolver solver;

        [TestFixtureSetUp]
        public void SetUp()
        {
            solver = new TLPSolver();
        }

        [Test]
        public void InputFileNotFound()
        {
            var solutionStatus = solver.Solve(Path.GetRandomFileName(), Path.GetRandomFileName());
            Assert.AreEqual(SolutionStatus.BadInputFilePath, solutionStatus);
        }

        [Test]
        public void IncorrectOutputFile()
        {
            var inputFilePath = @"TestFiles\SampleInput.txt";
            var outputFilePath = "///.&*#";
            var solutionStatus = solver.Solve(inputFilePath, outputFilePath);
            Assert.AreEqual(SolutionStatus.BadOutputFilePath, solutionStatus);
        }

        [Test]
        public void IncorrectCrossword()
        {
            var inputFilePath = @"TestFiles\IncorrectCrossword.txt";
            var outputFilePath = Path.GetRandomFileName();
            var solutionStatus = solver.Solve(inputFilePath, outputFilePath);
            Assert.AreEqual(SolutionStatus.IncorrectCrossword, solutionStatus);
        }

        [Test]
        public void Simplest()
        {
            var inputFilePath = @"TestFiles\SampleInput.txt";
            var outputFilePath = Path.GetRandomFileName();
            var correctOutputFilePath = @"TestFiles\SampleInput.solved.txt";
            var solutionStatus = solver.Solve(inputFilePath, outputFilePath);
            Assert.AreEqual(SolutionStatus.Solved, solutionStatus);
            CollectionAssert.AreEqual(File.ReadAllText(correctOutputFilePath), File.ReadAllText(outputFilePath));
        }

        [Test]
        public void Car()
        {
            var inputFilePath = @"TestFiles\Car.txt";
            var outputFilePath = Path.GetRandomFileName();
            var correctOutputFilePath = @"TestFiles\Car.solved.txt";
            var solutionStatus = solver.Solve(inputFilePath, outputFilePath);
            Assert.AreEqual(SolutionStatus.Solved, solutionStatus);
            CollectionAssert.AreEqual(File.ReadAllText(correctOutputFilePath), File.ReadAllText(outputFilePath));
        }

        [Test]
        public void Flower()
        {
            var inputFilePath = @"TestFiles\Flower.txt";
            var outputFilePath = Path.GetRandomFileName();
            var correctOutputFilePath = @"TestFiles\Flower.solved.txt";
            var solutionStatus = solver.Solve(inputFilePath, outputFilePath);
            Assert.AreEqual(SolutionStatus.Solved, solutionStatus);
            CollectionAssert.AreEqual(File.ReadAllText(correctOutputFilePath), File.ReadAllText(outputFilePath));
        }

        [Test]
        public void Winter()
        {
            var inputFilePath = @"TestFiles\Winter.txt";
            var outputFilePath = Path.GetRandomFileName();
            var correctOutputFilePath = @"TestFiles\Winter.solved.txt";
            var solutionStatus = solver.Solve(inputFilePath, outputFilePath);
            Assert.AreEqual(SolutionStatus.PartiallySolved, solutionStatus);
            CollectionAssert.AreEqual(File.ReadAllText(correctOutputFilePath), File.ReadAllText(outputFilePath));
        }

        [Test]
        public void TLP_faster_than_simple() // also harder, better and stronger.
        {
            var solver = new CrosswordSolver();
            var TLPsolver = new TLPSolver();
            var stopwatch = Stopwatch.StartNew();
            var input = @"TestFiles\Flower.txt";
            solver.Solve(input, Path.GetRandomFileName());
            var simpleTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Stop();
            stopwatch = Stopwatch.StartNew();
            TLPsolver.Solve(input, Path.GetRandomFileName());
            var TLPTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Stop();
            Assert.IsTrue(TLPTime < simpleTime);
        }

        [Test]
        public void BackTracker_winter_test()
        {
            var inputFilePath = @"TestFiles\Winter.txt";
            var outputFilePath = Path.GetRandomFileName();
            var correctOutputFilePath = @"TestFiles\Winter.FullSolved.txt";

            var solver = new CrosswordSolver();
            var tracking = new BackTracker(solver);
            var crossword = FileWorker.ReadFromFile(inputFilePath);
            var answer = tracking.GetAnswer(crossword);
            FileWorker.WriteToFile(outputFilePath, answer.Field);
            CollectionAssert.AreEqual(File.ReadAllText(correctOutputFilePath), File.ReadAllText(outputFilePath));
        }

        [Test]
        public void BackTracker_working_on_car()
        {
            var inputFilePath = @"TestFiles\Car.txt";
            var outputFilePath = Path.GetRandomFileName();
            var correctOutputFilePath = @"TestFiles\Car.solved.txt";

            var solver = new CrosswordSolver();
            var tracking = new BackTracker(solver);
            var crossword = FileWorker.ReadFromFile(inputFilePath);
            var answer = tracking.GetAnswer(crossword);
            FileWorker.WriteToFile(outputFilePath, answer.Field);
            CollectionAssert.AreEqual(File.ReadAllText(correctOutputFilePath), File.ReadAllText(outputFilePath));
        }

        [Test]
        public void BackTracker_simple_test()
        {
            var solver = new CrosswordSolver();
            var tracking = new BackTracker(solver);
            var crossword = new Crossword(new List<int>[] { new List<int> { 1 }, new List<int> { 1 }}, new List<int>[] { new List<int>() { 1 } , new List<int>() { 1 } });
            var answer = tracking.GetAnswer(crossword);
            Assert.AreEqual(Cell.White, answer.Field[0, 0]);
            Assert.AreEqual(Cell.White, answer.Field[1, 1]);
            Assert.AreEqual(Cell.Black, answer.Field[1, 0]);
            Assert.AreEqual(Cell.Black, answer.Field[0, 1]);
        }
    }
}