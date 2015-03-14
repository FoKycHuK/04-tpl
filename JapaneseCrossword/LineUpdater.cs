using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    public class LineUpdater
    {
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
        }

        public Cell[] GetAnswer()
        {
            //todo: этот цикл не нужен, если занести его в рекурсию, тогда в рекурсивной функции мы будем перебирать начало текущего блока, а не следующего. Тогда не будет этого дублирования
            for (var i = 0; i <= line.Length - lineBlocks[0]; i++)
            {
                if (i > 0 && line[i - 1] == Cell.Black)
                    break; 
                if (PermutationExists(i, 0))
                    UpdatePossibilities(false, 0, i);
            }
            var ans = new Cell[line.Length];
            for (var i = 0; i < line.Length; i++)
            {
                if (!possibleBlack[i] && !possibleWhite[i])
                    throw new IncorrectLineUpdaterInputDataException();

                if (possibleBlack[i] != possibleWhite[i]) 
                    ans[i] = possibleWhite[i] ? Cell.White : Cell.Black;

                //todo: если так получилось, то это какой-то плохой back-tracking, он не должен выдавать неконсистентные результаты. Лучше сразу отрубать такие ветки перебора (это и быстрее будет)
                //if (line[i] != Cell.Unknown &&
                //    ans[i] != Cell.Unknown &&
                //    ans[i] != line[i]) // если мы знали до этого, а нашли другое -- плохо.
                //    throw new IncorrectLineUpdaterInputDataException();

                //if (line[i] != Cell.Unknown) // если мы знаем цвет заранее -- так и оставим.
                //    ans[i] = line[i];
            }
            return ans;
        }

        //todo: название метода совершенно не помогает понять, что он делает. Как его лучше обозвать, чтобы была понятна семантика?
        //todo: число комментариев просто зашкаливает, но они повторяют код, лучше написать код понятней, чем держать в нем комменты
        bool PermutationExists(int startIndex, int blockIndex) // осторожно, опасно
        {
            //todo: за время существования LineUpdater будет происходить много перекрывающихся вызовов рекурсии, которые в данном случае отлично кэшируются, это ускорит алгоритм

            var endOfBlock = startIndex + lineBlocks[blockIndex];

            if (!CanPlaceBlackBlock(startIndex, endOfBlock))
                return false;
            if (blockIndex != lineBlocks.Count - 1)
            {
                var res = false;
                for (var startNext = endOfBlock + 1;
                    startNext <= line.Length - lineBlocks[blockIndex + 1];
                    startNext++)
                {
                    if (line[startNext - 1] == Cell.Black) // если где-то посерединке появилась черная -- нам уже не подойдет нигде.
                        break;
                    if (PermutationExists(startNext, blockIndex + 1)) // если такой вариант оказался норм, значит можно сразу расставить.
                    {
                        res = true; //и такой вариант тоже может быть.
                        UpdatePossibilities(true, startIndex, endOfBlock);
                        UpdatePossibilities(false, endOfBlock, startNext);
                    }
                }
                return res;
            }
            for (var i = endOfBlock; i < line.Length; i++)
                if (line[i] == Cell.Black) //если после последнего блока есть еще черные -- плохо
                    return false;
            UpdatePossibilities(true, startIndex, endOfBlock);
            UpdatePossibilities(false, endOfBlock, line.Length);
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
            for (var i = startIndex; i < endOfBlock; i++) // проверяем на то, что текущий блок можно разместить.
                if (line[i] == Cell.White)
                    return false;
            return true;
        }
    }
}
