using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Animation;
using DynamicData;

namespace Stater.Models;

public interface IProjectManager
{
    IObservable<Project> Project { get; }
    IObservable<IChangeSet<StateMachine, string>> StateMachines { get; }
    IObservable<StateMachine> StateMachine { get; }

    void CreateProject(string name);
    Project? LoadProject(StreamReader sr);
    void SaveProject(StreamWriter sw);

    Project? GetProject();
    StateMachine? GetStateMachine();
    List<StateMachine> GetStateMachines();
    
    void Undo();
    void Redo();
    
    StateMachine CreateStateMachine();
    void RemoveStateMachine(Guid guid);
    void UpdateStateMachine(StateMachine newStateMachine);
    StateMachine? OpenStateMachine(Guid guid);
    
    State? CreateState();
    void RemoveState(Guid guid);
    State? GetState(Guid guid);
    void UpdateState(State state);

    Transition? CreateTransition(State start, State end);
    void RemoveTransition(Guid guid);
    Transition? GetTransition(Guid guid);
    void UpdateTransition(Transition transition);

    Variable? CreateVariable();
    void RemoveVariable(Guid guid);
    void UpdateVariable(Variable variable);

    void ChangeStateMachines(List<StateMachine> stateMachines);
}