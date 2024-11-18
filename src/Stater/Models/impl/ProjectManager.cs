using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
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

    public void UpdateStateMachine(StateMachine newStateMachine)
    {
        _stateMachines.AddOrUpdate(newStateMachine);
        _stateMachine.OnNext(newStateMachine);
    }

    public StateMachine CreateStateMachine()
    {
        var stateMachine = new StateMachine(
            Guid: Guid.NewGuid(),
            Name: "StateMachine " + _stateMachines.Count,
            States: new List<State>(),
            Transitions: new List<Transition>()
        );
        UpdateStateMachine(stateMachine);
        return stateMachine;
    }

    public StateMachine? OpenStateMachine(Guid guid)
    {
        var currentStateMachine = GetCurrentStateMachine();
        if (currentStateMachine?.Guid == guid) return null;
        var stateMachine = _stateMachines.KeyValues[guid.ToString()];
        _stateMachine.OnNext(stateMachine);
        return stateMachine;
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

    public Transition? CreateTransition(State start, State end)
    {
        var currentStateMachine = GetCurrentStateMachine();
        if (currentStateMachine == null) return null;
        Transition transition = new(
            Guid: Guid.NewGuid(),
            Name: "State",
            Start: start.Guid,
            End: end.Guid
        );
        var newStateMachine = currentStateMachine with
        {
            Transitions = new List<Transition>(currentStateMachine.Transitions)
            {
                transition
            }
        };
        UpdateStateMachine(newStateMachine);
        return transition;
    }

    public void RemoveTransition(Guid guid)
    {
        var currentStateMachine = GetCurrentStateMachine();
        if (currentStateMachine == null) return;
        var transitions = currentStateMachine.Transitions.Where(el => el.Guid != guid);
        var newStateMachine = currentStateMachine with
        {
            Transitions = new List<Transition>(transitions)
        };
        UpdateStateMachine(newStateMachine);
    }
}