using Avalonia;
using Stater.Domain.Models;

namespace Stater.Models;

public record AssociateTransition(
    State? Start,
    Point StartPoint,
    State? End,
    Point EndPoint,
    Transition Transition
);


public record StateTransition(
    State StartState,
    State EndState,
    Transition Transition
);