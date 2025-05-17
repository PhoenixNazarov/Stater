using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.JavaScript;

public class JavaScriptAdapter : BaseLanguageAdapter
{
    protected override string TemplateName => "javascript";
    protected override string TestTemplateName => "javascript-test";

    protected override string GetVariableValueTypeName(VariableValue value)
    {
        throw new NotImplementedException();
    }

    protected override string GetVariableValue(VariableValue value)
    {
        return value switch
        {
            VariableValue.IntVariable variable => variable.ToString(),
            VariableValue.BoolVariable variable => variable.Value ? "false" : "true",
            VariableValue.StringVariable variable => '"' + variable.Value + '"',
            VariableValue.FloatVariable variable => variable.ToString(),
            _ => "unknown"
        };
    }

    protected override string GetCondition(Transition transition, Condition condition, StateMachine stateMachine)
    {
        switch (condition)
        {
            case Condition.VariableCondition e:
                var variable = stateMachine.GetVariableByGuid(e.VariableGuid);
                return
                    $"ctx.{variable!.Name} {e.GetDefaultConditionSign().Replace("==", "===")} {GetVariableValue(variable.StartValue)}";
        }

        return "";
    }

    protected override string GetEvent(Transition transition, Event eEvent, StateMachine stateMachine)
    {
        switch (eEvent)
        {
            case Event.VariableMath e:
                var variable1 = stateMachine.GetVariableByGuid(e.VariableGuid);
                return $"ctx.{variable1!.Name} = ctx.{variable1.Name} {e.GetDefaultMathTypeSign()} {GetVariableValue(e.Value)}";

            case Event.VariableSet e:
                var variable2 = stateMachine.GetVariableByGuid(e.VariableGuid);
                return $"ctx.{variable2!.Name} = {GetVariableValue(e.Value)}";
        }

        return "";
    }
}