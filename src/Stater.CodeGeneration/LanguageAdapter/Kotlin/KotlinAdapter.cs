using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.Kotlin;

public class KotlinAdapter : BaseLanguageAdapter
{
    protected override string TemplateName => "kotlin";

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
}