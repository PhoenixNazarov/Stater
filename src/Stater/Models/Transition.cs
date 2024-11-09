using System;

namespace Stater.Models;

public record Transition(
    string Name,
    Guid Start,
    Guid End
);