using System;
using Stater.Domain.Models;

namespace Stater.Models.Editors;

public interface IStateEditor
{
    IObservable<State> State { get; }

    void DoSelect(State state);
    void Update(State state);
}