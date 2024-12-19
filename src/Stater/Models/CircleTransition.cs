using System.Collections.Generic;
using Avalonia;
using Stater.Utils;

namespace Stater.Models;

public record class CircleTransition(
    State Start,
    double X,
    double Y,
    double Radius,
    Transition Transition,
    List<Point> ArrowPoints
) : DrawArrows(Start, Start, Transition)
{
    public double Diameter => Radius * 2;
    
    public double Width => Radius;
    
    public double Height => Diameter;
    
    public double Left => X - Radius;
    
    public double Top => Y - Radius;
    
    public string Name => Transition.Name;
    
    public Point StartPoint => Transition.LinePoints[0];
    public Point EndPoint => Transition.LinePoints[^1];
    
    public Point LeftArrowPoint => ArrowPoints[0];

    public Point RightArrowPoint => ArrowPoints[1];
    public Point EndArrowPoint => EndPoint;
    
    public string Color => Transition.Color;
    
    // public Point NamePoint => Transition.NamePoint;

    public Point NamePoint => new (X, Top);
}