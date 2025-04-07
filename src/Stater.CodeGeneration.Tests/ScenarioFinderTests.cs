using Stater.CodeGeneration.Tests.LanguageAdapter;

namespace Stater.CodeGeneration.Tests;

public class ScenarioFinderTests
{
    [Fact]
    public void TestScenarioFinder()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var results = ScenarioFinder.FindScenarios(stateMachine);
        Assert.Equal(3, results.Count);
        Assert.Equal(5, results[0].Count);
        Assert.Equal("preClose", results[0][0].Name);
        Assert.Equal("close", results[0][1].Name);
        Assert.Equal("preOpen", results[0][2].Name);
        Assert.Equal("ajarPlus", results[0][3].Name);
        Assert.Equal("open", results[0][4].Name);
        
        Assert.Equal(5, results[1].Count);
        Assert.Equal("preClose", results[1][0].Name);
        Assert.Equal("ajarPlus", results[1][1].Name);
        Assert.Equal("close", results[1][2].Name);
        Assert.Equal("preOpen", results[1][3].Name);
        Assert.Equal("open", results[1][4].Name);
        
        Assert.Equal(4, results[2].Count);
        Assert.Equal("preClose", results[2][0].Name);
        Assert.Equal("close", results[2][1].Name);
        Assert.Equal("preOpen", results[2][2].Name);
        Assert.Equal("open", results[2][3].Name);
    }
}