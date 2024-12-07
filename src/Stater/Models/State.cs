using System;
using System.Collections.Generic;

namespace Stater.Models;

public enum StateType
{
    Common,
    Start,
    End
}

public record State(
    Guid Guid,
    string Name,
    string Description,
    StateType Type,
    float X,
    float Y,
    List<Event> EntryEvents,
    List<Event> ExitEvents
)
{
    public State() : this(
        Guid.NewGuid(),
        "State",
        "",
        StateType.Common,
        0,
        0,
        new List<Event>(),
        new List<Event>()
    )
    {
    }
}