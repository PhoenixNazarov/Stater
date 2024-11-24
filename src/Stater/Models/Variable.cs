using System;

namespace Stater.Models;

public abstract record Variable(
    Guid Guid,
    string Name
)
{
    public record IntVariable(
        Guid Guid,
        string Name,
        int Value
    ) : Variable(Guid, Name);

    public record StringVariable(
        Guid Guid,
        string Name,
        string Value
    ) : Variable(Guid, Name);

    public record BoolVariable(
        Guid Guid,
        string Name,
        bool Value
    ) : Variable(Guid, Name);
}