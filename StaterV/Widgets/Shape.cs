using System;
using System.Collections.Generic;
using StaterV.Utility;
using StaterV.Windows;

namespace StaterV.Widgets
{
    [Serializable]

    /// Обобщенный класс для прямоугольных виджетов.
    public abstract class Shape : Widget
    {
        protected Shape() : base() {}

        public Shape(WindowBase _window) : base(_window)
        {
        }

        protected List<Arrow> incomingArrows = new List<Arrow>();
        protected List<Arrow> outgoingArrows = new List<Arrow>();

        public List<Arrow> IncomingArrows
        {
            get { return incomingArrows; }
            set { incomingArrows = value; }
        }

        public List<Arrow> OutgoingArrows
        {
            get { return outgoingArrows; }
            set { outgoingArrows = value; }
        }

        public abstract void DetermineWidth();

        private Dot FindIntersection(float x1, float y1, float x2, float y2)
        {
            Dot s = new Dot();
            s.x = x1;
            s.y = y1;
            Dot e = new Dot();
            e.x = x2;
            e.y = y2;
            return FindIntersection(s, e);
        }

        private Dot FindIntersection(Dot s, Dot e)
        {
            Dot res = new Dot();
            Dot tl = new Dot();
            Dot tr = new Dot();
            Dot dl = new Dot();
            Dot dr = new Dot();
            tl.x = X;
            tl.y = Y;
            tr.x = X + Width;
            tr.y = Y;
            dl.x = X;
            dl.y = Y + Height;
            dr.x = X + Width;
            dr.y = Y + Height;

            //Пробуем пересечь с верхней границей.
            if (MathSupport.IntersectLines(tl, tr, s, e, out res))
            {
                return res;
            }
            //Пробуем пересечь с правой границей.
            if (MathSupport.IntersectLines(dr, tr, s, e, out res))
            {
                return res;
            }
            //Пробуем пересечь с левой границей.
            if (MathSupport.IntersectLines(tl, dl, s, e, out res))
            {
                return res;
            }
            //Пробуем пересечь с нижней границей.
            if (MathSupport.IntersectLines(dl, dr, s, e, out res))
            {
                return res;
            }
            return res;
        }

        public virtual bool IntersectLine(Dot c1, Dot c2, out Dot intersection)
        {
            return IntersectLine(c1.x, c1.y, c2.x, c2.y, out intersection);
        }

        public virtual bool IntersectLine(float x1, float y1, float x2, float y2, out Dot intersection)
        {
            intersection = new Dot();
            if (IntersectsWith(x1, y1))
            {
                if (IntersectsWith(x2, y2))
                {
                    return false;
                }

                intersection = FindIntersection(x1, y1, x2, y2);
                return true;
            }

            if (IntersectsWith(x2, y2))
            {
                intersection = FindIntersection(x1, y1, x2, y2);
                return true;
            }
            return false;
        }

        public override bool IntersectsWith(float dotX, float dotY)
        {
            return ((dotX >= X) && (dotX <= X + Width) && (dotY >= Y) &&
                    (dotY <= Y + Height));
        }

        public override bool IntersectsWith(float rectX, float rectY, float rectH, float rectW)
        {
            return IntersectsWith(rectX, rectY)
                   || IntersectsWith(rectX, rectY + rectH)
                   || IntersectsWith(rectX + rectW, rectY + rectH)
                   || IntersectsWith(rectX + rectW, rectY);
        }

        /// <summary>
        /// Возвращает кординаты для конца дуги.
        /// </summary>
        /// <param name="coord">Координаты для одного из концов дуги.</param>
        public void GetPosition(out Utility.Dot coord)
        {
            coord.x = X + Width/2;
            coord.y = Y + Height/2;
        }
        /// <summary>
        /// Возвращает координаты и номер для отрисовки дуги. (Не очень удачная 
        /// функция - мультидуги пересекаются).
        /// </summary>
        /// <param name="arrow">Дуга</param>
        /// <param name="coord">Координаты для одного из концов дуги.</param>
        /// <param name="number">Номер дуги среди дуг с одинаковыми началом и концом.</param>
        /// <param name="amount">Количество дуг с одинаковыми c arrow началом и концом.</param>
        public void GetPosition(Arrow arrow, out Utility.Dot coord,  int number,
                                out int amount)
        {
            number = 0;
            coord.x = X + Width/2;
            coord.y = Y + Height/2;
            //Если arrow начинается здесь. 
            if (arrow.Start == this)
            {
                //Проходим все исходящие дуги, пока не дошли до arrow.
                foreach (var outgoingArrow in OutgoingArrows)
                {
                    if (arrow == outgoingArrow)
                    {
                        break;
                    }
                    //Ищем дуги с таким же концом, как у arrow.
                    //Если нашли, 
                    if (arrow.End == outgoingArrow.End)
                    {
                        //то увеличиваем number на 1.
                        number++;
                    }
                }
            }
            else
            {
                //Проходим все исходящие дуги с такими же концами.
                foreach (var outgoingArrow in OutgoingArrows)
                {
                    if (arrow.Start == outgoingArrow.End)
                    {
                        number++;
                    }
                }

                //Проходим все входящие дуги, пока не дошли до arrow.
                foreach (var incomingArrow in IncomingArrows)
                {
                    if (arrow == incomingArrow)
                    {
                        break;
                    }
                    //Ищем дуги с таким же концом, как у arrow.
                    //Если нашли, 
                    if (arrow.Start == incomingArrow.Start)
                    {
                        //то увеличиваем number на 1.
                        number++;
                    }
                }
            }

            //Вычисляем общее количество таких дуг. Можно эффективнее, но так проще.
            amount = 0;
            foreach (var outgoingArrow in OutgoingArrows)
            {
                if ((arrow.Start == outgoingArrow.Start) && 
                    (arrow.End == outgoingArrow.End))
                {
                    amount++;
                }
                else if ((arrow.End == outgoingArrow.Start)&& 
                         (arrow.Start == outgoingArrow.End))
                {
                    amount++;
                }
            }
            foreach (var incomingArrow in IncomingArrows)
            {
                if ((arrow.Start == incomingArrow.Start) &&
                    (arrow.End == incomingArrow.End))
                {
                    amount++;
                }
                else if ((arrow.End == incomingArrow.Start) &&
                         (arrow.Start == incomingArrow.End))
                {
                    amount++;
                }
            }
        }

        public override StaterV.Commands.Command PrepareDeleteCommand()
        {
            Commands.CompositeParams p = new Commands.CompositeParams();
            p.Window = Window;
            foreach (var inArr in IncomingArrows)
            {
                p.Commands.Add(inArr.PrepareDeleteCommand());
            }

            foreach (var outArr in OutgoingArrows)
            {
                p.Commands.Add(outArr.PrepareDeleteCommand());
            }

            Commands.CompositeParams pShape = new Commands.CompositeParams();
            pShape.TheWidget = this;
            pShape.Window = Window;
            Commands.DeleteShapeCommand cmdShape = new Commands.DeleteShapeCommand(pShape);
            p.Commands.Add(cmdShape);

            Commands.CompositeCommand cmd = new Commands.CompositeCommand(p);
            return cmd;
        }
    }
}