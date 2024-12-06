using System;
using System.Collections.Generic;
using Avalonia.Animation;

namespace Stater.Models;

public record Transition(
    Guid Guid,
    string Name,
    Guid? Start,
    Guid? End,
    Condition? Condition
)
{
    public Transition() : this(
        new Guid(),
        "Transition",
        null,
        null,
        null
    )
    {
    }
}