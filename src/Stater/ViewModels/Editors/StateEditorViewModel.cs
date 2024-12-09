using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using Avalonia;
using Avalonia.Platform;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stater.Models;
using Stater.Models.Editors;
using Stater.Utils;

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
                TypeIndex = x.Type switch
                {
                    StateType.Common => 0,
                    StateType.Start => 1,
                    StateType.End => 2,
                    _ => TypeIndex
                };
                Width = x.Width.ToString();
                Height = x.Height.ToString();
            });
        
        projectManager
            .StateMachine
            .Subscribe(x =>
            {
                AllStates = x.States;
                Transitions = x.Transitions.Select(y =>
                    {
                        var startState = x.States.Find(s => s.Guid == y.Start)!;
                        var endState = x.States.Find(s => s.Guid == y.End)!;
                        return DrawUtils.GetAssociateTransition(startState, endState, y);
                    }
                ).ToList();
            });

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
    [Reactive] public int TypeIndex { get; set; }
    
    [Reactive] public string Width { get; set; }
    [Reactive] public string Height { get; set; }

    [Reactive] public List<State> AllStates { get; set; }
    [Reactive] public List<DrawArrows> Transitions { get; set; }
    

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
        var type = TypeIndex switch
        {
            2 => StateType.End,
            1 => StateType.Start,
            _ => StateType.Common
        };
        var tryWidthParse = float.TryParse(Width, out float width);
        var tryHeightParse = float.TryParse(Height, out float height);
        if(!tryWidthParse || !tryHeightParse) return;
        var state = _projectManager.GetState(State.Guid);
        if (state == null) return;
        var newState = state with { Name = Name, Description = Description, Type = type , Width = width, Height = height, X = State.X, Y = State.Y};
        _stateEditor.Update(newState);
    }
    
}