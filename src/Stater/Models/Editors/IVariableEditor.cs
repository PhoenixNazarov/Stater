using System;
using Stater.Domain.Models;

namespace Stater.Models.Editors;

public interface IVariableEditor
{
    IObservable<Variable> Variable { get; }
    
    void DoSelect(Variable transition);
    void Update(Variable transition);
}