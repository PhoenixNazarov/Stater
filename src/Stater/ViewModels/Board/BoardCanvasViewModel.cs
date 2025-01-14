using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reactive;
using Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stater.Models;
using Stater.Models.Draw;
using Stater.Models.Editors;
using Stater.Utils;

namespace Stater.ViewModels.Board;

public class BoardCanvasViewModel : ReactiveObject
{
    public BoardCanvasViewModel(IProjectManager projectManager, IEditorManager editorManager)
    {
        _projectManager = projectManager;
        _editorManager = editorManager;
        
        Height = 400;
        Width = 800;
        
        projectManager
            .StateMachine
            .Subscribe(x =>
            {
                StateMachine = isAnalyze ? AnalyzeGraph.Analyze(x)! : x;
                StartPointsFigure = StateMachine.States
                    .Where(y => y.Type == StateType.Start)
                    .Select(y => new CircleFigure(
                        new Point(y.Left + pointRadius * 1.2, y.Bottom + pointRadius * 1.2), 2 * pointRadius, "black"))
                    .ToList();
                EndPointsFigure = StateMachine.States
                    .Where(y => y.Type == StateType.End)
                    .Select(y => new SquareFigure(
                        new Point(y.Left + pointRadius * 1.2, y.Bottom + pointRadius * 1.2),2 * pointRadius, "black"))
                    .ToList();
                Transitions = StateMachine.Transitions.Select(y =>
                        {
                            var startState = x.States.Find(s => s.Guid == y.Start)!;
                            var endState = x.States.Find(s => s.Guid == y.End)!;
                            return DrawUtils.GetTransition(startState, endState, y, isAnalyze);
                        }
                    ).Where(y => y != null)
                    .OfType<DrawArrows>()
                    .ToList();
            });
        _projectManager
            .IsAnalyze
            .Subscribe(x => isAnalyze = x);

        StateClickCommand = ReactiveCommand.Create<State>(OnStateClicked);
        UpdateStateCoordsCommand = ReactiveCommand.Create<Vector2>(UpdateStateCoords);
        TransitionClickCommand = ReactiveCommand.Create<Transition>(OnTransitionClicked);
    }

    private readonly IProjectManager _projectManager;
    private readonly IEditorManager _editorManager;

    [Reactive] public List<DrawArrows> Transitions { get; set; }

    private bool isAnalyze = false;
    [Reactive] public int Width { get; set; }
    
    [Reactive] public int Height { get; set; }
    [Reactive] public double pointRadius { get; set; } = 10;
    [Reactive] public List<CircleFigure> StartPointsFigure { get; set; }
    [Reactive] public List<SquareFigure> EndPointsFigure { get; set; }

    [Reactive] public Transition? Transition { get; set; }
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
        Transition = selectedTransition ?? null;
        if (selectedTransition != null) _editorManager.DoSelectTransition(selectedTransition);
    }

    private void UpdateStateCoords(Vector2 coords)
    {
        if (State == null) return;
        var newState = State with
        {
            CenterPoint = new Point(State.X + coords.X, State.Y + coords.Y),
        };
        var startUpdate = StateMachine.Transitions.FindAll(t => t.Start == newState.Guid);
        UpdateTransitionStart(coords, startUpdate);
        var endUpdate = StateMachine.Transitions.FindAll(t => t.End == newState.Guid);
        UpdateTransitionEnd(coords, endUpdate);
        _editorManager.DoSelectState(newState);
        _projectManager.UpdateState(newState);
    }

    private void UpdateTransitionStart(Vector2 coords, List<Transition> transitions)
    {
        foreach (var transition in transitions)
        {
            transition.LinePoints[0] += new Point(coords.X, coords.Y);
        }
    }

    private void UpdateTransitionEnd(Vector2 coords, List<Transition> transitions)
    {
        foreach (var transition in transitions)
        {
            transition.LinePoints[^1] += new Point(coords.X, coords.Y);
        }
    }
}