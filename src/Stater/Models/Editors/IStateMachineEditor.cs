using System;
using Stater.Domain.Models;

namespace Stater.Models.Editors;

public interface IStateMachineEditor
{
    IObservable<StateMachine> StateMachine { get; }

    void DoSelect(StateMachine stateMachine);
    void Update(StateMachine stateMachine);
}