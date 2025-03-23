using Scriban;
using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.Kotlin;

public class KotlinAdapter : ILanguageAdapter
{
    public string Generate(StateMachine stateMachine, GenerationSettings settings)
    {
        switch (settings.Mode)
        {
            case Mode.Claz when settings is { GenerateContext: true, GenerateStates: true }:
                return GenerateClassStateContext(stateMachine, settings.GenerateInterface);
            case Mode.Claz when settings.GenerateStates:
                return GenerateClassState(stateMachine, settings.GenerateInterface);
            case Mode.Claz when settings.GenerateContext:
                return GenerateClassState(stateMachine, settings.GenerateInterface);
            case Mode.Claz:
                return GenerateClass(stateMachine, settings.GenerateInterface);
            case Mode.Builder:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(settings));
        }

        return "";
    }

    private static string GenerateClass(StateMachine stateMachine, bool generateInterface)
    {
        var templateContent = TemplateLoader.LoadTemplate("Kotlin.class");
        var template = Template.Parse(templateContent);
        return template.Render(new
        {
            fsm = stateMachine, generateInterface
        });
    }

    private static string GenerateClassContext(StateMachine stateMachine, bool generateInterface)
    {
        var templateContent = TemplateLoader.LoadTemplate("Kotlin.classContext");
        var template = Template.Parse(templateContent);
        return template.Render(new
        {
            fsm = stateMachine, generateInterface
        });
    }

    private static string GenerateClassState(StateMachine stateMachine, bool generateInterface)
    {
        var templateContent = TemplateLoader.LoadTemplate("Kotlin.classState");
        var template = Template.Parse(templateContent);
        return template.Render(new
        {
            fsm = stateMachine, generateInterface
        });
    }

    private static string GenerateClassStateContext(StateMachine stateMachine, bool generateInterface)
    {
        var templateContent = TemplateLoader.LoadTemplate("Kotlin.classStateContext");
        var template = Template.Parse(templateContent);
        return template.Render(new
        {
            fsm = stateMachine, generateInterface
        });
    }
}