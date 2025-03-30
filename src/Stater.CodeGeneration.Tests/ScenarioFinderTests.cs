using Stater.CodeGeneration.Tests.LanguageAdapter;
using Xunit;
using Xunit.Abstractions;

namespace Stater.CodeGeneration.Tests;

public class ScenarioFinderTests
{
    [Fact]
    public void ClazzGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var results = ScenarioFinder.FindScenarios(stateMachine);
        
        Console.WriteLine(results.ToString());
    }
}