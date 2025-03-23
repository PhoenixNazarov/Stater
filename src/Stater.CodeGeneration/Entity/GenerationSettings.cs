namespace Stater.CodeGeneration.Entity;

public record GenerationSettings(
    Language Language,
    Mode Mode = Mode.Clazz,
    bool GenerateStates = false,
    bool GenerateContext = false,
    bool GenerateInterface = false,
    bool GenerateEventAndCondition = false
);