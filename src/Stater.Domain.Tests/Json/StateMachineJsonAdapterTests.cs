using Stater.Domain.Json;
using Stater.Domain.Models;
using Xunit;

namespace Stater.Domain.Tests.Json;

public class StateMachineJsonAdapterTests
{
    private static StateMachine DoorTestStateMachine()
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
            StateType.Common,
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
            ajar, close, open 
        };


        var preOpen = new Transition(
            Guid.NewGuid(),
            "preOpen",
            close.Guid,
            ajar.Guid,
            null,
            null
        );

        var preClose = new Transition(
            Guid.NewGuid(),
            "preClose",
            open.Guid,
            ajar.Guid,
            null,
            null
        );

        var openEvent = new Transition(
            Guid.NewGuid(),
            "open",
            ajar.Guid,
            open.Guid,
            null,
            null
        );

        var closeEvent = new Transition(
            Guid.NewGuid(),
            "close",
            ajar.Guid,
            close.Guid,
            null,
            null
        );

        var ajarPlus = new Transition(
            Guid.NewGuid(),
            "ajarPlus",
            ajar.Guid,
            ajar.Guid,
            null,
            null
        );

        var ajarMinus = new Transition(
            Guid.NewGuid(),
            "ajarMinus",
            ajar.Guid,
            ajar.Guid,
            null,
            null
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

        return new StateMachine(
            Guid.NewGuid(),
            "Door",
            states,
            transitions,
            new List<Variable>()
        );
    }

    private const string Json =
        """
        {
          "states": [
            "AJAR",
            "CLOSE",
            "OPEN"
          ],
          "startState": "OPEN",
          "transitions": [
            {
              "name": "preOpen",
              "start": "CLOSE",
              "end": "AJAR"
            },
            {
              "name": "preClose",
              "start": "OPEN",
              "end": "AJAR"
            },
            {
              "name": "open",
              "start": "AJAR",
              "end": "OPEN"
            },
            {
              "name": "close",
              "start": "AJAR",
              "end": "CLOSE"
            },
            {
              "name": "ajarPlus",
              "start": "AJAR",
              "end": "AJAR"
            },
            {
              "name": "ajarMinus",
              "start": "AJAR",
              "end": "AJAR"
            }
          ]
        }
        """;

    [Fact]
    public void TestToJsonSchema()
    {
        var sm = DoorTestStateMachine();
        var result = StateMachineJsonAdapter.ToJsonSchema(sm);

        Assert.Equal(Json, result);
    }

    [Fact]
    public void TestFromJsonSchema()
    {
        var actualSm = DoorTestStateMachine();
        var sm = StateMachineJsonAdapter.FromJsonSchema(Json);
        Assert.Equal(actualSm.States.Count, sm.States.Count);

        for (var i = 0; i < actualSm.States.Count; i++)
        {
            Assert.Equal(actualSm.States[i].Name, sm.States[i].Name);
            Assert.Equal(actualSm.States[i].Type, sm.States[i].Type);
        }


        var actualStateTransition = actualSm.StateTransitions;
        var stateTransition = sm.StateTransitions;
        Assert.Equal(actualStateTransition.Count, stateTransition.Count);

        for (var i = 0; i < actualStateTransition.Count; i++)
        {
            Assert.Equal(actualStateTransition[i].Transition.Name, stateTransition[i].Transition.Name);
            Assert.Equal(actualStateTransition[i].StartState.Name, stateTransition[i].StartState.Name);
            Assert.Equal(actualStateTransition[i].EndState.Name, stateTransition[i].EndState.Name);
        }
    }
}