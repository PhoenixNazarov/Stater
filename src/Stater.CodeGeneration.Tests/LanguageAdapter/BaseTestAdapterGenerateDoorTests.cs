using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Base;

namespace Stater.CodeGeneration.Tests.LanguageAdapter;

public abstract class BaseTestAdapterGenerateDoorTests
{
    protected abstract BaseLanguageAdapter Adapter { get; }
    protected abstract Language Language { get; }
    
    
    
    [Fact]
    public Task ClazzGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language
        );
        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task ClazzGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language,
            GenerateInterface: true
        );

        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task ClazzStateGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language,
            GenerateStates: true
        );

        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task ClazzStateGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language,
            GenerateStates: true,
            GenerateInterface: true
        );

        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task ClazzContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language,
            GenerateContext: true
        );

        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task ClazzContextGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task ClazzStateContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language,
            GenerateStates: true,
            GenerateContext: true
        );

        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task ClazzStateContextGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language,
            GenerateStates: true,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language,
            Mode.Builder
        );

        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language,
            Mode.Builder,
            GenerateInterface: true
        );

        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderStateGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language,
            Mode.Builder,
            GenerateStates: true
        );

        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderStateGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language,
            Mode.Builder,
            GenerateStates: true,
            GenerateInterface: true
        );

        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language,
            Mode.Builder,
            GenerateContext: true
        );

        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderContextGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language,
            Mode.Builder,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderStateContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language,
            Mode.Builder,
            GenerateStates: true,
            GenerateContext: true
        );

        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderStateContextGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var settings = new GenerationSettings(
            Language,
            Mode.Builder,
            GenerateStates: true,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = Adapter.Generate(stateMachine, settings);
        return Verify(result).UseDirectory(Language.ToString());
    }
}