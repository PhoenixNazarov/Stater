using System.Reflection;
using Scriban;
using Scriban.Runtime;
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

    public static string RenderTemplate(
        string templateName,
        StateMachine stateMachine,
        GenerationSettings settings,
        Func<VariableValue, string> convertVariableType,
        Func<VariableValue, string> convertVariableValue
    )
    {
        var scriptObject = new ScriptObject();
        scriptObject.Import("convert_variable_type", new Func<VariableValue, string>(convertVariableType));
        scriptObject.Import("convert_variable_value", new Func<VariableValue, string>(convertVariableValue));
        scriptObject["fsm"] = stateMachine;
        scriptObject["settings"] = settings;
        
        var context = new TemplateContext();
        context.PushGlobal(scriptObject);
        
        var templateContent = LoadTemplate(templateName);
        var template = Template.Parse(templateContent);
        return template.Render(context);
    }
}