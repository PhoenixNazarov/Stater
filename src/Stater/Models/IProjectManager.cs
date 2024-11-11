using System;
using DynamicData;

namespace Stater.Models;

public interface IProjectManager
{
    IObservable<Project> Project { get; }
    IObservable<IChangeSet<StateMachine, string>> StateMachines { get; }
    IObservable<StateMachine> StateMachine { get; }

    void CreateProject(string name);
    Project LoadProject(string path);
    void SaveProject(Project project, string path);
    
    StateMachine CreateStateMachine();
    void UpdateStateMachine(StateMachine newStateMachine);
    StateMachine? OpenStateMachine(Guid guid);
    State? CreateState();
    State? GetState(Guid guid);
    void UpdateState(State state);
}