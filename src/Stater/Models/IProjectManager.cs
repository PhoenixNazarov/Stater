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
    void CreateStateMachine();
}