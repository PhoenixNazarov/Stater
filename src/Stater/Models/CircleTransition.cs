using System.Collections.Generic;
using Avalonia;
using Stater.Utils;

namespace Stater.Models;

public record class CircleTransition(
    State? Start,
    float X,
    float Y,
    float Radius,
    List<Point> ArrowPoints,
    Point NamePoint
) : DrawArrows
{
    
}