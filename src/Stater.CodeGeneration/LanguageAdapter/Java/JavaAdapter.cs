using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.Java;

public class JavaAdapter : BaseLanguageAdapter
{
    protected override string TemplateName => "java";
    protected override string TestTemplateName => "java-test";

    protected override string GetVariableValueTypeName(VariableValue value)
    {
        return value switch
        {
            VariableValue.IntVariable => "int",
            VariableValue.BoolVariable => "boolean",
            VariableValue.StringVariable => "String",
            VariableValue.FloatVariable => "float",
            _ => "unknown"
        };
    }

    protected override string GetVariableValue(VariableValue value)
    {
        return value switch
        {
            VariableValue.IntVariable variable => variable.ToString(),
            VariableValue.BoolVariable variable => variable.Value ? "false" : "true",
            VariableValue.StringVariable variable => '"' + variable.Value + '"',
            VariableValue.FloatVariable variable => variable.ToString() + 'f',
            _ => "unknown"
        };
    }

    protected override string GetCondition(Transition transition, Condition condition, StateMachine stateMachine)
    {
        switch (condition)
        {
            case Condition.VariableCondition e:
                var variable = stateMachine.GetVariableByGuid(e.VariableGuid);
                return !CheckDefaultMathCondition(variable!.StartValue, e.Value, e.ConditionType)
                    ? "true"
                    : $"ctx.{variable.Name} {e.GetDefaultConditionSign()} {GetVariableValue(variable.StartValue)}";
        }

        return "true";
    }

    protected override string GetEvent(Transition transition, Event eEvent, StateMachine stateMachine)
    {
        switch (eEvent)
        {
            case Event.VariableMath e:
                var variable1 = stateMachine.GetVariableByGuid(e.VariableGuid);
                return !CheckDefaultMathEvent(variable1!.StartValue, e.Value, e.MathType)
                    ? "{}"
                    : $"ctx.{variable1.Name} = ctx.{variable1.Name} {e.GetDefaultMathTypeSign()} {GetVariableValue(e.Value)}";

            case Event.VariableSet e:
                var variable2 = stateMachine.GetVariableByGuid(e.VariableGuid);
                return $"ctx.{variable2!.Name} = {GetVariableValue(e.Value)}";
        }

        return "{}";
    }
}