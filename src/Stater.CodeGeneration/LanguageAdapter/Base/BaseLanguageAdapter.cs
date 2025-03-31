using Stater.CodeGeneration.Entity;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.Base;

public abstract class BaseLanguageAdapter
{
    protected abstract string TemplateName { get; }
    protected abstract string TestTemplateName { get; }

    protected abstract string GetVariableValueTypeName(VariableValue value);
    protected abstract string GetVariableValue(VariableValue value);

    protected abstract string GetCondition(Transition transition, Condition condition, StateMachine stateMachine);
    protected abstract string GetEvent(Transition transition, Event eEvent, StateMachine stateMachine);

    public string Generate(StateMachine stateMachine, GenerationSettings settings)
    {
        return TemplateLoader.RenderTemplate(
            TemplateName,
            stateMachine,
            settings,
            GetVariableValueTypeName,
            GetVariableValue,
            GetCondition,
            GetEvent,
            new List<List<Transition>>()
        );
    }

    public string GenerateTests(StateMachine stateMachine, GenerationSettings settings,
        List<List<Transition>> scenarios)
    {
        return TemplateLoader.RenderTemplate(
            TestTemplateName,
            stateMachine,
            settings,
            GetVariableValueTypeName,
            GetVariableValue,
            GetCondition,
            GetEvent,
            scenarios
        );
    }

    protected bool CheckDefaultMathEvent(
        VariableValue variableValue1,
        VariableValue variableValue2,
        Event.VariableMath.MathTypeEnum mathTypeEnum
    )
    {
        return (variableValue1, variableValue2, mathTypeEnum) switch
        {
            (VariableValue.IntVariable, VariableValue.IntVariable, _) => true,
            (VariableValue.IntVariable, VariableValue.FloatVariable, _) => true,
            (VariableValue.FloatVariable, VariableValue.IntVariable, _) => true,
            (VariableValue.FloatVariable, VariableValue.FloatVariable, _) => true,

            (VariableValue.StringVariable, VariableValue.StringVariable, Event.VariableMath.MathTypeEnum.Sum) => true,

            _ => false
        };
    }
}