namespace Stater.CodeGeneration.Entity;

public record GenerationSettings(
    Language Language,
    Mode Mode,
    bool GenerateStates,
    bool GenerateContext,
    bool GenerateInterface
);