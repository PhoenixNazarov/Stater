using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Kotlin;
using Stater.Domain.Models;
using Xunit;

namespace Stater.CodeGeneration.Tests.LanguageAdapter.Kotlin;


public class KotlinAdapterDoorTests
{
    private readonly KotlinAdapter adapter = new();

    [Theory]
    [ClassData(typeof(DoorStateMachineTestData))]
    public void ClazGenerate(StateMachine stateMachine)
    {
        var settings = new GenerationSettings(
            Language.Kotlin,
            Mode.Claz,
            false,
            false,
            false
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal(
            """
            class DoorStateMachine : StaterStateMachine<String, EmptyContext>(
                transitions = listOf(
                    Transition(
                        name = "preOpen",
                        start = "CLOSE",
                        end = "AJAR"
                    ),
                    Transition(
                        name = "preClose",
                        start = "OPEN",
                        end = "AJAR"
                    ),
                    Transition(
                        name = "open",
                        start = "AJAR",
                        end = "OPEN"
                    ),
                    Transition(
                        name = "close",
                        start = "AJAR",
                        end = "CLOSE"
                    ),
                    Transition(
                        name = "ajarPlus",
                        start = "AJAR",
                        end = "AJAR"
                    ),
                    Transition(
                        name = "ajarMinus",
                        start = "AJAR",
                        end = "AJAR"
                    )
                ),
                startState = "OPEN",
                context = EmptyContext()
            )
            """, result);
    }
    
    [Theory]
    [ClassData(typeof(DoorStateMachineTestData))]
    public void ClazGenerateWithInterface(StateMachine stateMachine)
    {
        var settings = new GenerationSettings(
            Language.Kotlin,
            Mode.Claz,
            false,
            false,
            true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal(
            """
            class DoorStateMachine : StaterStateMachine<String, EmptyContext>(
                transitions = listOf(
                    Transition(
                        name = "preOpen",
                        start = "CLOSE",
                        end = "AJAR"
                    ),
                    Transition(
                        name = "preClose",
                        start = "OPEN",
                        end = "AJAR"
                    ),
                    Transition(
                        name = "open",
                        start = "AJAR",
                        end = "OPEN"
                    ),
                    Transition(
                        name = "close",
                        start = "AJAR",
                        end = "CLOSE"
                    ),
                    Transition(
                        name = "ajarPlus",
                        start = "AJAR",
                        end = "AJAR"
                    ),
                    Transition(
                        name = "ajarMinus",
                        start = "AJAR",
                        end = "AJAR"
                    )
                ),
                startState = "OPEN",
                context = EmptyContext()
            )
            """, result);
    }
}