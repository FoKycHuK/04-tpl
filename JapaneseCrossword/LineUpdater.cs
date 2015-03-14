using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    //todo: Не самая удачная абстракция, к тому же она много знает о внешнем мире. Лучше сделать LineAnalyzer, который просто по линии вангует возможные состояния её клеток
    //todo: Оставить только один метод Analyze(почти GetAnswer), который знать не знает про кроссворд, отличия колонок от строк, очереди и т.д.
    public class LineUpdater
    {
        Cell[] line;
        //todo: когда происходит обращение к lineInfo, то по названию совсем не понимаешь с чем имеешь дело и в чем отличие от line, нужно смотреть код.
        //todo: здесь больше подошла бы метафора lineGroups или lineBlocks, или просто groups или blocks
        List<int> lineInfo;
        bool[] possibleBlack;
        bool[] possibleWhite;

        //todo: этот метод лучше вынести наверх в CrosswordSolver, он слишком много знает о том, как используют LineUpdater. К тому же без этого метода проще тестировать будет
        //todo: можно вынести в базовый абстрактный класс BaseCrosswordSolver
        static public void UpdateOneLine(Crossword cs, bool isColumn, int index, Queue<int> otherLinesToWork)
        {
            var line = cs.GetLine(isColumn, index);
            var res = new LineUpdater(line, isColumn ? cs.columns[index] : cs.rows[index]).GetAnswer();
            for (var i = 0; i < line.Length; i++)
                if (res[i] != line[i] && !otherLinesToWork.Contains(i))
                    otherLinesToWork.Enqueue(i);
            cs.SetLine(isColumn, index, res);
        }

        LineUpdater(Cell[] line, List<int> lineInfo)
        {
            this.line = line;
            possibleBlack = new bool[line.Length];
            possibleWhite = new bool[line.Length];
            this.lineInfo = lineInfo;
        }

        Cell[] GetAnswer()
        {
            //todo: этот цикл не нужен, если занести его в рекурсию, тогда в рекурсивной функции мы будем перебирать начало текущего блока, а не следующего. Тогда не будет этого дублирования
            for (var i = 0; i <= line.Length - lineInfo[0]; i++)
            {
                //todo: WTF?! комментарий
                if (i > 0 && line[i - 1] == Cell.Black) // Rest In Peace
                    break; 
                if (SomethingRecursion(i, 0))
                {
                    //todo: везде дублируется эта логика. можно вынести в функцию, тогда будет проще читать и меньше будет вероятность ошибиться с индексами
                    for (var j = 0; j < i; j++)
                        possibleWhite[j] = true;
                }
            }
            var ans = new Cell[line.Length];
            for (var i = 0; i < line.Length; i++)
            {
                if (!possibleBlack[i] && !possibleWhite[i])
                    //todo: очень неправильно кидать в таком контексте ArgumentException. Контекст этого исключения предполагает наличие неправильных параметров, здесь валидируется не это. 
                    //todo: к тому же кажется, что LineUpdater вообще ничего про кроссворд знать не должен
                    throw new ArgumentException("Incorrect crossword");

                if (possibleBlack[i] != possibleWhite[i]) 
                    ans[i] = possibleWhite[i] ? Cell.White : Cell.Black;

                //todo: если так получилось, то это какой-то плохой back-tracking, он не должен выдавать неконсистентные результаты. Лучше сразу отрубать такие ветки перебора (это и быстрее будет)
                if (line[i] != Cell.Unknown &&
                    ans[i] != Cell.Unknown &&
                    ans[i] != line[i]) // если мы знали до этого, а нашли другое -- плохо.
                    throw new ArgumentException("Incorrect crossword");

                if (line[i] != Cell.Unknown) // если мы знаем цвет заранее -- так и оставим.
                    ans[i] = line[i];
            }
            return ans;
        }

        //todo: название метода совершенно не помогает понять, что он делает. Как его лучше обозвать, чтобы была понятна семантика?
        //todo: число комментариев просто зашкаливает, но они повторяют код, лучше написать код понятней, чем держать в нем комменты
        bool SomethingRecursion(int startIndex, int blockIndex) // осторожно, опасно
        {
            //todo: за время существования LineUpdater будет происходить много перекрывающихся вызовов рекурсии, которые в данном случае отлично кэшируются, это ускорит алгоритм

            //todo: бесполезный коммент
            var endOfBlock = startIndex + lineInfo[blockIndex]; // переменная, делающая код няшным.

            //todo: вынести метод CanPlaceBlackBlock - и комментарий не понадобится, все и так будет ясно
            for (var i = startIndex; i < endOfBlock; i++) // проверяем на то, что текущий блок можно разместить.
                if (line[i] == Cell.White)
                    return false;
            //todo: комментарий дублирует понятный код
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
