using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Windows.Input;
using DynamicData;
using ReactiveUI.Fody.Helpers;
using Stater.Models;
using ReactiveUI;
using Stater.Models.Editors;

namespace Stater.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    public MainWindowViewModel(IProjectManager projectManager, IEditorManager editorManager)
    {
        _projectManager = projectManager;
        _editorManager = editorManager;

        projectManager
            .StateMachines
            .Bind(out _stateMachines)
            .Subscribe();

        projectManager
            .Project
            .Subscribe(x => Project = x);

        projectManager
            .StateMachine
            .Subscribe(x => { StateMachine = x; });

        NewCommand = ReactiveCommand.Create(NewProject);
        OpenCommand = ReactiveCommand.Create<StreamReader>(OpenProject);
        SaveCommand = ReactiveCommand.Create<StreamWriter>(SaveProject);
        NewStateMachineCommand = ReactiveCommand.Create(NewStateMachine);
        NewStateCommand = ReactiveCommand.Create(NewState);
        UndoCommand = ReactiveCommand.Create(Undo);
        RedoCommand = ReactiveCommand.Create(Redo);
        ReBuildGraphCommand = ReactiveCommand.Create(ReBuildGraph);
    }

    private readonly IProjectManager _projectManager;
    private readonly IEditorManager _editorManager;

    [Reactive] public Project Project { get; private set; }

    private StateMachine _stateMachine;

    public StateMachine StateMachine
    {
        get => _stateMachine;
        set
        {
            this.RaiseAndSetIfChanged(ref _stateMachine, value);
            var openStateMachine = _projectManager.OpenStateMachine(value.Guid);
            if (openStateMachine != null)
                _editorManager.DoSelectStateMachine(openStateMachine);
        }
    }


    private readonly ReadOnlyObservableCollection<StateMachine> _stateMachines;
    public ReadOnlyObservableCollection<StateMachine> StateMachines => _stateMachines;

    public ICommand NewCommand { get; }
    public ReactiveCommand<StreamReader, Unit> OpenCommand { get; }
    public ReactiveCommand<StreamWriter, Unit> SaveCommand { get; }
    public ICommand NewStateMachineCommand { get; }
    public ICommand NewStateCommand { get; }
    public ICommand UndoCommand { get; }
    public ICommand RedoCommand { get; }
    
    public ICommand ReBuildGraphCommand { get; }

    private void OpenProject(StreamReader sr)
    {
        _projectManager.LoadProject(sr);
    }

    private void NewProject()
    {
        _projectManager.CreateProject("New Project");
    }

    private void SaveProject(StreamWriter sw)
    {
        _projectManager.SaveProject(sw);
    }

    private void NewStateMachine()
    {
        var stateMachine = _projectManager.CreateStateMachine();
        _editorManager.DoSelectStateMachine(stateMachine);
    }

    private void NewState()
    {
        var state = _projectManager.CreateState();
        if (state != null) _editorManager.DoSelectState(state);
    }

    private void ReBuildGraph() => _projectManager.ReBuildGraph();

    private void Undo() => _projectManager.Undo();
    private void Redo() => _projectManager.Redo();
}