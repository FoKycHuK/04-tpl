using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    public class LineUpdater
    {
        //Dictionary<Point, bool> cache;
        Cell[] line;
        List<int> lineBlocks;
        bool[] possibleBlack;
        bool[] possibleWhite;

        public LineUpdater(Cell[] line, List<int> lineInfo)
        {
            this.line = line;
            possibleBlack = new bool[line.Length];
            possibleWhite = new bool[line.Length];
            this.lineBlocks = lineInfo;
            //cache = new Dictionary<Point, bool>();
        }

        public Cell[] GetAnswer()
        {
            PermutationExists(-1, -1);

            var ans = new Cell[line.Length];
            for (var i = 0; i < line.Length; i++)
            {
                if (!possibleBlack[i] && !possibleWhite[i])
                    throw new IncorrectLineUpdaterInputDataException();

                if (possibleBlack[i] != possibleWhite[i])
                    ans[i] = possibleWhite[i] ? Cell.White : Cell.Black;
            }
            return ans;
        }

        bool PermutationExists(int startIndex, int blockIndex)
        {
            var point = new Point(startIndex, blockIndex);
            //todo: эти комментарии незачем держать в коде, историю о них помнит Git
            //if (cache.Keys.Contains(point))
            //    return cache[point];
            var endOfBlock = startIndex;
            if (blockIndex != -1)
            {
                endOfBlock += lineBlocks[blockIndex];
                if (!CanPlaceBlackBlock(startIndex, endOfBlock))
                {
                    //cache.Add(point, false);
                    return false;
                }
            }
            if (blockIndex != lineBlocks.Count - 1)
            {
                var res = false;
                for (var startNext = endOfBlock + 1;
                    startNext <= line.Length - lineBlocks[blockIndex + 1];
                    startNext++)
                {
                    if (startNext != 0 && line[startNext - 1] == Cell.Black) // если где-то посерединке появилась черная -- нам уже не подойдет нигде.
                        break;
                    if (PermutationExists(startNext, blockIndex + 1)) // если такой вариант оказался норм, значит можно сразу расставить.
                    {
                        res = true; //и такой вариант тоже может быть.
                        if (blockIndex != -1)
                        {
                            UpdatePossibilities(true, startIndex, endOfBlock);
                            UpdatePossibilities(false, endOfBlock, startNext);
                        }
                        else
                            UpdatePossibilities(false, 0, startNext);
                    }
                }
                //cache.Add(point, res);
                return res;
            }
            for (var i = endOfBlock; i < line.Length; i++)
                if (line[i] == Cell.Black) //если после последнего блока есть еще черные -- плохо
                {
                    //cache.Add(point, false);
                    return false;
                }
            UpdatePossibilities(true, startIndex, endOfBlock);
            UpdatePossibilities(false, endOfBlock, line.Length);
            //cache.Add(point, true);
            return true;

        }

        void UpdatePossibilities(bool isBlack, int from, int to)
        {
            var possible = isBlack ? possibleBlack : possibleWhite;
            for (var i = from; i < to; i++)
                possible[i] = true;
        }

        bool CanPlaceBlackBlock(int startIndex, int endOfBlock)
        {
            for (var i = startIndex; i < endOfBlock; i++)
                if (line[i] == Cell.White)
                    return false;
            return true;
        }
    }
}
