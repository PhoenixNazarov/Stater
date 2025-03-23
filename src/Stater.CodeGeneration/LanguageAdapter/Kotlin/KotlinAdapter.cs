using Scriban;
using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.Kotlin;

public class KotlinAdapter : ILanguageAdapter
{
    public string Generate(StateMachine stateMachine, GenerationSettings settings)
    {
        return RenderTemplate("kotlin", stateMachine, settings);
    }

    private static string RenderTemplate(string templateName, StateMachine stateMachine, GenerationSettings settings)
    {
        var templateContent = TemplateLoader.LoadTemplate(templateName);
        var template = Template.Parse(templateContent);
        return template.Render(new
        {
            fsm = stateMachine, settings
        });
    }
}