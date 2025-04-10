using Newtonsoft.Json;
using Stater.Domain.Models;

namespace Stater.CodeGeneration;

public class JsonTransition(string name, string start, string end)
{
    public string Name { get; } = name;
    public string Start { get; } = start;
    public string End { get; } = end;
}

public class JsonSchema
(
    List<string> states,
    string startState,
    List<JsonTransition> transitions
)
{
    public List<string> States { get; } = states;
    public string StartState { get; } = startState;
    public List<JsonTransition> Transitions { get; } = transitions;
}

public class JsonLoader
{
    public static StateMachine Load(string schema)
    {
        var schemaObject = JsonConvert.DeserializeObject<JsonSchema>(schema);

        var states = schemaObject.States.Select(x =>
        {
            var state = new State() with { Name = x };
            if (x == schemaObject.StartState)
            {
                state = state with { Type = StateType.Start };
            }

            return state;
        }).ToList();

        var stateMachine = new StateMachine { States = states };

        var transition = schemaObject.Transitions.Select(x => new Transition
        {
            Name = x.Name, Start = stateMachine.GetStateByName(x.Start)!.Guid,
            End = stateMachine.GetStateByName(x.End)!.Guid
        }).ToList();

        return stateMachine with { Transitions = transition };
    }
}