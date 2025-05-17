using System;
using Stater.Domain.Models;

namespace Stater.Models.Editors;

public interface ITransitionEditor
{
    IObservable<Transition> Transition { get; }

    void DoSelect(Transition transition);
    void Update(Transition transition);
}