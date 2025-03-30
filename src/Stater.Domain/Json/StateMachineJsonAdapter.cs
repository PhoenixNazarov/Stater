using System.Text.Json;
using Stater.Domain.Models;

namespace Stater.Domain.Json;

public static class StateMachineJsonAdapter
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public static string ToJsonSchema(StateMachine stateMachine)
    {
        var jsonSchema = new JsonSchema(
            States: stateMachine.States.Select(el => el.Name).Distinct().Order().ToList(),
            StartState: stateMachine.StartState!.Name,
            Transitions: stateMachine.StateTransitions.Select(
                el => new JsonTransition(Name: el.Transition.Name, Start: el.StartState.Name, End: el.EndState.Name)
            ).ToList()
        );
        return JsonSerializer.Serialize(jsonSchema, Options);
    }

    public static StateMachine FromJsonSchema(string json)
    {
        var jsonSchema = JsonSerializer.Deserialize<JsonSchema>(json, Options);

        if (jsonSchema == null)
        {
            throw new ArgumentException(null, nameof(json));
        }

        var sm = new StateMachine
        {
            States = jsonSchema.States.Select(el => new State
            {
                Name = el,
                Type = el == jsonSchema.StartState ? StateType.Start : StateType.Common
            }).ToList()
        };
        sm = sm with
        {
            Transitions = jsonSchema.Transitions.Select(el => new Transition
                {
                    Name = el.Name,
                    Start = sm.GetStateByName(el.Start)!.Guid,
                    End = sm.GetStateByName(el.End)!.Guid,
                })
                .ToList()
        };

        return sm;
    }
}