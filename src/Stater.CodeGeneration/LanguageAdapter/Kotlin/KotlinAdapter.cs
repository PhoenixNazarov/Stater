using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.Kotlin;

public class KotlinAdapter : BaseLanguageAdapter
{
    protected override string TemplateName => "kotlin";
    protected override string TestTemplateName => "kotlin-test";
    
    protected override string GetVariableValueTypeName(VariableValue value)
    {
        return value switch
        {
            VariableValue.IntVariable => "Int",
            VariableValue.BoolVariable => "Boolean",
            VariableValue.StringVariable => "String",
            VariableValue.FloatVariable => "Float",
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
        throw new NotImplementedException();
    }

    protected override string GetEvent(Transition transition, Event eEvent, StateMachine stateMachine)
    {
        throw new NotImplementedException();
    }
}