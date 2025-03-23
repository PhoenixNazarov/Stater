using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Windows.Input;
using DynamicData;
using ReactiveUI.Fody.Helpers;
using Stater.Models;
using ReactiveUI;
using Stater.Domain.Models;
using Stater.Models.Editors;
using Stater.Plugin;

namespace Stater.ViewModels;

public record PathPluginDto(
    ButtonFilePlugin Plugin, string Path
);

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
        PluginButtinCommand = ReactiveCommand.Create<PathPluginDto>(StartButtonFilePlugin);
    }

    private readonly IProjectManager _projectManager;
    private readonly IEditorManager _editorManager;

    [Reactive] public Project Project { get; private set; }

    public List<IPlugin> Plugins =>
        new()
        {
            new SLXPlugin()
        };

    private StateMachine _stateMachine;

    public StateMachine? StateMachine
    {
        get => _stateMachine;
        set
        {
            if (value == null) return;
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
    public ReactiveCommand<PathPluginDto, Unit> PluginButtinCommand { get; }
    public ICommand NewStateMachineCommand { get; }
    public ICommand NewStateCommand { get; }
    public ICommand UndoCommand { get; }
    public ICommand RedoCommand { get; }


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

    private void Undo() => _projectManager.Undo();
    private void Redo() => _projectManager.Redo();

    private void StartButtonFilePlugin(PathPluginDto pathPluginDto)
    {
        var input = new PluginInput(
            Project: _projectManager.GetProject(),
            StateMachine: _projectManager.GetStateMachine(),
            StateMachines: _projectManager.GetStateMachines(),
            ProjectManager: _projectManager
        );
        var res = pathPluginDto.Plugin.Start(input, pathPluginDto.Path);

        _projectManager.ChangeStateMachines(res.ChangedStateMachines);
    }
}