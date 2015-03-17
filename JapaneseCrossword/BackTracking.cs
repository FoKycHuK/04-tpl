using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    //todo: имя класса не совпадает с именем файла, в котором он находится
    public class BackTracker
    {
        BaseCrosswordSolver solver;

        public BackTracker(BaseCrosswordSolver solver)
        {
            this.solver = solver;
        }

        //todo: префикс WithBackTracking лишний, по контексту класса можно догадаться как он находит ответ
        public Crossword GetAnswerWithBackTracking(Crossword crossword)
        {
            try
            {
                solver.IterateLineLook(crossword);
            }
            //todo: это бесполезное исключение, солвер вполне бы мог возвращать вместо него особый код ответа либо null. на его обработку среда затратит кучу ресурсов, не нужно бросать его
            catch (IncorrectLineUpdaterInputDataException)
            {
                return null;
            }
            Point unknown = null;
            //todo: почему сразу не выходим, когда нашли точку unknown? это будет работать, но сбивает с толку
            for(var i = 0; i < crossword.RowCount; i++)
                for (var j = 0; j < crossword.ColumnCount; j++)
                    if (crossword.Field[i, j] == Cell.Unknown)
                        unknown = new Point(i, j);
            if (unknown == null)
                return crossword;
            var supposition = crossword.Copy();
            supposition.Field[unknown.X, unknown.Y] = Cell.White;
            var answer = GetAnswerWithBackTracking(supposition);
            if (answer != null)
                return answer;
            supposition.Field[unknown.X, unknown.Y] = Cell.Black;
            return GetAnswerWithBackTracking(supposition);
        }
    }
}
