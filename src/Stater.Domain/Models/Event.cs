using System.Xml.Serialization;

namespace Stater.Domain.Models;

[XmlInclude(typeof(VariableMath))]
[Serializable]
public abstract record Event
{
    public record VariableMath(
        Guid VariableGuid,
        VariableMath.MathTypeEnum MathType,
        VariableValue Value
    ) : Event
    {
        public enum MathTypeEnum
        {
            Sum, // +
            Sub, // -
            Mul, // *
            Div, // /
        }

        public VariableMath() : this(Guid.Empty, MathTypeEnum.Sum, null)
        {
        }
    }

    public record VariableSet(
        Guid VariableGuid,
        VariableValue Value
    ) : Event;
}