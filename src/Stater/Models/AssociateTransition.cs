using System.Collections.Generic;
using Avalonia;

namespace Stater.Models;

public record AssociateTransition(
    State? Start,
    State? End,
    List<Point> LinePoints,
    List<Point> ArrowPoints,
    Transition Transition,
    TypeArrow Type
) {
    public Point? StartPoint => LinePoints[0];
    public Point? EndPoint => LinePoints[^1];
    
    public Point? LeftArrowPoint => ArrowPoints[0];
    
    public Point? RightArrowPoint => ArrowPoints[1];
    public Point? StartArrowPoint => LinePoints[^2];
    public Point? EndArrowPoint => EndPoint;
    public string Name => Transition.Name;
    public string Color => "Black";
}