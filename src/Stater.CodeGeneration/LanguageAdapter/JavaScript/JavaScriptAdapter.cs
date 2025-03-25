using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.JavaScript;

public class JavaScriptAdapter : ILanguageAdapter
{
    public string Generate(StateMachine stateMachine, GenerationSettings settings)
    {
        return TemplateLoader.RenderTemplate("javascript", stateMachine, settings);
    }
}