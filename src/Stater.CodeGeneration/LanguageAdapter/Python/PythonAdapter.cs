using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.Python;

public class PythonAdapter : BaseLanguageAdapter
{
    protected override string TemplateName => "python";
    protected override string TestTemplateName => "python-test";

    protected override string GetVariableValueTypeName(VariableValue value)
    {
        return value switch
        {
            VariableValue.IntVariable => "int",
            VariableValue.BoolVariable => "bool",
            VariableValue.StringVariable => "str",
            VariableValue.FloatVariable => "float",
            _ => ""
        };
    }

    protected override string GetVariableValue(VariableValue value)
    {
        return value switch
        {
            VariableValue.IntVariable variable => variable.ToString(),
            VariableValue.BoolVariable variable => variable.Value ? "False" : "True",
            VariableValue.StringVariable variable => '"' + variable.Value + '"',
            VariableValue.FloatVariable variable => variable.ToString(),
            _ => ""
        };
    }

    protected override string GetCondition(Transition transition, Condition condition, StateMachine stateMachine)
    {
        switch (condition)
        {
            case Condition.VariableCondition e:
                var variable = stateMachine.GetVariableByGuid(e.VariableGuid);
                return $"ctx.{variable!.Name} {e.GetDefaultConditionSign()} {GetVariableValue(variable.StartValue)}";
        }

        return "";
    }

    protected override string GetEvent(Transition transition, Event eEvent, StateMachine stateMachine)
    {
        switch (eEvent)
        {
            case Event.VariableMath e:
                var variable1 = stateMachine.GetVariableByGuid(e.VariableGuid);
                return
                    $"setattr(ctx, '{variable1!.Name}', ctx.{variable1.Name} {e.GetDefaultMathTypeSign()} {GetVariableValue(e.Value)})";
            case Event.VariableSet e:
                var variable2 = stateMachine.GetVariableByGuid(e.VariableGuid);
                return $"setattr(ctx, '{variable2!.Name}', {GetVariableValue(e.Value)})";
        }

        return "";
    }
}