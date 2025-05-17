using System;
using System.Reactive.Subjects;
using Stater.Domain.Models;

namespace Stater.Models.Editors.impl;

public class TransitionEditor(
    IProjectManager projectManager
) : ITransitionEditor
{
    private readonly ReplaySubject<Transition> _transition = new();
    public IObservable<Transition> Transition => _transition;
    
    public void DoSelect(Transition transition)
    {
        _transition.OnNext(transition);
    }

    public void Update(Transition transition)
    {
        projectManager.UpdateTransition(transition);
    }
}