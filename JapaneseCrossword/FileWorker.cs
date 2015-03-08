using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    public static class FileWorker
    {
        static Dictionary<Cell, char> formatter = new Dictionary<Cell, char>()
        {
            {Cell.Unknown, '?'},
            {Cell.Black, '*'},
            {Cell.Whilte, '.'}
        };
        public static Crossword ReadFromFile(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            var rowCount = int.Parse(lines[0].Split(':')[1].Trim());
            var rows = new List<int>[rowCount];
            for (var i = 1; i <= rowCount; i++)
            {
                rows[i - 1] = new List<int>();
                foreach (var value in lines[i].Split(' '))
                    rows[i - 1].Add(int.Parse(value));
            }

            var columnCount = int.Parse(lines[rowCount + 1].Split(':')[1].Trim());
            var columns = new List<int>[columnCount];
            for (var i = rowCount + 2; i < lines.Length; i++)
            {
                columns[i - (rowCount + 2)] = new List<int>();
                foreach (var value in lines[i].Split(' '))
                    columns[i - (rowCount + 2)].Add(int.Parse(value));
            }

            return new Crossword(rows, columns);
        }

        public static void WriteToFile(string fileName, Cell[,] field)
        {
            if (!File.Exists(fileName))
                File.Create(fileName);
            var data = new StringBuilder();
            for (var i = 0; i < field.GetLength(0); i++)
            {
                for (var j = 0; j < field.GetLength(1); j++)
                    data.Append(formatter[field[i, j]]);
                data.Append("\r\n");
            }
            File.WriteAllText(fileName, data.ToString());
        }
    }
}
