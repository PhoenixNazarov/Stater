﻿using System;
using System.Collections.Generic;

namespace Stater.Models;

public record StateMachine(
    Guid Guid,
    string Name,
    List<State> States,
    List<Transition> Transitions,
    List<Variable> Variables
)
{
    public StateMachine() : this(
        new Guid(),
        "",
        new List<State>(),
        new List<Transition>(),
        new List<Variable>()
    )
    {
    }

    public State? StartState
    {
        get { return States.Find(x => x.Type == StateType.Start); }
    }
}