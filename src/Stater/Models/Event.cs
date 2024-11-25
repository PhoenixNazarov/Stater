using System;

namespace Stater.Models;

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
    }
}