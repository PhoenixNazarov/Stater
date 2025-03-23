using System;
using Stater.Domain.Models;

namespace Stater.Models.Editors;

public enum EditorTypeEnum
{
    Null,
    StateMachine,
    State,
    Transition,
    Variable
}

public interface IEditorManager
{
    IObservable<EditorTypeEnum> EditorType { get; }

    void DoSelectNull();
    
    void DoSelectStateMachine(StateMachine stateMachine);
    void DoSelectState(State state);
    void DoSelectTransition(Transition transition);
    void DoSelectVariable(Variable variable);
}