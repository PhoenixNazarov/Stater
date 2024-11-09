using System;

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
    float Y
);