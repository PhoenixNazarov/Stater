using System;

namespace Stater.Models.Editors;

public enum EditorTypeEnum
{
    Null,
    StateMachine,
    State,
    Transition
}

public interface IEditorManager
{
    IObservable<EditorTypeEnum> EditorType { get; }

    void DoSelectStateMachine(StateMachine stateMachine);
    void DoSelectState(State state);
    void DoSelectTransition(Transition transition);
}