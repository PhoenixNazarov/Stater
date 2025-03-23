namespace Stater.Domain.Models;

public record StateTransition(
    State StartState,
    State EndState,
    Transition Transition
);