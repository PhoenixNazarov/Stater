using System;
using System.Collections.Generic;
using Avalonia;
using Stater.Models;

namespace Stater.Utils;

public static class DrawUtils
{
    private static double PifagoreDistatnce(Point p1, Point p2)
    {
        return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
    }

    private static List<Point> GeneratePoints(State s)
    {
        var left = new Point(s.Left, s.Y);
        var right = new Point(s.Right, s.Y);
        var top = new Point(s.X, s.Top);
        var bottom = new Point(s.X, s.Bottom);
        return [left, right, top, bottom];
    }

    private static List<Point> GeneratePifagorPath(State start, State end)
    {
        var startPoints = GeneratePoints(start);
        var endPoints = GeneratePoints(end);
        var startPoint = startPoints[0];
        var endPoint = endPoints[0];
        var dist = PifagoreDistatnce(startPoint, endPoint);
        foreach (var p1 in startPoints)
        {
            foreach (var p2 in endPoints)
            {
                if (dist <= PifagoreDistatnce(p1, p2)) continue;
                dist = PifagoreDistatnce(p1, p2);
                startPoint = p1;
                endPoint = p2;
            }
        }

        return [startPoint, endPoint];
    }

    private static List<Point> GenerateManhattanPath(State start, State end)
    {
        var startPoints = GeneratePoints(start);
        var endPoints = GeneratePoints(end);
        var startPoint = startPoints[0];
        var endPoint = endPoints[0];
        var dist = PifagoreDistatnce(startPoint, endPoint);
        foreach (var p1 in startPoints)
        {
            foreach (var p2 in endPoints)
            {
                if (dist <= PifagoreDistatnce(p1, p2)) continue;
                dist = PifagoreDistatnce(p1, p2);
                startPoint = p1;
                endPoint = p2;
            }
        }

        var startP = new Point(startPoint.X, startPoint.Y);
        
        return [startPoint, endPoint];
    }

    private static Point GetTransitionNamePoint(List<Point> p)
    {
        if (p.Count % 2 == 0)
        {
            var right = p[p.Count / 2];
            var left = p[p.Count / 2 - 1];
            return new ((right.X + left.X) / 2, (right.Y + left.Y) / 2);
        }
        return p[p.Count / 2];
    }
    
    public static AssociateTransition GetAssociateTransition(State s, State e, Transition t)
    {
        List<Point> linePoints = [];
        switch (t.Type)
        {
            case TypeArrow.Pifagor:
            {
                linePoints = GeneratePifagorPath(s, e);
                break;
            }
            case TypeArrow.Manhattan:
            {
                linePoints = GenerateManhattanPath(s, e);
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
        return new AssociateTransition(
            Transition: t,
            LinePoints: linePoints,
            ArrowPoints: GetArrowPoints(linePoints[^2], linePoints[^1]),
            NamePoint: GetTransitionNamePoint(linePoints),
            Start: s,
            End: e,
            Type: t.Type
        );
    }

    private const double Eps = 1e-9;

    private static double NotZeroDivisor(double b)
    {
        var bAbs = Math.Abs(b);
        var bSign = Math.Sign(b);
        return bSign * (bAbs + Eps);
    }
    
    private const double LengthArrow = 20;
    private const double AngleArrow = Math.PI / 6;
    
    private static List<Point> GetArrowPoints(Point p1, Point p2)
    {
        var angle = Math.Atan2(p2.Y - p1.Y , NotZeroDivisor(p2.X - p1.X));
        var leftAngle = angle + AngleArrow;
        var rightAngle = angle - AngleArrow;
        
        var leftP = new Point(p2.X - LengthArrow * Math.Cos(leftAngle), p2.Y - LengthArrow * Math.Sin(leftAngle));
        var rightP = new Point(p2.X - LengthArrow * Math.Cos(rightAngle), p2.Y - LengthArrow * Math.Sin(rightAngle));
        return [leftP, rightP];
    }
}