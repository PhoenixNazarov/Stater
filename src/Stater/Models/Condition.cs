using System;

namespace Stater.Models;

public enum ConditionType
{
    LT, // <
    LE, // <=
    EQ, // ==
    NE, // !=
    GT, // >
    GE, // >=
}

public abstract record Condition
{
    public record VariableCondition(
        Guid VariableGuid,
        ConditionType ConditionType,
        Variable Variable
    ) : Condition;
}