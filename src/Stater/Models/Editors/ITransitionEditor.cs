using System;
using Avalonia.Animation;

namespace Stater.Models.Editors;

public interface ITransitionEditor
{
    IObservable<Transition> Transition { get; }

    void DoSelect(Transition transition);
    void Update(Transition transition);
}