using Stater.CodeGeneration.LanguageAdapter.Base;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.Python;

public class PythonAdapter : BaseLanguageAdapter
{
    protected override string TemplateName => "python";

    protected override string GetVariableValueTypeName(VariableValue value)
    {
        return value switch
        {
            VariableValue.IntVariable => "int",
            VariableValue.BoolVariable => "bool",
            VariableValue.StringVariable => "str",
            VariableValue.FloatVariable => "float",
            _ => "unknown"
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
            _ => "unknown"
        };
    }
}