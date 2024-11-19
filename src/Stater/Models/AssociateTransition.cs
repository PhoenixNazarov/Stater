using Avalonia;

namespace Stater.Models;

public record AssociateTransition(
    State? Start,
    Point StartPoint,
    State? End,
    Point EndPoint,
    Transition Transition
);