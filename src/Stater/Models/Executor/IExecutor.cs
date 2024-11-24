using System;

namespace Stater.Models.Executor;

public interface IExecutor
{
    IObservable<State> State { get; }
    IObservable<ExecuteLog> Logs { get; }

    void Start(double stepTime);
    void Pause();
    void Reset();
    void Step();
}