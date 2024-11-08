using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;

namespace Stater.Models.impl;

internal class ProjectManager : IProjectManager
{
    private readonly ReplaySubject<Project> _project = new();
    public IObservable<Project> Project => _project.AsObservable();

    private readonly SourceCache<StateMachine, string> _stateMachines = new(x => x.Name);
    public IObservable<IChangeSet<StateMachine, string>> StateMachines => _stateMachines.Connect();

    private readonly Subject<StateMachine> _stateMachine = new();
    public IObservable<StateMachine> StateMachine => _stateMachine;

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

    public void CreateStateMachine()
    {
        var stateMachine = new StateMachine(
            Name: "Test",
            States: new List<State>
            {
                new(X: 5, Y: 10, Size: 5, Color: "Green"),
                new(X: 5, Y: 10, Size: 50, Color: "Black"),
                new(X: 10, Y: 200, Size: 5, Color: "White"),
            }
        );
        _stateMachines.AddOrUpdate(stateMachine);
        _stateMachine.OnNext(stateMachine);
    }
}