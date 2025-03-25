using System.Reflection;
using Scriban;
using Stater.CodeGeneration.Entity;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.Base;

public static class TemplateLoader
{
    public static string LoadTemplate(string templateName)
    {
        var resourceName = $"Stater.CodeGeneration.Templates.{templateName}.scriban";

        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        if (stream == null)
            throw new FileNotFoundException($"Template {templateName} not found");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static string RenderTemplate(string templateName, StateMachine stateMachine, GenerationSettings settings)
    {
        var templateContent = LoadTemplate(templateName);
        var template = Template.Parse(templateContent);
        return template.Render(new
        {
            fsm = stateMachine, settings
        });
    }
}