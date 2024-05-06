using System;
using StaterV.Widgets;

namespace StaterV.Utility
{
    [Serializable]
    public struct Dot
    {
        public float x;
        public float y;
    }

    public struct Size
    {
        public float width;
        public float height;
    }

    public enum SidePieces
    {
        Top,
        Right,
        Bottom,
        Left,
    }

    public class MathSupport
    {
        public static float DeltaEqual = 0.0001f;

        public static bool Between(double v, double lower, double upper)
        {
            return (v <= upper) && (v >= lower);
        }

        public static bool FineBetween(double v, double bound1, double bound2, double prescision)
        {
            return (v <= Math.Max(bound1, bound2) + prescision) && 
                   (v >= Math.Min(bound1, bound2) - prescision);
        }

        /// <summary>
        /// Определяет, лежит ли точка на отрезке с заданной точностью.
        /// </summary>
        /// <param name="testing">Проверяемая точка</param>
        /// <param name="s">Начало отрезка</param>
        /// <param name="e">Конец отрезка</param>
        /// <param name="prescision">точность</param>
        /// <returns>Лежит ли точка на отрезке</returns>
        public static bool IsDotCrossesSegLine(Dot testing, Dot s, Dot e, float prescision)
        {
            if ((!FineBetween(testing.x, s.x, e.x, prescision)) ||
                (!FineBetween(testing.y, s.y, e.y, prescision)))
            {
                return false;
            }

            if (IsVertical(s, e, 10))
            {
                var dist = (testing.x - s.x);
                if (Math.Abs(dist) < prescision)
                {
                    return true;
                }
                return false;
            }

            if (IsHorizontal(s, e, 10))
            {
                var dist = (testing.y - s.y);
                if (Math.Abs(dist) < prescision)
                {
                    return true;
                }
                return false;
            }

            var xVal = (testing.x - s.x) / (e.x - s.x);
            var yVal = (testing.y - s.y) / (e.y - s.y);

            var length = Math.Sqrt((s.x - e.x)*(s.x - e.x) + (s.y - e.y)*(s.y - e.y));
            var relativePrescision = prescision/length;

            return (Math.Abs(xVal - yVal) < relativePrescision);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <param name="prescision">in degrees</param>
        /// <returns></returns>
        public static bool IsHorizontal(Dot s, Dot e, float prescision)
        {
            if (Math.Abs(s.y - e.y) < 2)
            {
                return true;
            }

            var angle = GetLineAngle(s, e);

            if (Math.Abs(angle) < prescision ||
                //Math.Abs(angle - 90) < prescision ||
                Math.Abs(angle - 180) < prescision ||
                //Math.Abs(angle - 270) < prescision ||
                Math.Abs(angle - 360) < prescision)
            {
                return true;
            }

            return false;
        }

        public static bool IsVertical(Dot s, Dot e, float prescision)
        {
            if (Math.Abs(s.x - e.x) < 2)
            {
                return true;
            }

            var angle = GetLineAngle(s, e);

            if (Math.Abs(angle - 90) < prescision ||
                Math.Abs(angle - 270) < prescision)
            {
                return true;
            }

            return false;
        }


        public static bool IsParallelAxis(Dot s, Dot e, float prescision)
        {
            if (Math.Abs(s.y - e.y) < prescision)
            {
                return true;
            }

            if (Math.Abs(s.x - e.x) < prescision)
            {
                return true;
            }

            var angle = GetLineAngle(s, e);

            if (Math.Abs(angle) < prescision || 
                Math.Abs(angle - 90) < prescision ||
                Math.Abs(angle - 180) < prescision ||
                Math.Abs(angle - 270) < prescision ||
                Math.Abs(angle - 360) < prescision)
            {
                return true;
            }

            return false;
        }

        public static bool IsDotCrossesSegLine(Dot testing, Dot s, Dot e)
        {
            return IsDotCrossesSegLine(testing, s, e, DeltaEqual);
        }

        /// <summary>
        /// Определяет, лежит ли точка, находящаяся на прямой, содержащей отрезок, внутри этого отрезка.
        /// </summary>
        /// <param name="testing">Точка</param>
        /// <param name="s">Начало отрезка</param>
        /// <param name="e">Конец отрезка</param>
        /// <returns>Лежит ли точка внутри отрезка</returns>
        public static bool IsDotInsideSegLine(Dot testing, Dot s, Dot e)
        {
            return (((testing.x >= s.x - DeltaEqual && testing.x - DeltaEqual <= e.x) ||
                     (testing.x - DeltaEqual <= s.x && testing.x >= e.x - DeltaEqual)) &&
                    ((testing.y >= s.y - DeltaEqual && testing.y - DeltaEqual <= e.y) ||
                     (testing.y - DeltaEqual <= s.y && testing.y >= e.y - DeltaEqual)));
        }

        /// <summary>
        /// Определяет, лежит ли точка на окружности с заданной точностью.
        /// </summary>
        /// <param name="testing">Проверяемая точка</param>
        /// <param name="center">Центр окружности</param>
        /// <param name="radius">Радиус окружности</param>
        /// <param name="prescision">Точность</param>
        /// <returns>Лежит ли точка на окружности</returns>
        public static bool IsDotCrossesCircle(Dot testing, Dot center, float radius, float prescision)
        {
            var valX = (testing.x - center.x)*(testing.x - center.x);
            var valY = (testing.y - center.y) * (testing.y - center.y);
            return ( ((valX + valY - radius*radius) < prescision*radius) &&
                     ((valX + valY - radius * radius) > -prescision * radius * 4 / 5) );
        }


        public static bool IntersectLines(Dot s1, Dot e1, Dot s2, Dot e2, out Dot intersect)
        {
            intersect = new Dot();
            var A1 = s1.y - e1.y;
            var B1 = e1.x - s1.x;
            var C1 = s1.x*e1.y - e1.x*s1.y;

            var A2 = s2.y - e2.y;
            var B2 = e2.x - s2.x;
            var C2 = s2.x*e2.y - e2.x*s2.y;

            var denominator = A1*B2 - A2*B1;
            if (Math.Abs(denominator) > DeltaEqual)
            {
                //Прямые пересекаются.
                intersect.x = (C2*B1 - C1*B2)/denominator;
                intersect.y = (A2*C1 - A1*C2)/denominator;
                
                //Проверим, что точка пересечения попадает в оба отрезка.
                if (IsDotInsideSegLine(intersect, s1, e1) &&
                    IsDotInsideSegLine(intersect, s2, e2))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public static void GetLoopBorderPosition(Dot pos, Shape _shape, out Dot borderPos, out SidePieces side)
        {
            float []distance = new float[4];

            //Расстояние до верхней границы прямоугольного виджета.
            distance[0] = Math.Abs(pos.y - _shape.Y);

            //Расстояние до правой границы.
            distance[1] = Math.Abs(pos.x - (_shape.X + _shape.Width));

            //Расстояние до нижней границы.
            distance[2] = Math.Abs(pos.y - (_shape.Y + _shape.Height));

            //Расстояние до левой границы.
            distance[3] = Math.Abs(pos.x - _shape.X);

            int minPos = 0;

            for (int i = 0; i < distance.Length; i++)
            {
                if (distance[i] < distance[minPos])
                {
                    minPos = i;
                }
            }

            side = (SidePieces) minPos;

            borderPos = new Dot();
            switch (side)
            {
                case SidePieces.Top:
                    borderPos.x = pos.x;
                    borderPos.y = _shape.Y;
                    break;
                case SidePieces.Right:
                    borderPos.x = _shape.X + _shape.Width;
                    borderPos.y = pos.y;
                    break;
                case SidePieces.Bottom:
                    borderPos.x = pos.x;
                    borderPos.y = _shape.Y + _shape.Height;
                    break;
                case SidePieces.Left:
                    borderPos.x = _shape.X;
                    borderPos.y = pos.y;
                    break;
            }
            /*
            var vertLeft = true;
            var horizTop = true;

            var dx = Math.Abs(pos.x - _shape.X);
            var potentialDx = Math.Abs(pos.x - (_shape.X + _shape.Width));
            if (potentialDx < dx)
            {
                dx = potentialDx;
                vertLeft = false;
            }

            var dy = Math.Abs(pos.y - _shape.Y);
            var potentialDy = Math.Abs(pos.y - (_shape.Y + _shape.Height));
            if (potentialDy < dy)
            {
                dy = potentialDy;
                horizTop = false;
            }

            if (dx < dy)
            {
                //Ближе всего вертикальная.
                borderPos.y = pos.y;
                borderPos.x = (vertLeft) ? _shape.X : _shape.X + _shape.Width;
                side = true;
            }
            else
            {
                borderPos.x = pos.x;
                borderPos.y = (horizTop) ? _shape.Y : _shape.Y + _shape.Height;
                side = false;
            }
            */
        }

        public static double GetLineAngle(Dot s, Dot e)
        {
            var A = s.y - e.y;
            var B = e.x - s.x;

            if (Math.Abs((float) ((sbyte) B)) < DeltaEqual)
            {
                return (s.y > e.y) ? 270 : 90;
            }

            var k = -A/B;
            var angle = Math.Atan(k) * 180 / Math.PI;

            
            if (e.x < s.x)
            {
                //angle = (180 - angle) % 360;
                //angle = -angle;
                angle = (angle + 180)%360;
            }
             

            if (e.y < s.y)
            {
                //angle = -angle;
            }

            return angle;
        }

        /// <summary>
        /// Создает начальную точку отрезка, если заданы его угол, конечная точка
        /// и длина.
        /// </summary>
        public static void CreateStartSegLineAngle(double angle, float length, Dot e, out Dot s)
        {
            s = new Dot();
            if (Math.Abs(angle - 90) < DeltaEqual)
            {
                //Угол = 90 градусов. => Вверх.
                s.x = e.x;
                s.y = e.y + length;
            }

            if ((Math.Abs(angle - 270) < DeltaEqual) ||
                (Math.Abs(angle + 90) < DeltaEqual))
            {
                //Вниз.
                s.x = e.x;
                s.y = e.y - length;
            }

            //var k = Math.Tan(angle * Math.PI / 180);
            //var b = e.y - k*e.x;
            var lx = length * Math.Cos(angle * Math.PI / 180);
            var ly = length * Math.Sin(angle * Math.PI / 180);
            s.x = e.x - (float)lx;
            s.y = e.y - (float)ly;
        }

        /// <summary>
        /// Вычисляет координату конца дуги внутри прямоугольного виджета.
        /// </summary>
        /// <param name="coord">Координата центра откуда начинать отсчет.</param>
        /// <param name="pos">Номер дуги среди дуг с одинаковыми началом и концом.</param>
        /// <param name="amount">Число дуг с такими началом и концом.</param>
        /// <param name="height">Высота прямоугольного виджета.</param>
        /// <param name="width">Ширина прямоугольного виджета.</param>
        /// <returns></returns>
        public static Dot CalcPositionInsideBox(Dot coord, int pos, int amount, float height, float width)
        {
            Dot res = new Dot();

            var radius = (height/2)*(5.0/6.0);
            var angle = 2*Math.PI*pos/amount + Math.PI/8;
            res.x = (float)(coord.x + (radius * Math.Sin(angle)) * width / height);
            res.y = (float)(coord.y + (radius*Math.Cos(angle)));

            return res;
        }
    }
}