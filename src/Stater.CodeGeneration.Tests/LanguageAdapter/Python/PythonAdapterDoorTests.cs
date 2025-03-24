using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Python;
using Xunit;

namespace Stater.CodeGeneration.Tests.LanguageAdapter.Python;

public class PythonAdapterDoorTests
{
    private readonly PythonAdapter adapter = new();

    [Fact]
    public void ClazzGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal(
            """
            from stater_state_machine import EmptyContext
            from stater_state_machine import StaterStateMachine
            from stater_state_machine import Transition


            class DoorStateMachine(StaterStateMachine[str, EmptyContext]):
                def __init__(self):
                    super().__init__(
                        transitions=[
                            Transition(
                                name="preOpen",
                                start="CLOSE",
                                end="AJAR"
                            ),
                            Transition(
                                name="preClose",
                                start="OPEN",
                                end="AJAR"
                            ),
                            Transition(
                                name="open",
                                start="AJAR",
                                end="OPEN"
                            ),
                            Transition(
                                name="close",
                                start="AJAR",
                                end="CLOSE"
                            ),
                            Transition(
                                name="ajarPlus",
                                start="AJAR",
                                end="AJAR"
                            ),
                            Transition(
                                name="ajarMinus",
                                start="AJAR",
                                end="AJAR"
                            )
                        ],
                        context=EmptyContext(),
                        start_state="OPEN",
                    )
            """, result);
    }

    [Fact]
    public void ClazzGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3,
            GenerateInterface: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal(
            """
            from stater_state_machine import EmptyContext
            from stater_state_machine import StaterStateMachine
            from stater_state_machine import Transition


            class DoorStateMachine(StaterStateMachine[str, EmptyContext]):
                def __init__(self):
                    super().__init__(
                        transitions=[
                            Transition(
                                name="preOpen",
                                start="CLOSE",
                                end="AJAR"
                            ),
                            Transition(
                                name="preClose",
                                start="OPEN",
                                end="AJAR"
                            ),
                            Transition(
                                name="open",
                                start="AJAR",
                                end="OPEN"
                            ),
                            Transition(
                                name="close",
                                start="AJAR",
                                end="CLOSE"
                            ),
                            Transition(
                                name="ajarPlus",
                                start="AJAR",
                                end="AJAR"
                            ),
                            Transition(
                                name="ajarMinus",
                                start="AJAR",
                                end="AJAR"
                            )
                        ],
                        context=EmptyContext(),
                        start_state="OPEN",
                    )
            
                def preOpen(self):
                    self.transition("preOpen")
            
                def preClose(self):
                    self.transition("preClose")
            
                def open(self):
                    self.transition("open")
            
                def close(self):
                    self.transition("close")
            
                def ajarPlus(self):
                    self.transition("ajarPlus")
            
                def ajarMinus(self):
                    self.transition("ajarMinus")
            """, result);
    }

    [Fact]
    public void ClazzStateGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3,
            GenerateStates: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal(
            """
            from enum import Enum
            from stater_state_machine import EmptyContext
            from stater_state_machine import StaterStateMachine
            from stater_state_machine import Transition


            class States(Enum):
                OPEN = "OPEN"
                AJAR = "AJAR"
                CLOSE = "CLOSE"
                
            
            class DoorStateMachine(StaterStateMachine[States, EmptyContext]):
                def __init__(self):
                    super().__init__(
                        transitions=[
                            Transition(
                                name="preOpen",
                                start=States.CLOSE,
                                end=States.AJAR
                            ),
                            Transition(
                                name="preClose",
                                start=States.OPEN,
                                end=States.AJAR
                            ),
                            Transition(
                                name="open",
                                start=States.AJAR,
                                end=States.OPEN
                            ),
                            Transition(
                                name="close",
                                start=States.AJAR,
                                end=States.CLOSE
                            ),
                            Transition(
                                name="ajarPlus",
                                start=States.AJAR,
                                end=States.AJAR
                            ),
                            Transition(
                                name="ajarMinus",
                                start=States.AJAR,
                                end=States.AJAR
                            )
                        ],
                        context=EmptyContext(),
                        start_state=States.OPEN,
                    )
            """, result);
    }

    [Fact]
    public void ClazzStateGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3,
            GenerateStates: true,
            GenerateInterface: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal(
            """
            from enum import Enum
            from stater_state_machine import EmptyContext
            from stater_state_machine import StaterStateMachine
            from stater_state_machine import Transition


            class States(Enum):
                OPEN = "OPEN"
                AJAR = "AJAR"
                CLOSE = "CLOSE"
                
            
            class DoorStateMachine(StaterStateMachine[States, EmptyContext]):
                def __init__(self):
                    super().__init__(
                        transitions=[
                            Transition(
                                name="preOpen",
                                start=States.CLOSE,
                                end=States.AJAR
                            ),
                            Transition(
                                name="preClose",
                                start=States.OPEN,
                                end=States.AJAR
                            ),
                            Transition(
                                name="open",
                                start=States.AJAR,
                                end=States.OPEN
                            ),
                            Transition(
                                name="close",
                                start=States.AJAR,
                                end=States.CLOSE
                            ),
                            Transition(
                                name="ajarPlus",
                                start=States.AJAR,
                                end=States.AJAR
                            ),
                            Transition(
                                name="ajarMinus",
                                start=States.AJAR,
                                end=States.AJAR
                            )
                        ],
                        context=EmptyContext(),
                        start_state=States.OPEN,
                    )
            
                def preOpen(self):
                    self.transition("preOpen")
            
                def preClose(self):
                    self.transition("preClose")
            
                def open(self):
                    self.transition("open")
            
                def close(self):
                    self.transition("close")
            
                def ajarPlus(self):
                    self.transition("ajarPlus")
            
                def ajarMinus(self):
                    self.transition("ajarMinus")
            """, result);
    }

    [Fact]
    public void ClazzContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3,
            GenerateContext: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     from pydantic import BaseModel
                     from stater_state_machine import Context
                     from stater_state_machine import StaterStateMachine
                     from stater_state_machine import Transition


                     class DoorFSMContext(BaseModel, Context):
                         degree_of_opening: int = 100


                     class DoorStateMachine(StaterStateMachine[str, DoorFSMContext]):
                         def __init__(self):
                             super().__init__(
                                 transitions=[
                                     Transition(
                                         name="preOpen",
                                         start="CLOSE",
                                         end="AJAR"
                                     ),
                                     Transition(
                                         name="preClose",
                                         start="OPEN",
                                         end="AJAR"
                                     ),
                                     Transition(
                                         name="open",
                                         start="AJAR",
                                         end="OPEN"
                                     ),
                                     Transition(
                                         name="close",
                                         start="AJAR",
                                         end="CLOSE"
                                     ),
                                     Transition(
                                         name="ajarPlus",
                                         start="AJAR",
                                         end="AJAR"
                                     ),
                                     Transition(
                                         name="ajarMinus",
                                         start="AJAR",
                                         end="AJAR"
                                     )
                                 ],
                                 context=DoorFSMContext(),
                                 start_state="OPEN",
                             )
                     """, result);
    }

    [Fact]
    public void ClazzContextGenerateWithContext()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     from pydantic import BaseModel
                     from stater_state_machine import Context
                     from stater_state_machine import StaterStateMachine
                     from stater_state_machine import Transition


                     class DoorFSMContext(BaseModel, Context):
                         degree_of_opening: int = 100


                     class DoorStateMachine(StaterStateMachine[str, DoorFSMContext]):
                         def __init__(self):
                             super().__init__(
                                 transitions=[
                                     Transition(
                                         name="preOpen",
                                         start="CLOSE",
                                         end="AJAR"
                                     ),
                                     Transition(
                                         name="preClose",
                                         start="OPEN",
                                         end="AJAR"
                                     ),
                                     Transition(
                                         name="open",
                                         start="AJAR",
                                         end="OPEN"
                                     ),
                                     Transition(
                                         name="close",
                                         start="AJAR",
                                         end="CLOSE"
                                     ),
                                     Transition(
                                         name="ajarPlus",
                                         start="AJAR",
                                         end="AJAR"
                                     ),
                                     Transition(
                                         name="ajarMinus",
                                         start="AJAR",
                                         end="AJAR"
                                     )
                                 ],
                                 context=DoorFSMContext(),
                                 start_state="OPEN",
                             )
                     
                         def preOpen(self):
                             self.transition("preOpen")
                     
                         def preClose(self):
                             self.transition("preClose")
                     
                         def open(self):
                             self.transition("open")
                     
                         def close(self):
                             self.transition("close")
                     
                         def ajarPlus(self):
                             self.transition("ajarPlus")
                     
                         def ajarMinus(self):
                             self.transition("ajarMinus")
                     """, result);
    }

    [Fact]
    public void ClazzStateContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3,
            GenerateStates: true,
            GenerateContext: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     from enum import Enum
                     from pydantic import BaseModel
                     from stater_state_machine import Context
                     from stater_state_machine import StaterStateMachine
                     from stater_state_machine import Transition


                     class States(Enum):
                         OPEN = "OPEN"
                         AJAR = "AJAR"
                         CLOSE = "CLOSE"
                         
                     
                     class DoorFSMContext(BaseModel, Context):
                         degree_of_opening: int = 100


                     class DoorStateMachine(StaterStateMachine[States, DoorFSMContext]):
                         def __init__(self):
                             super().__init__(
                                 transitions=[
                                     Transition(
                                         name="preOpen",
                                         start=States.CLOSE,
                                         end=States.AJAR
                                     ),
                                     Transition(
                                         name="preClose",
                                         start=States.OPEN,
                                         end=States.AJAR
                                     ),
                                     Transition(
                                         name="open",
                                         start=States.AJAR,
                                         end=States.OPEN
                                     ),
                                     Transition(
                                         name="close",
                                         start=States.AJAR,
                                         end=States.CLOSE
                                     ),
                                     Transition(
                                         name="ajarPlus",
                                         start=States.AJAR,
                                         end=States.AJAR
                                     ),
                                     Transition(
                                         name="ajarMinus",
                                         start=States.AJAR,
                                         end=States.AJAR
                                     )
                                 ],
                                 context=DoorFSMContext(),
                                 start_state=States.OPEN,
                             )
                     """, result);
    }

    [Fact]
    public void ClazzStateContextGenerateWithContext()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3,
            GenerateStates: true,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     from enum import Enum
                     from pydantic import BaseModel
                     from stater_state_machine import Context
                     from stater_state_machine import StaterStateMachine
                     from stater_state_machine import Transition


                     class States(Enum):
                         OPEN = "OPEN"
                         AJAR = "AJAR"
                         CLOSE = "CLOSE"
                         
                     
                     class DoorFSMContext(BaseModel, Context):
                         degree_of_opening: int = 100


                     class DoorStateMachine(StaterStateMachine[States, DoorFSMContext]):
                         def __init__(self):
                             super().__init__(
                                 transitions=[
                                     Transition(
                                         name="preOpen",
                                         start=States.CLOSE,
                                         end=States.AJAR
                                     ),
                                     Transition(
                                         name="preClose",
                                         start=States.OPEN,
                                         end=States.AJAR
                                     ),
                                     Transition(
                                         name="open",
                                         start=States.AJAR,
                                         end=States.OPEN
                                     ),
                                     Transition(
                                         name="close",
                                         start=States.AJAR,
                                         end=States.CLOSE
                                     ),
                                     Transition(
                                         name="ajarPlus",
                                         start=States.AJAR,
                                         end=States.AJAR
                                     ),
                                     Transition(
                                         name="ajarMinus",
                                         start=States.AJAR,
                                         end=States.AJAR
                                     )
                                 ],
                                 context=DoorFSMContext(),
                                 start_state=States.OPEN,
                             )
                     
                         def preOpen(self):
                             self.transition("preOpen")
                     
                         def preClose(self):
                             self.transition("preClose")
                     
                         def open(self):
                             self.transition("open")
                     
                         def close(self):
                             self.transition("close")
                     
                         def ajarPlus(self):
                             self.transition("ajarPlus")
                     
                         def ajarMinus(self):
                             self.transition("ajarMinus")
                     """, result);
    }

    [Fact]
    public void BuilderGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3,
            Mode.Builder
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal(
            """
            from stater_state_machine import EmptyContext
            from stater_state_machine import StaterStateMachineBuilder
            
            
            builder_Door_state_machine = (
                StaterStateMachineBuilder[str, EmptyContext]()
                .set_start_state("OPEN")
                .set_context(EmptyContext())
                .add_transition("preOpen", "CLOSE", "AJAR")
                .add_transition("preClose", "OPEN", "AJAR")
                .add_transition("open", "AJAR", "OPEN")
                .add_transition("close", "AJAR", "CLOSE")
                .add_transition("ajarPlus", "AJAR", "AJAR")
                .add_transition("ajarMinus", "AJAR", "AJAR")
            )
            """, result);
    }

    [Fact]
    public void BuilderGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3,
            Mode.Builder,
            GenerateInterface: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal(
            """
            from stater_state_machine import EmptyContext
            from stater_state_machine import StaterStateMachine
            from stater_state_machine import StaterStateMachineBuilder
            
            
            class TypesDoorStateMachine(StaterStateMachine[str, EmptyContext]):
                def preOpen(self):
                    self.transition("preOpen")
                
                def preClose(self):
                    self.transition("preClose")
                
                def open(self):
                    self.transition("open")
                
                def close(self):
                    self.transition("close")
                
                def ajarPlus(self):
                    self.transition("ajarPlus")
                
                def ajarMinus(self):
                    self.transition("ajarMinus")
            
            
            def typed_Door_factory(*args, **kwargs):
                return TypesDoorStateMachine(StaterStateMachine(*args, **kwargs))
            
            
            builder_Door_state_machine = (
                StaterStateMachineBuilder[str, EmptyContext]()
                .set_start_state("OPEN")
                .set_context(EmptyContext())
                .set_factory(typed_Door_factory)
                .add_transition("preOpen", "CLOSE", "AJAR")
                .add_transition("preClose", "OPEN", "AJAR")
                .add_transition("open", "AJAR", "OPEN")
                .add_transition("close", "AJAR", "CLOSE")
                .add_transition("ajarPlus", "AJAR", "AJAR")
                .add_transition("ajarMinus", "AJAR", "AJAR")
            )
            """, result);
    }

    [Fact]
    public void BuilderStateGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3,
            Mode.Builder,
            GenerateStates: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal(
            """
            from enum import Enum
            from stater_state_machine import EmptyContext
            from stater_state_machine import StaterStateMachineBuilder
            
            
            class States(Enum):
                OPEN = "OPEN"
                AJAR = "AJAR"
                CLOSE = "CLOSE"
                
            
            builder_Door_state_machine = (
                StaterStateMachineBuilder[States, EmptyContext]()
                .set_start_state(States.OPEN)
                .set_context(EmptyContext())
                .add_transition("preOpen", States.CLOSE, States.AJAR)
                .add_transition("preClose", States.OPEN, States.AJAR)
                .add_transition("open", States.AJAR, States.OPEN)
                .add_transition("close", States.AJAR, States.CLOSE)
                .add_transition("ajarPlus", States.AJAR, States.AJAR)
                .add_transition("ajarMinus", States.AJAR, States.AJAR)
            )
            """, result);
    }

    [Fact]
    public void BuilderStateGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3,
            Mode.Builder,
            GenerateStates: true,
            GenerateInterface: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal(
            """
            from enum import Enum
            from stater_state_machine import EmptyContext
            from stater_state_machine import StaterStateMachine
            from stater_state_machine import StaterStateMachineBuilder
            
            
            class States(Enum):
                OPEN = "OPEN"
                AJAR = "AJAR"
                CLOSE = "CLOSE"
                
            
            class TypesDoorStateMachine(StaterStateMachine[States, EmptyContext]):
                def preOpen(self):
                    self.transition("preOpen")
                
                def preClose(self):
                    self.transition("preClose")
                
                def open(self):
                    self.transition("open")
                
                def close(self):
                    self.transition("close")
                
                def ajarPlus(self):
                    self.transition("ajarPlus")
                
                def ajarMinus(self):
                    self.transition("ajarMinus")
            
            
            def typed_Door_factory(*args, **kwargs):
                return TypesDoorStateMachine(StaterStateMachine(*args, **kwargs))
            
            
            builder_Door_state_machine = (
                StaterStateMachineBuilder[States, EmptyContext]()
                .set_start_state(States.OPEN)
                .set_context(EmptyContext())
                .set_factory(typed_Door_factory)
                .add_transition("preOpen", States.CLOSE, States.AJAR)
                .add_transition("preClose", States.OPEN, States.AJAR)
                .add_transition("open", States.AJAR, States.OPEN)
                .add_transition("close", States.AJAR, States.CLOSE)
                .add_transition("ajarPlus", States.AJAR, States.AJAR)
                .add_transition("ajarMinus", States.AJAR, States.AJAR)
            )
            """, result);
    }

    [Fact]
    public void BuilderContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3,
            Mode.Builder,
            GenerateContext: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     from pydantic import BaseModel
                     from stater_state_machine import Context
                     from stater_state_machine import StaterStateMachineBuilder
                     
                     
                     class DoorFSMContext(BaseModel, Context):
                         degree_of_opening: int = 100
                     
                     
                     builder_Door_state_machine = (
                         StaterStateMachineBuilder[str, DoorFSMContext]()
                         .set_start_state("OPEN")
                         .set_context(DoorFSMContext())
                         .add_transition("preOpen", "CLOSE", "AJAR")
                         .add_transition("preClose", "OPEN", "AJAR")
                         .add_transition("open", "AJAR", "OPEN")
                         .add_transition("close", "AJAR", "CLOSE")
                         .add_transition("ajarPlus", "AJAR", "AJAR")
                         .add_transition("ajarMinus", "AJAR", "AJAR")
                     )
                     """, result);
    }

    [Fact]
    public void BuilderContextGenerateWithContext()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3,
            Mode.Builder,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     from pydantic import BaseModel
                     from stater_state_machine import Context
                     from stater_state_machine import StaterStateMachine
                     from stater_state_machine import StaterStateMachineBuilder
                     
                     
                     class DoorFSMContext(BaseModel, Context):
                         degree_of_opening: int = 100
                     
                     
                     class TypesDoorStateMachine(StaterStateMachine[str, DoorFSMContext]):
                         def preOpen(self):
                             self.transition("preOpen")
                         
                         def preClose(self):
                             self.transition("preClose")
                         
                         def open(self):
                             self.transition("open")
                         
                         def close(self):
                             self.transition("close")
                         
                         def ajarPlus(self):
                             self.transition("ajarPlus")
                         
                         def ajarMinus(self):
                             self.transition("ajarMinus")
                     
                     
                     def typed_Door_factory(*args, **kwargs):
                         return TypesDoorStateMachine(StaterStateMachine(*args, **kwargs))
                     
                     
                     builder_Door_state_machine = (
                         StaterStateMachineBuilder[str, DoorFSMContext]()
                         .set_start_state("OPEN")
                         .set_context(DoorFSMContext())
                         .set_factory(typed_Door_factory)
                         .add_transition("preOpen", "CLOSE", "AJAR")
                         .add_transition("preClose", "OPEN", "AJAR")
                         .add_transition("open", "AJAR", "OPEN")
                         .add_transition("close", "AJAR", "CLOSE")
                         .add_transition("ajarPlus", "AJAR", "AJAR")
                         .add_transition("ajarMinus", "AJAR", "AJAR")
                     )
                     """, result);
    }

    [Fact]
    public void BuilderStateContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3,
            Mode.Builder,
            GenerateStates: true,
            GenerateContext: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     from enum import Enum
                     from pydantic import BaseModel
                     from stater_state_machine import Context
                     from stater_state_machine import StaterStateMachineBuilder
                     
                     
                     class States(Enum):
                         OPEN = "OPEN"
                         AJAR = "AJAR"
                         CLOSE = "CLOSE"
                         
                     
                     class DoorFSMContext(BaseModel, Context):
                         degree_of_opening: int = 100
                     
                     
                     builder_Door_state_machine = (
                         StaterStateMachineBuilder[States, DoorFSMContext]()
                         .set_start_state(States.OPEN)
                         .set_context(DoorFSMContext())
                         .add_transition("preOpen", States.CLOSE, States.AJAR)
                         .add_transition("preClose", States.OPEN, States.AJAR)
                         .add_transition("open", States.AJAR, States.OPEN)
                         .add_transition("close", States.AJAR, States.CLOSE)
                         .add_transition("ajarPlus", States.AJAR, States.AJAR)
                         .add_transition("ajarMinus", States.AJAR, States.AJAR)
                     )
                     """, result);
    }

    [Fact]
    public void BuilderStateContextGenerateWithContext()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language.Python3,
            Mode.Builder,
            GenerateStates: true,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = adapter.Generate(stateMachine, settings);
        Assert.Equal("""
                     from enum import Enum
                     from pydantic import BaseModel
                     from stater_state_machine import Context
                     from stater_state_machine import StaterStateMachine
                     from stater_state_machine import StaterStateMachineBuilder
                     
                     
                     class States(Enum):
                         OPEN = "OPEN"
                         AJAR = "AJAR"
                         CLOSE = "CLOSE"
                         
                     
                     class DoorFSMContext(BaseModel, Context):
                         degree_of_opening: int = 100
                     
                     
                     class TypesDoorStateMachine(StaterStateMachine[States, DoorFSMContext]):
                         def preOpen(self):
                             self.transition("preOpen")
                         
                         def preClose(self):
                             self.transition("preClose")
                         
                         def open(self):
                             self.transition("open")
                         
                         def close(self):
                             self.transition("close")
                         
                         def ajarPlus(self):
                             self.transition("ajarPlus")
                         
                         def ajarMinus(self):
                             self.transition("ajarMinus")
                     
                     
                     def typed_Door_factory(*args, **kwargs):
                         return TypesDoorStateMachine(StaterStateMachine(*args, **kwargs))
                     
                     
                     builder_Door_state_machine = (
                         StaterStateMachineBuilder[States, DoorFSMContext]()
                         .set_start_state(States.OPEN)
                         .set_context(DoorFSMContext())
                         .set_factory(typed_Door_factory)
                         .add_transition("preOpen", States.CLOSE, States.AJAR)
                         .add_transition("preClose", States.OPEN, States.AJAR)
                         .add_transition("open", States.AJAR, States.OPEN)
                         .add_transition("close", States.AJAR, States.CLOSE)
                         .add_transition("ajarPlus", States.AJAR, States.AJAR)
                         .add_transition("ajarMinus", States.AJAR, States.AJAR)
                     )
                     """, result);
    }
}