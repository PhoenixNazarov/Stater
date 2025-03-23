using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Kotlin;
using Stater.Domain.Models;
using Xunit;

namespace Stater.CodeGeneration.Tests.LanguageAdapter.Kotlin;

public class KotlinAdapterDoorTests
{
    private readonly KotlinAdapter adapter = new();

    [Fact]
    public void ClazzGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin
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

    [Fact]
    public void ClazzGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin,
            GenerateInterface: true
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
            ) {
                fun preOpen() = transition("preOpen")
                fun preClose() = transition("preClose")
                fun open() = transition("open")
                fun close() = transition("close")
                fun ajarPlus() = transition("ajarPlus")
                fun ajarMinus() = transition("ajarMinus")
            }
            """, result);
    }

    [Fact]
    public void ClazzStateGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin,
            GenerateStates: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal(
            """
            enum class States {
                OPEN,
                AJAR,
                CLOSE
            }

            class DoorStateMachine : StaterStateMachine<States, EmptyContext>(
                transitions = listOf(
                    Transition(
                        name = "preOpen",
                        start = States.CLOSE,
                        end = States.AJAR
                    ),
                    Transition(
                        name = "preClose",
                        start = States.OPEN,
                        end = States.AJAR
                    ),
                    Transition(
                        name = "open",
                        start = States.AJAR,
                        end = States.OPEN
                    ),
                    Transition(
                        name = "close",
                        start = States.AJAR,
                        end = States.CLOSE
                    ),
                    Transition(
                        name = "ajarPlus",
                        start = States.AJAR,
                        end = States.AJAR
                    ),
                    Transition(
                        name = "ajarMinus",
                        start = States.AJAR,
                        end = States.AJAR
                    )
                ),
                startState = States.OPEN,
                context = EmptyContext()
            )
            """, result);
    }

    [Fact]
    public void ClazzStateGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin,
            GenerateStates: true,
            GenerateInterface: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal(
            """
            enum class States {
                OPEN,
                AJAR,
                CLOSE
            }

            class DoorStateMachine : StaterStateMachine<States, EmptyContext>(
                transitions = listOf(
                    Transition(
                        name = "preOpen",
                        start = States.CLOSE,
                        end = States.AJAR
                    ),
                    Transition(
                        name = "preClose",
                        start = States.OPEN,
                        end = States.AJAR
                    ),
                    Transition(
                        name = "open",
                        start = States.AJAR,
                        end = States.OPEN
                    ),
                    Transition(
                        name = "close",
                        start = States.AJAR,
                        end = States.CLOSE
                    ),
                    Transition(
                        name = "ajarPlus",
                        start = States.AJAR,
                        end = States.AJAR
                    ),
                    Transition(
                        name = "ajarMinus",
                        start = States.AJAR,
                        end = States.AJAR
                    )
                ),
                startState = States.OPEN,
                context = EmptyContext()
            ) {
                fun preOpen() = transition("preOpen")
                fun preClose() = transition("preClose")
                fun open() = transition("open")
                fun close() = transition("close")
                fun ajarPlus() = transition("ajarPlus")
                fun ajarMinus() = transition("ajarMinus")
            }
            """, result);
    }

    [Fact]
    public void ClazzContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin,
            GenerateContext: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     data class DoorFSMContext(
                         var degreeOfOpening: Int = 100,
                     ) : Context

                     class DoorStateMachine : StaterStateMachine<String, DoorFSMContext>(
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
                         context = DoorFSMContext()
                     )
                     """, result);
    }

    [Fact]
    public void ClazzContextGenerateWithContext()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     data class DoorFSMContext(
                         var degreeOfOpening: Int = 100,
                     ) : Context

                     class DoorStateMachine : StaterStateMachine<String, DoorFSMContext>(
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
                         context = DoorFSMContext()
                     ) {
                         fun preOpen() = transition("preOpen")
                         fun preClose() = transition("preClose")
                         fun open() = transition("open")
                         fun close() = transition("close")
                         fun ajarPlus() = transition("ajarPlus")
                         fun ajarMinus() = transition("ajarMinus")
                     }
                     """, result);
    }

    [Fact]
    public void ClazzStateContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin,
            GenerateStates: true,
            GenerateContext: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     enum class States {
                         OPEN,
                         AJAR,
                         CLOSE
                     }

                     data class DoorFSMContext(
                         var degreeOfOpening: Int = 100,
                     ) : Context

                     class DoorStateMachine : StaterStateMachine<States, DoorFSMContext>(
                         transitions = listOf(
                             Transition(
                                 name = "preOpen",
                                 start = States.CLOSE,
                                 end = States.AJAR
                             ),
                             Transition(
                                 name = "preClose",
                                 start = States.OPEN,
                                 end = States.AJAR
                             ),
                             Transition(
                                 name = "open",
                                 start = States.AJAR,
                                 end = States.OPEN
                             ),
                             Transition(
                                 name = "close",
                                 start = States.AJAR,
                                 end = States.CLOSE
                             ),
                             Transition(
                                 name = "ajarPlus",
                                 start = States.AJAR,
                                 end = States.AJAR
                             ),
                             Transition(
                                 name = "ajarMinus",
                                 start = States.AJAR,
                                 end = States.AJAR
                             )
                         ),
                         startState = States.OPEN,
                         context = DoorFSMContext()
                     )
                     """, result);
    }

    [Fact]
    public void ClazzStateContextGenerateWithContext()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin,
            GenerateStates: true,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     enum class States {
                         OPEN,
                         AJAR,
                         CLOSE
                     }

                     data class DoorFSMContext(
                         var degreeOfOpening: Int = 100,
                     ) : Context

                     class DoorStateMachine : StaterStateMachine<States, DoorFSMContext>(
                         transitions = listOf(
                             Transition(
                                 name = "preOpen",
                                 start = States.CLOSE,
                                 end = States.AJAR
                             ),
                             Transition(
                                 name = "preClose",
                                 start = States.OPEN,
                                 end = States.AJAR
                             ),
                             Transition(
                                 name = "open",
                                 start = States.AJAR,
                                 end = States.OPEN
                             ),
                             Transition(
                                 name = "close",
                                 start = States.AJAR,
                                 end = States.CLOSE
                             ),
                             Transition(
                                 name = "ajarPlus",
                                 start = States.AJAR,
                                 end = States.AJAR
                             ),
                             Transition(
                                 name = "ajarMinus",
                                 start = States.AJAR,
                                 end = States.AJAR
                             )
                         ),
                         startState = States.OPEN,
                         context = DoorFSMContext()
                     ) {
                         fun preOpen() = transition("preOpen")
                         fun preClose() = transition("preClose")
                         fun open() = transition("open")
                         fun close() = transition("close")
                         fun ajarPlus() = transition("ajarPlus")
                         fun ajarMinus() = transition("ajarMinus")
                     }
                     """, result);
    }
    
    [Fact]
    public void BuilderGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin,
            Mode.Builder
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal(
            """
            val builderDoorStateMachine = StaterStateMachineBuilder<String, EmptyContext>()
                .setStartState("OPEN")
                .setContext(EmptyContext())
                .addTransition("preOpen", "CLOSE", "AJAR")
                .addTransition("preClose", "OPEN", "AJAR")
                .addTransition("open", "AJAR", "OPEN")
                .addTransition("close", "AJAR", "CLOSE")
                .addTransition("ajarPlus", "AJAR", "AJAR")
                .addTransition("ajarMinus", "AJAR", "AJAR")
            """, result);
    }

    [Fact]
    public void BuilderGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin,
            Mode.Builder,
            GenerateInterface: true
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
            ) {
                fun preOpen() = transition("preOpen")
                fun preClose() = transition("preClose")
                fun open() = transition("open")
                fun close() = transition("close")
                fun ajarPlus() = transition("ajarPlus")
                fun ajarMinus() = transition("ajarMinus")
            }
            """, result);
    }

    [Fact]
    public void BuilderStateGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin,
            Mode.Builder,
            GenerateStates: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal(
            """
            enum class States {
                OPEN,
                AJAR,
                CLOSE
            }
            
            val builderDoorStateMachine = StaterStateMachineBuilder<States, EmptyContext>()
                .setStartState(States.OPEN)
                .setContext(EmptyContext())
                .addTransition("preOpen", States.CLOSE, States.AJAR)
                .addTransition("preClose", States.OPEN, States.AJAR)
                .addTransition("open", States.AJAR, States.OPEN)
                .addTransition("close", States.AJAR, States.CLOSE)
                .addTransition("ajarPlus", States.AJAR, States.AJAR)
                .addTransition("ajarMinus", States.AJAR, States.AJAR)
            """, result);
    }

    [Fact]
    public void BuilderStateGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin,
            Mode.Builder,
            GenerateStates: true,
            GenerateInterface: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal(
            """
            enum class States {
                OPEN,
                AJAR,
                CLOSE
            }

            class DoorStateMachine : StaterStateMachine<States, EmptyContext>(
                transitions = listOf(
                    Transition(
                        name = "preOpen",
                        start = States.CLOSE,
                        end = States.AJAR
                    ),
                    Transition(
                        name = "preClose",
                        start = States.OPEN,
                        end = States.AJAR
                    ),
                    Transition(
                        name = "open",
                        start = States.AJAR,
                        end = States.OPEN
                    ),
                    Transition(
                        name = "close",
                        start = States.AJAR,
                        end = States.CLOSE
                    ),
                    Transition(
                        name = "ajarPlus",
                        start = States.AJAR,
                        end = States.AJAR
                    ),
                    Transition(
                        name = "ajarMinus",
                        start = States.AJAR,
                        end = States.AJAR
                    )
                ),
                startState = States.OPEN,
                context = EmptyContext()
            ) {
                fun preOpen() = transition("preOpen")
                fun preClose() = transition("preClose")
                fun open() = transition("open")
                fun close() = transition("close")
                fun ajarPlus() = transition("ajarPlus")
                fun ajarMinus() = transition("ajarMinus")
            }
            """, result);
    }

    [Fact]
    public void BuilderContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin,
            Mode.Builder,
            GenerateContext: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     data class DoorFSMContext(
                         var degreeOfOpening: Int = 100,
                     ) : Context
                     
                     val builderDoorStateMachine = StaterStateMachineBuilder<String, DoorFSMContext>()
                         .setStartState("OPEN")
                         .setContext(DoorFSMContext())
                         .addTransition("preOpen", "CLOSE", "AJAR")
                         .addTransition("preClose", "OPEN", "AJAR")
                         .addTransition("open", "AJAR", "OPEN")
                         .addTransition("close", "AJAR", "CLOSE")
                         .addTransition("ajarPlus", "AJAR", "AJAR")
                         .addTransition("ajarMinus", "AJAR", "AJAR")
                     """, result);
    }

    [Fact]
    public void BuilderContextGenerateWithContext()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin,
            Mode.Builder,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     data class DoorFSMContext(
                         var degreeOfOpening: Int = 100,
                     ) : Context

                     class DoorStateMachine : StaterStateMachine<String, DoorFSMContext>(
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
                         context = DoorFSMContext()
                     ) {
                          fun preOpen() = transition("preOpen")
                          fun preClose() = transition("preClose")
                          fun open() = transition("open")
                          fun close() = transition("close")
                          fun ajarPlus() = transition("ajarPlus")
                          fun ajarMinus() = transition("ajarMinus")
                     }
                     """, result);
    }

    [Fact]
    public void BuilderStateContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin,
            Mode.Builder,
            GenerateStates: true,
            GenerateContext: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     enum class States {
                         OPEN,
                         AJAR,
                         CLOSE
                     }
                     
                     data class DoorFSMContext(
                         var degreeOfOpening: Int = 100,
                     ) : Context
                     
                     val builderDoorStateMachine = StaterStateMachineBuilder<States, DoorFSMContext>()
                         .setStartState(States.OPEN)
                         .setContext(DoorFSMContext())
                         .addTransition("preOpen", States.CLOSE, States.AJAR)
                         .addTransition("preClose", States.OPEN, States.AJAR)
                         .addTransition("open", States.AJAR, States.OPEN)
                         .addTransition("close", States.AJAR, States.CLOSE)
                         .addTransition("ajarPlus", States.AJAR, States.AJAR)
                         .addTransition("ajarMinus", States.AJAR, States.AJAR)
                     """, result);
    }

    [Fact]
    public void BuilderStateContextGenerateWithContext()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Kotlin,
            Mode.Builder,
            GenerateStates: true,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     enum class States {
                         OPEN,
                         AJAR,
                         CLOSE
                     }

                     data class DoorFSMContext(
                         var degreeOfOpening: Int = 100,
                     ) : Context

                     class DoorStateMachine : StaterStateMachine<States, EmptyContext>(
                         transitions = listOf(
                             Transition(
                                 name = "preOpen",
                                 start = States.CLOSE,
                                 end = States.AJAR
                             ),
                             Transition(
                                 name = "preClose",
                                 start = States.OPEN,
                                 end = States.AJAR
                             ),
                             Transition(
                                 name = "open",
                                 start = States.AJAR,
                                 end = States.OPEN
                             ),
                             Transition(
                                 name = "close",
                                 start = States.AJAR,
                                 end = States.CLOSE
                             ),
                             Transition(
                                 name = "ajarPlus",
                                 start = States.AJAR,
                                 end = States.AJAR
                             ),
                             Transition(
                                 name = "ajarMinus",
                                 start = States.AJAR,
                                 end = States.AJAR
                             )
                         ),
                         startState = States.OPEN,
                         context = EmptyContext()
                     ) {
                         fun preOpen() = transition("preOpen")
                         fun preClose() = transition("preClose")
                         fun open() = transition("open")
                         fun close() = transition("close")
                         fun ajarPlus() = transition("ajarPlus")
                         fun ajarMinus() = transition("ajarMinus")
                     }
                     """, result);
    }
}