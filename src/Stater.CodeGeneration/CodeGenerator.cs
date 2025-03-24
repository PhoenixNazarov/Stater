using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Kotlin;
using Stater.CodeGeneration.LanguageAdapter.Python;
using Stater.Domain.Models;

namespace Stater.CodeGeneration;

public class CodeGenerator
{
    private readonly KotlinAdapter kotlinAdapter = new();
    private readonly PythonAdapter pythonAdapter = new();

    public string Generate(StateMachine stateMachine, GenerationSettings generationSettings)
    {
        if (generationSettings is { GenerateEventAndCondition: true, GenerateContext: false })
        {
            throw new InvalidDataException();
        }
        
        switch (generationSettings.Language)
        {
            case Language.Kotlin: return kotlinAdapter.Generate(stateMachine, generationSettings);
            case Language.Java:
                break;
            case Language.CSharp:
                break;
            case Language.Python3: return pythonAdapter.Generate(stateMachine, generationSettings);
            case Language.JavaScript:
                break;
            case Language.TypeScript:
                break;
            case Language.C:
                break;
            case Language.CPlusPlus:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(generationSettings));
        }

        return "";
    }
}