using System;

namespace Stater.Models.Editors;

public interface IStateEditor
{
    IObservable<State> State { get; }

    void DoSelect(State state);
    void Update(State state);
}