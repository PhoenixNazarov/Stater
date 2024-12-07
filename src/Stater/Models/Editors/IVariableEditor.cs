using System;

namespace Stater.Models.Editors;

public interface IVariableEditor
{
    IObservable<Variable> Variable { get; }
    
    void DoSelect(Variable transition);
    void Update(Variable transition);
}