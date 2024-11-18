using System;

namespace Stater.Models;

public record Transition(
    Guid Guid,
    string Name,
    Guid Start,
    Guid End
);