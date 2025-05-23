using System;
using System.Reactive.Subjects;
using Stater.Domain.Models;

namespace Stater.Models.Editors.impl;

public class EditorManager : IEditorManager
{
    private readonly ReplaySubject<EditorTypeEnum> _editorType = new();
    public IObservable<EditorTypeEnum> EditorType => _editorType;

    private IStateMachineEditor _stateMachineEditor;
    private IStateEditor _stateEditor;
    private ITransitionEditor _transitionEditor;
    private IVariableEditor _variableEditor;

    public void DoSelectNull()
    {
        _editorType.OnNext(EditorTypeEnum.Null);
    }

    public EditorManager(
        IStateMachineEditor stateMachineEditor,
        IStateEditor stateEditor,
        ITransitionEditor transitionEditor,
        IVariableEditor variableEditor)
    {
        _editorType.OnNext(EditorTypeEnum.Null);
        _stateMachineEditor = stateMachineEditor;
        _stateEditor = stateEditor;
        _transitionEditor = transitionEditor;
        _variableEditor = variableEditor;
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

    public void DoSelectVariable(Variable variable)
    {
        _variableEditor.DoSelect(variable);
        _editorType.OnNext(EditorTypeEnum.Variable);
    }

    public void DoSelectCodeGeneration()
    {
        _editorType.OnNext(EditorTypeEnum.CodeGeneration);
    }
}