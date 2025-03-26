using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.JavaScript;

public class JavaScriptAdapter : BaseLanguageAdapter
{
    protected override string TemplateName => "javascript";

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
            VariableValue.FloatVariable variable => variable + ".0",
            _ => "unknown"
        };
    }
}