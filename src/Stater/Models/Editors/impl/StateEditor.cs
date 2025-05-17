using System;
using System.Reactive.Subjects;
using Stater.Domain.Models;

namespace Stater.Models.Editors.impl;

public class StateEditor(IProjectManager projectManager): IStateEditor
{
    private readonly ReplaySubject<State> _state = new();
    public IObservable<State> State => _state;
    
    public void DoSelect(State state)
    {
        _state.OnNext(state);
    }

    public void Update(State state)
    {
        projectManager.UpdateState(state);
    }
}