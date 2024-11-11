using System;
using System.Collections.ObjectModel;
using System.Numerics;
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
            .Subscribe(x => StateMachine = x);

        NewCommand = ReactiveCommand.Create(NewProject);
        OpenCommand = ReactiveCommand.Create(OpenProject);
        NewStateMachineCommand = ReactiveCommand.Create(NewStateMachine);
        NewStateCommand = ReactiveCommand.Create(NewState);
        StateClickCommand = ReactiveCommand.Create<State>(OnStateClicked);
        UpdateStateCoordsCommand = ReactiveCommand.Create<Vector2>(UpdateStateCoords);
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

    [Reactive] public State? State { get; private set; }

    private readonly ReadOnlyObservableCollection<StateMachine> _stateMachines;
    public ReadOnlyObservableCollection<StateMachine> StateMachines => _stateMachines;

    public ICommand NewCommand { get; }
    public ICommand OpenCommand { get; }
    public ICommand NewStateMachineCommand { get; }
    public ICommand NewStateCommand { get; }
    public ReactiveCommand<State, Unit> StateClickCommand { get; }
    public ReactiveCommand<Vector2, Unit> UpdateStateCoordsCommand { get; }


    private void OpenProject()
    {
    }

    private void NewProject()
    {
        _projectManager.CreateProject("New Project");
    }

    private void NewStateMachine()
    {
        var stateMachine = _projectManager.CreateStateMachine();
        _editorManager.DoSelectStateMachine(stateMachine);
    }

    private void NewState()
    {
        var state = _projectManager.CreateState();
        if (state != null)
        {
            State = state;
        }
    }

    private void OnStateClicked(State state)
    {
        var selectedState = _projectManager.GetState(state.Guid);
        State = selectedState ?? null;
        if (State != null) _editorManager.DoSelectState(State);
    }

    private void UpdateStateCoords(Vector2 coords)
    {
        if (State == null) return;
        var newState = State with
        {
            X = State.X + coords.X,
            Y = State.Y + coords.Y
        };
        _projectManager.UpdateState(newState);
    }
}