using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using Avalonia.Animation;
using DynamicData;

namespace Stater.Models.impl;

internal class ProjectManager : IProjectManager
{
    private readonly ReplaySubject<Project> _project = new();
    public IObservable<Project> Project => _project;

    private readonly SourceCache<StateMachine, string> _stateMachines = new(x => x.Guid.ToString());
    public IObservable<IChangeSet<StateMachine, string>> StateMachines => _stateMachines.Connect();

    private readonly ReplaySubject<StateMachine> _stateMachine = new();
    public IObservable<StateMachine> StateMachine => _stateMachine;

    private readonly ReplaySubject<State> _state = new();
    public IObservable<State> State => _state;

    private readonly ReplaySubject<Transition> _transtion = new();
    public IObservable<Transition> Transition => _transtion;

    private StateMachine? GetCurrentStateMachine()
    {
        StateMachine? currentStateMachine = null;
        _stateMachine.Subscribe(stateMachine => currentStateMachine = stateMachine);
        return currentStateMachine;
    }

    public void CreateProject(string name)
    {
        var project = new Project(
            Name: name,
            Location: null
        );
        _project.OnNext(project);
    }

    public Project LoadProject(string path)
    {
        throw new NotImplementedException();
    }

    public void SaveProject(Project project, string path)
    {
        throw new NotImplementedException();
    }

    private void UpdateStateMachine(StateMachine newStateMachine)
    {
        _stateMachines.AddOrUpdate(newStateMachine);
        _stateMachine.OnNext(newStateMachine);
    }

    public void CreateStateMachine()
    {
        var stateMachine = new StateMachine(
            Guid: Guid.NewGuid(),
            Name: "StateMachine " + _stateMachines.Count,
            States: new List<State>()
        );
        UpdateStateMachine(stateMachine);
    }

    public void OpenStateMachine(Guid guid)
    {
        var currentStateMachine = GetCurrentStateMachine();
        if (currentStateMachine?.Guid == guid) return;
        var stateMachine = _stateMachines.KeyValues[guid.ToString()];
        _stateMachine.OnNext(stateMachine);
    }

    public State? CreateState()
    {
        var currentStateMachine = GetCurrentStateMachine();
        if (currentStateMachine == null) return null;
        var state = new State(
            Guid: Guid.NewGuid(),
            Name: "State",
            Description: "",
            Type: StateType.Common,
            10,
            10
        );
        var newStateMachine = currentStateMachine with
        {
            States = new List<State>(currentStateMachine.States) { state }
        };
        UpdateStateMachine(newStateMachine);
        return state;
    }

    public State? GetState(Guid guid)
    {
        var currentStateMachine = GetCurrentStateMachine();
        return currentStateMachine?.States.FirstOrDefault(e => e.Guid == guid);
    }

    public void UpdateState(State state)
    {
        var currentStateMachine = GetCurrentStateMachine();
        if (currentStateMachine == null) return;
        var states = currentStateMachine.States.Where(el => el.Guid != state.Guid);
        var newStateMachine = currentStateMachine with
        {
            States = new List<State>(states) { state }
        };
        UpdateStateMachine(newStateMachine);
    }
}