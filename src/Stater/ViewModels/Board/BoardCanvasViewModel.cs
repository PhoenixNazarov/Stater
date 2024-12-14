using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reactive;
using Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stater.Models;
using Stater.Models.Editors;

namespace Stater.ViewModels.Board;

public class BoardCanvasViewModel : ReactiveObject
{
    public BoardCanvasViewModel(IProjectManager projectManager, IEditorManager editorManager)
    {
        _projectManager = projectManager;
        _editorManager = editorManager;

        projectManager
            .StateMachine
            .Subscribe(x =>
            {
                StateMachine = x;
                Transitions = x.Transitions.Select(y =>
                        {
                            var startState = x.States.Find(s => s.Guid == y.Start);
                            var endState = x.States.Find(s => s.Guid == y.End);
                            if (startState != null && endState != null)
                                return new AssociateTransition(
                                    Transition: y,
                                    StartPoint: new Point(startState.X, startState.Y),
                                    EndPoint: new Point(endState.X, endState.Y),
                                    Start: startState,
                                    End: endState
                                );
                            return null;
                        }
                    )
                    .Where(y => y != null)
                    .OfType<AssociateTransition>()
                    .ToList();
            });

        StateClickCommand = ReactiveCommand.Create<State>(OnStateClicked);
        UpdateStateCoordsCommand = ReactiveCommand.Create<Vector2>(UpdateStateCoords);
        TransitionClickCommand = ReactiveCommand.Create<Transition>(OnTransitionClicked);
    }

    private readonly IProjectManager _projectManager;
    private readonly IEditorManager _editorManager;

    [Reactive] public List<AssociateTransition> Transitions { get; set; }
    [Reactive] public State? State { get; private set; }
    [Reactive] public StateMachine StateMachine { get; private set; }

    public ReactiveCommand<State, Unit> StateClickCommand { get; }
    public ReactiveCommand<Vector2, Unit> UpdateStateCoordsCommand { get; }

    public ReactiveCommand<Transition, Unit> TransitionClickCommand { get; }

    private void OnStateClicked(State state)
    {
        var selectedState = _projectManager.GetState(state.Guid);
        State = selectedState ?? null;
        if (State != null) _editorManager.DoSelectState(State);
    }

    private void OnTransitionClicked(Transition transition)
    {
        var selectedTransition = _projectManager.GetTransition(transition.Guid);
        if (selectedTransition != null) _editorManager.DoSelectTransition(selectedTransition);
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