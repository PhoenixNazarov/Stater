using System.Collections.Generic;
using Avalonia;
using Stater.Utils;

namespace Stater.Models;

public record class CircleTransition(
    State? Start,
    double X,
    double Y,
    double Radius,
    List<Point> ArrowPoints,
    Point NamePoint,
    string Name
) : DrawArrows
{
    public string Color => "Black";
    
    public double Diameter => Radius * 2;
    
    public double Width => Radius;
    
    public double Height => Diameter;
    
    public double Left => X - Radius;
    
    public double Top => Y - Radius;
}