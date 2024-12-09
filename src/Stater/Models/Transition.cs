using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Animation;

namespace Stater.Models;

public enum TypeArrow { Manhattan, Pifagor}

public record Transition(
    Guid Guid,
    string Name,
    Guid? Start,
    Guid? End, 
    TypeArrow Type,
    List<Point>? LinePoints,
    Condition? Condition
)
{
    public Transition() : this(
        Guid.NewGuid(),
        "Transition",
        null,
        null,
        TypeArrow.Pifagor,
        null,
        null
    )
    {
    }
}