using System;
using System.Collections.Generic;

namespace Stater.Models;

public record StateMachine(
    Guid Guid,
    string Name,
    List<State> States
);