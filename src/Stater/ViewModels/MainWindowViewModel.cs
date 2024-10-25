using System.Collections.Generic;
using System.Windows.Input;
using Avalonia.Controls;
using Stater.Models;
using Stater.Models.impl;

namespace Stater.ViewModels;

using ReactiveUI;

public partial class MainWindowViewModel : ReactiveObject
{
    private readonly IProjectManager projectManager;
    public ICommand NewCommand { get; }
    public ICommand OpenCommand { get; }

    public ICommand NewStateMachineCommand { get; }

    private Project? project;

    public Project? Project
    {
        get => project;
        private set => this.RaiseAndSetIfChanged(ref project, value);
    }

    private StateMachine? _stateMachine;

    public StateMachine? StateMachine
    {
        get => _stateMachine;
        set => this.RaiseAndSetIfChanged(ref _stateMachine, value);
    }

    public MainWindowViewModel()
    {
        projectManager = new ProjectManager();
        NewCommand = ReactiveCommand.Create(NewProject);
        OpenCommand = ReactiveCommand.Create(OpenProject);
        NewStateMachineCommand = ReactiveCommand.Create(NewStateMachine);
    }

    private void OpenProject()
    {
    }

    private void NewProject()
    {
        Project = projectManager.CreateProject("New Project");
    }
    
    private void NewStateMachine()
    {
        Project?.StateMachines.Add(new StateMachine("Test", new List<State>()));
    }
}