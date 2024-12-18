using System;
using System.Collections.Generic;
using Avalonia.Animation;

namespace Stater.Models;

public record Transition(
    Guid Guid,
    string Name,
    Guid? Start,
    Guid? End,
    Condition? Condition,
    Event? Event
)
{
    public Transition() : this(
        Guid.NewGuid(),
        "Transition",
        null,
        null,
        null,
        null
    )
    {
    }
}