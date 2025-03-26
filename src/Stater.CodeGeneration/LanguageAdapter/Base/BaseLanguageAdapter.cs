using Stater.CodeGeneration.Entity;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.LanguageAdapter.Base;

public abstract class BaseLanguageAdapter
{
    protected abstract string TemplateName { get; }

    protected abstract string GetVariableValueTypeName(VariableValue value);
    protected abstract string GetVariableValue(VariableValue value);

    public string Generate(StateMachine stateMachine, GenerationSettings settings)
    {
        return TemplateLoader.RenderTemplate(
            TemplateName,
            stateMachine,
            settings,
            GetVariableValueTypeName,
            GetVariableValue
        );
    }
}