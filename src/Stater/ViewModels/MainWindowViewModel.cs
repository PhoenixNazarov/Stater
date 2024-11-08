using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Avalonia.Input;
using DynamicData;
using ReactiveUI.SourceGenerators;
using Stater.Models;

namespace Stater.ViewModels;

using ReactiveUI;

public class MainWindowViewModel : ReactiveObject
{
    public MainWindowViewModel(IProjectManager projectManager)
    {
        _projectManager = projectManager;
        NewCommand = ReactiveCommand.Create(NewProject);
        OpenCommand = ReactiveCommand.Create(OpenProject);
        NewStateMachineCommand = ReactiveCommand.Create(NewStateMachine);

        projectManager
            .StateMachines
            .Bind(out _stateMachines)
            .Subscribe();

        projectManager
            .Project
            .Subscribe(x => Project = x);

        projectManager
            .StateMachine
            .Subscribe(x => StateMachine = x);
    }

    private readonly IProjectManager _projectManager;

    private Project _project;

    public Project Project
    {
        get => _project;
        private set => this.RaiseAndSetIfChanged(ref _project, value);
    }

    private StateMachine _stateMachine;

    public StateMachine StateMachine
    {
        get => _stateMachine;
        private set => this.RaiseAndSetIfChanged(ref _stateMachine, value);
    }

    private readonly ReadOnlyObservableCollection<StateMachine> _stateMachines;
    public ReadOnlyObservableCollection<StateMachine> StateMachines => _stateMachines;

    public ICommand NewCommand { get; }
    public ICommand OpenCommand { get; }
    public ICommand NewStateMachineCommand { get; }

    private double _scale = 1.0;

    public double Scale
    {
        get => _scale;
        set => this.RaiseAndSetIfChanged(ref _scale, value);
    }


    private void OpenProject()
    {
    }

    private void NewProject()
    {
        _projectManager.CreateProject("New Project");
    }

    private void NewStateMachine()
    {
        _projectManager.CreateStateMachine();
    }

    void Canvas_PointerWheelChanged(object sender, PointerWheelEventArgs e)
    {
        double zoomFactor = e.Delta.Y > 0 ? 1.1 : 0.9;
        Scale *= zoomFactor;
    }
}