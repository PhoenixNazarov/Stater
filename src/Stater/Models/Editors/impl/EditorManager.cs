using System;
using System.Reactive.Subjects;

namespace Stater.Models.Editors.impl;

public class EditorManager : IEditorManager
{
    private readonly ReplaySubject<EditorTypeEnum> _editorType = new();
    public IObservable<EditorTypeEnum> EditorType => _editorType;

    private IStateMachineEditor _stateMachineEditor;
    private IStateEditor _stateEditor;
    private ITransitionEditor _transitionEditor;


    public EditorManager(IStateMachineEditor stateMachineEditor, IStateEditor stateEditor, ITransitionEditor transitionEditor)
    {
        _editorType.OnNext(EditorTypeEnum.Null);
        _stateMachineEditor = stateMachineEditor;
        _stateEditor = stateEditor;
        _transitionEditor = transitionEditor;
    }


    public void DoSelectStateMachine(StateMachine stateMachine)
    {
        _stateMachineEditor.DoSelect(stateMachine);
        _editorType.OnNext(EditorTypeEnum.StateMachine);
    }

    public void DoSelectState(State state)
    {
        _stateEditor.DoSelect(state);
        _editorType.OnNext(EditorTypeEnum.State);
    }

    public void DoSelectTransition(Transition transition)
    {
        _transitionEditor.DoSelect(transition);
        _editorType.OnNext(EditorTypeEnum.Transition);
    }
}