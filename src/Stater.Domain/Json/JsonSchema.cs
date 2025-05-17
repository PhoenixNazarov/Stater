namespace Stater.Domain.Json;

public record JsonTransition(
    string Name,
    string Start,
    string End
);

public record JsonSchema
(
    List<string> States,
    string StartState,
    List<JsonTransition> Transitions
);