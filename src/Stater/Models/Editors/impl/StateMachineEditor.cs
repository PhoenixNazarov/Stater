using System;
using System.Reactive.Subjects;

namespace Stater.Models.Editors.impl;

public class StateMachineEditor(
    IProjectManager projectManager
) : IStateMachineEditor
{
    private readonly ReplaySubject<StateMachine> _stateMachine = new();
    public IObservable<StateMachine> StateMachine => _stateMachine;

    public void DoSelect(StateMachine stateMachine)
    {
        _stateMachine.OnNext(stateMachine);
    }

    public void Update(StateMachine stateMachine)
    {
        projectManager.UpdateStateMachine(stateMachine);
    }
}