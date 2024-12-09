using System.Collections.Generic;
using Avalonia;
using Stater.Utils;

namespace Stater.Models;

public record AssociateTransition
(
    State? Start,
    State? End,
    List<Point> LinePoints,
    List<Point> ArrowPoints,
    Point NamePoint,
    Transition Transition,
    TypeArrow Type
) : DrawArrows {
    public Point StartPoint => LinePoints[0];
    public Point EndPoint => LinePoints[^1];
    
    public Point LeftArrowPoint => ArrowPoints[0];
    
    public Point RightArrowPoint => ArrowPoints[1];
    public Point EndArrowPoint => EndPoint;
    public string Name => Transition.Name;
    public string Color => "Black";
}