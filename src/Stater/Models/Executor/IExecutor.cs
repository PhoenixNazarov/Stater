using System;
using System.Collections.Generic;
using Stater.Domain.Models;

namespace Stater.Models.Executor;

public record LogContainer(
    List<ExecuteLog> Logs
);

public interface IExecutor
{
    IObservable<State?> State { get; }
    IObservable<LogContainer> Logs { get; }
    IObservable<IDictionary<Guid, VariableValue>?> Variables { get; }

    void Start(int stepTime);
    void Stop();
    void Reset();
    void Step();

    void ClearLog();
}