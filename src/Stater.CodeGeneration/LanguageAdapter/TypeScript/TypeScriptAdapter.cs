using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.TypeScript;

public class TypeScriptAdapter : BaseLanguageAdapter
{
    protected override string TemplateName => "typescript";

    protected override string GetVariableValueTypeName(VariableValue value)
    {
        return value switch
        {
            VariableValue.IntVariable => "number",
            VariableValue.BoolVariable => "boolean",
            VariableValue.StringVariable => "string",
            VariableValue.FloatVariable => "number",
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
            VariableValue.FloatVariable variable => variable + ".0",
            _ => "unknown"
        };
    }
}