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
        Func<VariableValue, string> convertVariableValue,
        Func<Transition, Condition, StateMachine, string> convertCondition,
        Func<Transition, Event, StateMachine, string> convertEvent, 
        List<List<Transition>> scenarios
    )
    {
        var scriptObject = new ScriptObject();
        scriptObject.Import("convert_variable_type", new Func<VariableValue, string>(convertVariableType));
        scriptObject.Import("convert_variable_value", new Func<VariableValue, string>(convertVariableValue));

        scriptObject.Import("convert_condition",
            new Func<Transition, Condition, string>((transition, condition) =>
                convertCondition(transition, condition, stateMachine)));
        scriptObject.Import("convert_event", new Func<Transition, Event, string>((transition, eEvent) =>
            convertEvent(transition, eEvent, stateMachine)));
        scriptObject["fsm"] = stateMachine;
        scriptObject["settings"] = settings;
        scriptObject["scenarios"] = scenarios;
        scriptObject.Import("get_variable_by_uuid", stateMachine.GetVariableByGuid);
        scriptObject.Import("get_state_by_uuid", stateMachine.GetStateByGuid);

        var context = new TemplateContext();
        context.PushGlobal(scriptObject);

        var templateContent = LoadTemplate(templateName);
        var template = Template.Parse(templateContent);
        return template.Render(context);
    }
}