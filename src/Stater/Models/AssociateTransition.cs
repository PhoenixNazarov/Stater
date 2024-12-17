using System.Collections.Generic;
using Avalonia;
using Stater.Utils;

namespace Stater.Models;

public record AssociateTransition(
    State? Start,
    State? End,
    Transition Transition,
    List<Point> ArrowPoints
) : DrawArrows
{
    public Point StartPoint => Transition.LinePoints[0];
    public Point EndPoint => Transition.LinePoints[^1];
    public Point LeftArrowPoint => ArrowPoints[0];
    public Point RightArrowPoint => ArrowPoints[1];
    public Point EndArrowPoint => EndPoint;
    
    // public Point NamePoint => Transition.NamePoint;
    public Point NamePoint => DrawUtils.GetTransitionNamePoint(Transition.LinePoints);
    public string Name => Transition.Name;
    public string Color => Transition.Color;
    
    public List<Point>? LinePoints => Transition.LinePoints;
}