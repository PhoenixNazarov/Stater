using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.LanguageAdapter.Base;

namespace Stater.CodeGeneration.Tests.LanguageAdapter;

public abstract class BaseTestAdapterGenerateTestsDoorTests
{
    protected abstract BaseLanguageAdapter Adapter { get; }
    protected abstract Language Language { get; }
    
    
    
    [Fact]
    public Task ClazzTestsGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language
        );
        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task ClazzTestsGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language,
            GenerateInterface: true
        );

        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task ClazzTestsStateGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language,
            GenerateStates: true
        );

        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task ClazzTestsStateGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language,
            GenerateStates: true,
            GenerateInterface: true
        );

        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task ClazzTestsContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language,
            GenerateContext: true
        );

        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task ClazzTestsContextGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task ClazzTestsStateContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language,
            GenerateStates: true,
            GenerateContext: true
        );

        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task ClazzTestsStateContextGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language,
            GenerateStates: true,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderTestsGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language,
            Mode.Builder
        );

        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderTestsGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language,
            Mode.Builder,
            GenerateInterface: true
        );

        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderTestsStateGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language,
            Mode.Builder,
            GenerateStates: true
        );

        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderTestsStateGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language,
            Mode.Builder,
            GenerateStates: true,
            GenerateInterface: true
        );

        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderTestsContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language,
            Mode.Builder,
            GenerateContext: true
        );

        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderTestsContextGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language,
            Mode.Builder,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderTestsStateContextGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language,
            Mode.Builder,
            GenerateStates: true,
            GenerateContext: true
        );

        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }

    [Fact]
    public Task BuilderTestsStateContextGenerateWithInterface()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var scenarios = ScenarioFinder.FindScenarios(stateMachine);
        var settings = new GenerationSettings(
            Language,
            Mode.Builder,
            GenerateStates: true,
            GenerateContext: true,
            GenerateInterface: true
        );

        var result = Adapter.GenerateTests(stateMachine, settings, scenarios);
        return Verify(result).UseDirectory(Language.ToString());
    }
}