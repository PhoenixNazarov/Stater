using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData;

namespace Stater.Models.impl;

public class ProjectManager : IProjectManager
{
    public Project CreateProject(string name)
    {
        return new Project(
            Name: name,
            Location: null,
            StateMachines: new ObservableCollection<StateMachine>()
        );
    }

    public Project LoadProject(string path)
    {
        throw new System.NotImplementedException();
    }

    public void SaveProject(Project project, string path)
    {
        throw new System.NotImplementedException();
    }
}