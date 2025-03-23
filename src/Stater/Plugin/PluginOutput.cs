using System.Collections.Generic;
using Stater.Domain.Models;

namespace Stater.Plugin;

public record PluginOutput(
    string? Message,
    List<StateMachine> ChangedStateMachines
)
{
    public static PluginOutput From(
        string message
    )
    {
        return new PluginOutput(message, new List<StateMachine>());
    }
}