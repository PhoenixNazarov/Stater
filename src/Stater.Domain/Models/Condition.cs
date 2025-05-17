using System.Xml.Serialization;

namespace Stater.Domain.Models;

[XmlInclude(typeof(VariableCondition))]
[Serializable]
public abstract record Condition
{
    public record VariableCondition(
        Guid VariableGuid,
        VariableCondition.ConditionTypeEnum ConditionType,
        VariableValue Value
    ) : Condition
    {
        public enum ConditionTypeEnum
        {
            Lt, // <
            Le, // <=
            Eq, // ==
            Ne, // !=
            Gt, // >
            Ge, // >=
        }

        public string GetDefaultConditionSign() => ConditionType switch
        {
            ConditionTypeEnum.Lt => "<",
            ConditionTypeEnum.Le => "<=",
            ConditionTypeEnum.Eq => "==",
            ConditionTypeEnum.Ne => "!=",
            ConditionTypeEnum.Gt => ">",
            ConditionTypeEnum.Ge => ">=",
            _ => throw new ArgumentOutOfRangeException()
        };

        public VariableCondition() : this(Guid.Empty, ConditionTypeEnum.Lt, null)
        {
        }
    }
}