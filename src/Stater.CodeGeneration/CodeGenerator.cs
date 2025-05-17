using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.CPlusPlus;
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
    private readonly CPlusPlusAdapter cPlusPlusAdapter = new();

    public string Generate(StateMachine stateMachine, GenerationSettings generationSettings)
    {
        if (generationSettings is { GenerateEventAndCondition: true, GenerateContext: false })
        {
            throw new InvalidDataException();
        }

        return generationSettings.Language switch
        {
            Language.Kotlin => kotlinAdapter.Generate(stateMachine, generationSettings),
            Language.Java => javaAdapter.Generate(stateMachine, generationSettings),
            Language.CSharp => cSharpAdapter.Generate(stateMachine, generationSettings),
            Language.Python3 => pythonAdapter.Generate(stateMachine, generationSettings),
            Language.JavaScript => javaScriptAdapter.Generate(stateMachine, generationSettings),
            Language.TypeScript => typeScriptAdapter.Generate(stateMachine, generationSettings),
            Language.CPlusPlus => cPlusPlusAdapter.Generate(stateMachine, generationSettings),
            _ => throw new ArgumentOutOfRangeException(nameof(generationSettings))
        };
    }
    
    public string GenerateTests(StateMachine stateMachine, GenerationSettings generationSettings, List<List<Transition>> scenarios)
    {
        if (generationSettings is { GenerateEventAndCondition: true, GenerateContext: false })
        {
            throw new InvalidDataException();
        }

        return generationSettings.Language switch
        {
            Language.Kotlin => kotlinAdapter.GenerateTests(stateMachine, generationSettings, scenarios),
            Language.Java => javaAdapter.GenerateTests(stateMachine, generationSettings, scenarios),
            Language.CSharp => cSharpAdapter.GenerateTests(stateMachine, generationSettings, scenarios),
            Language.Python3 => pythonAdapter.GenerateTests(stateMachine, generationSettings, scenarios),
            Language.JavaScript => javaScriptAdapter.GenerateTests(stateMachine, generationSettings, scenarios),
            Language.TypeScript => typeScriptAdapter.GenerateTests(stateMachine, generationSettings, scenarios),
            Language.CPlusPlus => cPlusPlusAdapter.GenerateTests(stateMachine, generationSettings, scenarios),
            _ => throw new ArgumentOutOfRangeException(nameof(generationSettings))
        };
    }
}