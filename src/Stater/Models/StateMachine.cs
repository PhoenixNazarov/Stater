using System.Collections.Generic;

namespace Stater.Models;

public record StateMachine(
    string Name,
    List<State> States
);