using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    //todo: (переделывать необязательно) нестатические классы облегчают тестирование (можно обойтись исключительно модульными тестами и мочить абстракцию), позволяют более гибко работать с кодом
    public static class FileWorker
    {
        static Dictionary<Cell, char> cellToCharMap = new Dictionary<Cell, char>()
        {
            {Cell.Unknown, '?'},
            {Cell.Black, '*'},
            {Cell.White, '.'}
        };

        public static Crossword ReadFromFile(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            var rowCount = int.Parse(lines[0].Split(':')[1].Trim());
            var rows = GetLinesSettings(lines, rowCount, 1);

            var columnCount = int.Parse(lines[rowCount + 1].Split(':')[1].Trim());
            var columns = GetLinesSettings(lines, columnCount, rowCount + 2);

            return new Crossword(rows, columns);
        }

        static List<int>[] GetLinesSettings(string[] lines, int count, int startValue)
        {
            var ans = new List<int>[count];
            for (var i = 0; i < count; i++)
            {
                ans[i] = new List<int>();
                foreach (var value in lines[i + startValue].Split(' '))
                    ans[i].Add(int.Parse(value));
            }
            return ans;
        }

        public static void WriteToFile(string fileName, Cell[,] field)
        {
            var data = new StringBuilder();
            for (var i = 0; i < field.GetLength(0); i++)
            {
                for (var j = 0; j < field.GetLength(1); j++)
                    data.Append(cellToCharMap[field[i, j]]);
                data.Append(Environment.NewLine);
            }
            File.WriteAllText(fileName, data.ToString());
        }
    }
}
