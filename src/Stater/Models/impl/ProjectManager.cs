using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Xml;
using DynamicData;
using DynamicData.Alias;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Drawing;
using Point = Avalonia.Point;

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
    
    private readonly ReplaySubject<Graph> _graph = new();
    public IObservable<Graph> Graph => _graph;

    private readonly Stack<StateMachine> undoStack = new();
    private readonly Stack<StateMachine> redoStack = new();

    private StateMachine? GetCurrentStateMachine()
    {
        StateMachine? currentStateMachine = null;
        var s = _stateMachine.Subscribe(stateMachine => currentStateMachine = stateMachine);
        s.Dispose();
        return currentStateMachine;
    }
    
    private Project? GetCurrentProject()
    {
        Project? currentProject = null;
        var s = _project.Subscribe(x => currentProject = x);
        s.Dispose();
        return currentProject;
    }
    
    private List<StateMachine> GetCurrentStateMachines()
    {
        var s = _stateMachines.KeyValues.Select(x => x.Value).ToList();
        return s;
    }
    
    public void CreateProject(string name)
    {
        var project = new Project(
            Name: name,
            Location: null
        );
        _project.OnNext(project);
    }

    public Project? LoadProject(StreamReader sr)
    {
        XmlDocument doc = new XmlDocument();
        var writer = new System.Xml.Serialization.XmlSerializer(typeof(ExportProject));
        var project = writer.Deserialize(sr);
        if (project is not ExportProject project1) throw new ValidationException();
        if (project1.Project != null) _project.OnNext(project1.Project);
        project1.StateMachines?.ForEach(UpdateStateMachine);
        return project1?.Project;
    }

    public void SaveProject(StreamWriter sw)
    {
        var writer = new System.Xml.Serialization.XmlSerializer(typeof(ExportProject));
        var exportProject = new ExportProject(
            GetCurrentProject(),
            GetCurrentStateMachines()
            );
        writer.Serialize(sw, exportProject);
        sw.Close();
    }

    public void Undo()
    {
        Console.WriteLine("YES " + undoStack.Count);
        if (undoStack.Count <= 1) return;
        var stateMachine = undoStack.Pop();
        redoStack.Push(stateMachine);
        _stateMachine.OnNext(undoStack.Peek());
    }

    public void Redo()
    {
        if (redoStack.Count <= 0) return;
        var stateMachine = redoStack.Pop();
        undoStack.Push(stateMachine);
        _stateMachine.OnNext(stateMachine);
    }

    public void UpdateStateMachine(StateMachine newStateMachine)
    {
        undoStack.Push(newStateMachine);
        redoStack.Clear();
        _stateMachines.AddOrUpdate(newStateMachine);
        _stateMachine.OnNext(newStateMachine);
    }

    public StateMachine CreateStateMachine()
    {
        var stateMachine = new StateMachine(
            Guid: Guid.NewGuid(),
            Name: "StateMachine " + _stateMachines.Count,
            States: new List<State>(),
            Transitions: new List<Transition>(),
            Variables: new List<Variable>()
        );
        UpdateStateMachine(stateMachine);
        return stateMachine;
    }

    public StateMachine? OpenStateMachine(Guid guid)
    {
        var currentStateMachine = GetCurrentStateMachine();
        if (currentStateMachine?.Guid == guid) return null;
        var stateMachine = _stateMachines.KeyValues[guid.ToString()];
        undoStack.Clear();
        redoStack.Clear();
        _stateMachine.OnNext(stateMachine);
        undoStack.Push(stateMachine);
        return stateMachine;
    }

    public State? CreateState()
    {
        var currentStateMachine = GetCurrentStateMachine();
        if (currentStateMachine == null) return null;
        float x = 50;
        float y = 25;
        float width = 100;
        float height = 50;
        var state = new State(
            Guid: Guid.NewGuid(),
            Name: "State",
            Description: "",
            Type: StateType.Common,
            CenterPoint: new Point(x, y),
            Width: width,
            Height: height,
            new List<Event>(),
            new List<Event>()
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
            Name: "Transition",
            Start: start.Guid,
            End: end.Guid,
            Condition: null,
            LinePoints: [],
            Type: TypeArrow.Pifagor
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

    public Transition? GetTransition(Guid guid)
    {
        var currentStateMachine = GetCurrentStateMachine();
        return currentStateMachine?.Transitions.FirstOrDefault(e => e.Guid == guid);
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

    public void UpdateTransition(Transition transition)
    {
        var currentStateMachine = GetCurrentStateMachine();
        if (currentStateMachine == null) return;
        var transitions = currentStateMachine.Transitions.Where(el => el.Guid != transition.Guid);
        var newStateMachine = currentStateMachine with
        {
            Transitions = new List<Transition>(transitions) { transition }
        };
        UpdateStateMachine(newStateMachine);
    }

    public Variable? CreateVariable()
    {
        var currentStateMachine = GetCurrentStateMachine();
        if (currentStateMachine == null) return null;

        var variable = new Variable();
        currentStateMachine.Variables.Add(variable);

        UpdateStateMachine(currentStateMachine);
        return variable;
    }

    public void RemoveVariable(Guid guid)
    {
        var currentStateMachine = GetCurrentStateMachine();
        if (currentStateMachine == null) return;
        var variables = currentStateMachine.Variables.Where(el => el.Guid != guid);
        var newStateMachine = currentStateMachine with
        {
            Variables = variables.ToList()
        };
        UpdateStateMachine(newStateMachine);
    }

    public void UpdateVariable(Variable variable)
    {
        var currentStateMachine = GetCurrentStateMachine();
        if (currentStateMachine == null) return;
        var variables = currentStateMachine.Variables.Where(el => el.Guid != variable.Guid);
        var newStateMachine = currentStateMachine with
        {
            Variables = new List<Variable>(variables) { variable }
        };
        UpdateStateMachine(newStateMachine);
    }
}