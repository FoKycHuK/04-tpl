using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    public class UpdateLine
    {
        Cell[] line;
        List<int> lineInfo;
        bool[] possibleBlack;
        bool[] possibleWhite;

        static public void UpdateOneLine(Crossword cs, bool isColumn, int index, Queue<int> otherLinesToWork)
        {
            var line = cs.GetLine(isColumn, index);
            var res = new UpdateLine(line, isColumn ? cs.columns[index] : cs.rows[index]).GetAnswer();
            for (var i = 0; i < line.Length; i++)
                if (res[i] != line[i] && !otherLinesToWork.Contains(i))
                    otherLinesToWork.Enqueue(i);
            cs.SetLine(isColumn, index, res);
        }

        UpdateLine(Cell[] line, List<int> lineInfo)
        {
            this.line = line;
            possibleBlack = new bool[line.Length];
            possibleWhite = new bool[line.Length];
            this.lineInfo = lineInfo;
        }

        Cell[] GetAnswer()
        {
            for (var i = 0; i <= line.Length - lineInfo[0]; i++)
            {
                if (i > 0 && line[i - 1] == Cell.Black) // Rest In Peace
                    break; 
                if (SomethingRecursion(i, 0))
                {
                    for (var j = 0; j < i; j++)
                        possibleWhite[j] = true;
                }
            }
            var ans = new Cell[line.Length];
            for (var i = 0; i < line.Length; i++)
            {
                if (!possibleBlack[i] && !possibleWhite[i])
                    throw new ArgumentException("Incorrect crossword");

                if (possibleBlack[i] != possibleWhite[i]) 
                    ans[i] = possibleWhite[i] ? Cell.White : Cell.Black;

                if (line[i] != Cell.Unknown &&
                    ans[i] != Cell.Unknown &&
                    ans[i] != line[i]) // если мы знали до этого, а нашли другое -- плохо.
                    throw new ArgumentException("Incorrect crossword");

                if (line[i] != Cell.Unknown) // если мы знаем цвет заранее -- так и оставим.
                    ans[i] = line[i];
            }
            return ans;
        }

        bool SomethingRecursion(int startIndex, int blockIndex) // осторожно, опасно
        {
            var endOfBlock = startIndex + lineInfo[blockIndex]; // переменная, делающая код няшным.

            for (var i = startIndex; i < endOfBlock; i++) // проверяем на то, что текущий блок можно разместить.
                if (line[i] == Cell.White)
                    return false;
            if (blockIndex != lineInfo.Count - 1) // если не последний
            {
                var res = false;
                for (var startNext = endOfBlock + 1;
                    startNext <= line.Length - lineInfo[blockIndex + 1];
                    startNext++)
                {
                    if (line[startNext - 1] == Cell.Black) // если где-то посерединке появилась черная -- нам уже не подойдет нигде.
                        break;
                    if (SomethingRecursion(startNext, blockIndex + 1)) // если такой вариант оказался норм, значит можно сразу расставить.
                    {
                        res = true; //и такой вариант тоже может быть.
                        for (var i = startIndex; i < endOfBlock; i++)
                            possibleBlack[i] = true;
                        for (var i = endOfBlock; i < startNext; i++)
                            possibleWhite[i] = true;
                    }
                }
                return res;
            }
            for (var i = endOfBlock; i < line.Length; i++)
                if (line[i] == Cell.Black) //если после последнего блока есть еще черные -- плохо
                    return false;
            for (var i = startIndex; i < endOfBlock; i++)
                possibleBlack[i] = true;
            for (var i = endOfBlock; i < line.Length; i++)
                possibleWhite[i] = true;
            return true;

        }
    }
}
