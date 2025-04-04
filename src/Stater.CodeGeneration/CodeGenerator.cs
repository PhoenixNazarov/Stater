using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.CSharp;
using Stater.CodeGeneration.LanguageAdapter.Java;
using Stater.CodeGeneration.LanguageAdapter.JavaScript;
using Stater.CodeGeneration.LanguageAdapter.Kotlin;
using Stater.CodeGeneration.LanguageAdapter.Python;
using Stater.CodeGeneration.LanguageAdapter.TypeScript;
using Stater.Domain.Models;

namespace Stater.CodeGeneration;

public class CodeGenerator
{
    private readonly KotlinAdapter kotlinAdapter = new();
    private readonly PythonAdapter pythonAdapter = new();
    private readonly JavaScriptAdapter javaScriptAdapter = new();
    private readonly TypeScriptAdapter typeScriptAdapter = new();
    private readonly JavaAdapter javaAdapter = new();
    private readonly CSharpAdapter cSharpAdapter = new();

    public string Generate(StateMachine stateMachine, GenerationSettings generationSettings)
    {
        if (generationSettings is { GenerateEventAndCondition: true, GenerateContext: false })
        {
            throw new InvalidDataException();
        }
        
        switch (generationSettings.Language)
        {
            case Language.Kotlin: return kotlinAdapter.Generate(stateMachine, generationSettings);
            case Language.Java: return javaAdapter.Generate(stateMachine, generationSettings);
            case Language.CSharp: return cSharpAdapter.Generate(stateMachine, generationSettings);
            case Language.Python3: return pythonAdapter.Generate(stateMachine, generationSettings);
            case Language.JavaScript:return javaScriptAdapter.Generate(stateMachine, generationSettings);
            case Language.TypeScript:return typeScriptAdapter.Generate(stateMachine, generationSettings);
            case Language.C:
                break;
            case Language.CPlusPlus:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(generationSettings));
        }

        return "";
    }
    
    public string GenerateTests(StateMachine stateMachine, GenerationSettings generationSettings, List<List<Transition>> scenarios)
    {
        if (generationSettings is { GenerateEventAndCondition: true, GenerateContext: false })
        {
            throw new InvalidDataException();
        }
        
        switch (generationSettings.Language)
        {
            case Language.Kotlin: return kotlinAdapter.GenerateTests(stateMachine, generationSettings, scenarios);
            case Language.Java: return javaAdapter.GenerateTests(stateMachine, generationSettings, scenarios);
            case Language.CSharp: return cSharpAdapter.GenerateTests(stateMachine, generationSettings, scenarios);
            case Language.Python3: return pythonAdapter.GenerateTests(stateMachine, generationSettings, scenarios);
            case Language.JavaScript:return javaScriptAdapter.GenerateTests(stateMachine, generationSettings, scenarios);
            case Language.TypeScript:return typeScriptAdapter.GenerateTests(stateMachine, generationSettings, scenarios);
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