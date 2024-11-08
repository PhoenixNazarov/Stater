using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Input;
using DynamicData;
using ReactiveUI.Fody.Helpers;
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

    [Reactive] public Project Project { get; private set; }
    [Reactive] public StateMachine StateMachine { get; private set; }

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
}