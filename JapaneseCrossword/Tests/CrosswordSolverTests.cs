using System.IO;
using NUnit.Framework;
using System.Diagnostics;

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
    }
}