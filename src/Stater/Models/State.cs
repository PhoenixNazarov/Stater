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
    float Width,
    float Height,
    List<Event> EntryEvents,
    List<Event> ExitEvents
)
{
    public float Left => X - Width / 2;
    public float Right => X + Width / 2;
    public float Top => Y + Height / 2;
    public float Bottom => Y - Height / 2;
    public State() : this(
        Guid.NewGuid(),
        "State",
        "",
        StateType.Common,
        50,
        25,
        100,
        50,
        new List<Event>(),
        new List<Event>()
    )
    {
    }
}