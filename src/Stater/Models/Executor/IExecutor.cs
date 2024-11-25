using System;
using System.Collections.Generic;
using DynamicData;
using DynamicData.Binding;

namespace Stater.Models.Executor;

public interface IExecutor
{
    IObservable<State?> State { get; }
    IObservable<IChangeSet<ExecuteLog>> Logs { get; }
    IObservable<IDictionary<Guid, VariableValue>?> Variables { get; }

    void Start(int stepTime);
    void Stop();
    void Reset();
    void Step();
}