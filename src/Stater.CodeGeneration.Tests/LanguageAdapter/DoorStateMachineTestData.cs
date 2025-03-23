using System.Collections;
using Stater.Domain.Models;

namespace Stater.CodeGeneration.Tests.LanguageAdapter;

public class DoorStateMachineTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var open = new State(
            Guid.NewGuid(),
            "OPEN",
            "Open door",
            StateType.Start,
            0,
            0,
            new List<Event>(),
            new List<Event>()
        );

        var close = new State(
            Guid.NewGuid(),
            "CLOSE",
            "Close door",
            StateType.End,
            0,
            0,
            new List<Event>(),
            new List<Event>()
        );

        var ajar = new State(
            Guid.NewGuid(),
            "AJAR",
            "Ajar door",
            StateType.Common,
            0,
            0,
            new List<Event>(),
            new List<Event>()
        );

        var states = new List<State>
        {
            open, ajar, close
        };

        var degreeOfOpening = new Variable(
            Guid.NewGuid(),
            "degreeOfOpening",
            new VariableValue.IntVariable(0)
        );


        var preOpen = new Transition(
            Guid.NewGuid(),
            "preOpen",
            close.Guid,
            ajar.Guid,
            null,
            new Event.VariableSet(degreeOfOpening.Guid, new VariableValue.IntVariable(1))
        );

        var preClose = new Transition(
            Guid.NewGuid(),
            "preClose",
            open.Guid,
            ajar.Guid,
            null,
            new Event.VariableSet(degreeOfOpening.Guid, new VariableValue.IntVariable(99))
        );

        var openEvent = new Transition(
            Guid.NewGuid(),
            "open",
            ajar.Guid,
            open.Guid,
            new Condition.VariableCondition(degreeOfOpening.Guid, Condition.VariableCondition.ConditionTypeEnum.Ge,
                new VariableValue.IntVariable(99)),
            new Event.VariableSet(degreeOfOpening.Guid, new VariableValue.IntVariable(100))
        );

        var closeEvent = new Transition(
            Guid.NewGuid(),
            "close",
            ajar.Guid,
            close.Guid,
            new Condition.VariableCondition(degreeOfOpening.Guid, Condition.VariableCondition.ConditionTypeEnum.Le,
                new VariableValue.IntVariable(1)),
            new Event.VariableSet(degreeOfOpening.Guid, new VariableValue.IntVariable(0))
        );

        var ajarPlus = new Transition(
            Guid.NewGuid(),
            "ajarPlus",
            ajar.Guid,
            ajar.Guid,
            null, // TODO
            new Event.VariableMath(degreeOfOpening.Guid, Event.VariableMath.MathTypeEnum.Sum,
                new VariableValue.IntVariable(1))
        );

        var ajarMinus = new Transition(
            Guid.NewGuid(),
            "ajarMinus",
            ajar.Guid,
            ajar.Guid,
            null, // TODO
            new Event.VariableMath(degreeOfOpening.Guid, Event.VariableMath.MathTypeEnum.Sub,
                new VariableValue.IntVariable(1))
        );


        var transitions = new List<Transition>
        {
            preOpen,
            preClose,
            openEvent,
            closeEvent,
            ajarPlus,
            ajarMinus
        };

        var stateMachine = new StateMachine(
            Guid.NewGuid(),
            "Door",
            states,
            transitions,
            new List<Variable> { degreeOfOpening }
        );
        yield return new object[] { stateMachine };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}