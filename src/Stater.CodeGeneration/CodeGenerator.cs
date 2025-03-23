using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Kotlin;
using Stater.Domain.Models;

namespace Stater.CodeGeneration;

public class CodeGenerator
{
    private readonly KotlinAdapter kotlinAdapter = new();

    public string Generate(StateMachine stateMachine, GenerationSettings generationSettings)
    {
        if (generationSettings is { GenerateEventAndCondition: true, GenerateContext: false })
        {
            throw new InvalidDataException();
        }
        
        switch (generationSettings.Language)
        {
            case Language.Kotlin: return kotlinAdapter.Generate(stateMachine, generationSettings);
        }

        return "";
    }
}