using System;
using System.Reactive.Subjects;
using Stater.Domain.Models;

namespace Stater.Models.Editors.impl;

public class VariableEditor(
    IProjectManager projectManager
) : IVariableEditor
{
    private readonly ReplaySubject<Variable> _variable = new();
    public IObservable<Variable> Variable => _variable;
    
    public void DoSelect(Variable variable)
    {
        _variable.OnNext(variable);
    }

    public void Update(Variable variable)
    {
        projectManager.UpdateVariable(variable);
    }
}