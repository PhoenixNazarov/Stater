using System;
using System.Collections.Generic;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stater.Models;
using Stater.Models.Editors;

namespace Stater.ViewModels.Editors;

public class StateEditorViewModel : ReactiveObject
{
    public StateEditorViewModel(IStateEditor stateEditor, IProjectManager projectManager)
    {
        _stateEditor = stateEditor;
        _projectManager = projectManager;

        SaveCommand = ReactiveCommand.Create(Save);

        _stateEditor
            .State
            .Subscribe(x =>
            {
                State = x;
                Name = x.Name;
                Description = x.Description;
                Type = x.Type.ToString();
            });

        projectManager
            .StateMachine
            .Subscribe(x =>
                {
                    AllStates = x.States;
                    Transitions =
                        x.Transitions.FindAll(y => State != null && (y.Start == State.Guid || y.End == State.Guid));
                }
            );

        AddTransitionCommand = ReactiveCommand.Create<State>(AddTransition);
        RemoveTransitionCommand = ReactiveCommand.Create<Transition>(RemoveTransition);
    }

    private readonly IStateEditor _stateEditor;
    private readonly IProjectManager _projectManager;

    public ICommand SaveCommand { get; }

    public ReactiveCommand<State, Unit> AddTransitionCommand { get; }
    public ReactiveCommand<Transition, Unit> RemoveTransitionCommand { get; }

    [Reactive] private State? State { get; set; }

    [Reactive] public string Name { get; set; }
    [Reactive] public string Description { get; set; }
    [Reactive] public string Type { get; set; }

    [Reactive] public List<State> AllStates { get; set; }
    [Reactive] public List<Transition> Transitions { get; set; }
    

    private void AddTransition(State state)
    {
        if (State == null) return;
        _projectManager.CreateTransition(State, state);
    }

    private void RemoveTransition(Transition transition)
    {
        _projectManager.RemoveTransition(transition.Guid);
    }

    private void Save()
    {
        if (State == null) return;
        var tryParse = Enum.TryParse(Type, out StateType type);
        if (!tryParse) return;
        var newState = State with { Name = Name, Description = Description, Type = type };
        _stateEditor.Update(newState);
    }
}