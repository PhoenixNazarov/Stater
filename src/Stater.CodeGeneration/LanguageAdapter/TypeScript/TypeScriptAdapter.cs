using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.TypeScript;

public class TypeScriptAdapter : ILanguageAdapter
{
    public string Generate(StateMachine stateMachine, GenerationSettings settings)
    {
        return TemplateLoader.RenderTemplate("typescript", stateMachine, settings);
    }
}