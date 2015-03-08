﻿using System;
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

        public UpdateLine(Cell[] line, List<int> lineInfo)
        {
            this.line = line;
            possibleBlack = new bool[line.Length];
            possibleWhite = new bool[line.Length];
            this.lineInfo = lineInfo;
        }

        public Cell[] GetAnswer()
        {
            for (var i = 0; i <= line.Length - lineInfo[0]; i++)
                if (SomethingRecursion(i, 0))
                {
                    for (var j = 0; j < i; j++)
                        possibleWhite[j] = true;
                }
            var ans = new Cell[line.Length];
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] != Cell.Unknown)
                    continue;
                if (possibleBlack[i] != possibleWhite[i])
                    ans[i] = possibleWhite[i] ? Cell.White : Cell.Black;
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
                    startNext <= line.Length - lineInfo[blockIndex + 1] + 1;
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
