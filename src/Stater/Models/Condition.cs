using System;

namespace Stater.Models;

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
    }
}