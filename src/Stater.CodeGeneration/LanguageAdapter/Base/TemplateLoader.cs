using System.Reflection;
using System.Runtime.CompilerServices;
using Scriban;
using Scriban.Runtime;
using Stater.CodeGeneration.Entity;
using Stater.Domain.Json;
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
        scriptObject.Import("state_machine_json_schema", StateMachineJsonAdapter.ToJsonSchema);

        InitTestStateMachines(scriptObject, stateMachine);

        var context = new TemplateContext();
        context.PushGlobal(scriptObject);

        var templateContent = LoadTemplate(templateName);
        var template = Template.Parse(templateContent);
        return template.Render(context);
    }

    private static void InitTestStateMachines(ScriptObject scriptObject, StateMachine sm)
    {
        var sm1 = sm with { States = sm.States.Concat(new[] { new State { Name = "__test_state_1__" } }).ToList() };
        var sm2 = sm1 with { States = sm1.States.Concat(new[] { new State { Name = "__test_state_2__" } }).ToList() };
        var sm3 = sm2 with
        {
            Transitions = sm2.Transitions.Concat(new[]
            {
                new Transition
                {
                    Name = "__test_transition__",
                    Start = sm2.GetStateByName("__test_state_1__")!.Guid,
                    End = sm2.GetStateByName("__test_state_2__")!.Guid,
                }
            }).ToList()
        };
        scriptObject["fsm_test_state_1"] = sm1;
        scriptObject["fsm_test_state_2"] = sm2;
        scriptObject["fsm_test_state_3"] = sm3;
    }
}