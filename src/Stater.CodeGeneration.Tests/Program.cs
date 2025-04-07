using Stater.CodeGeneration.Entity;
using Stater.CodeGeneration.Tests.LanguageAdapter;

namespace Stater.CodeGeneration.Tests;

public class Program
{
    public static void main()
    {
        // matrixGenerate();
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        var results = ScenarioFinder.FindScenarios(stateMachine);
        Console.WriteLine(results[2].Count);
        Console.WriteLine(results[2][0]);
        Console.WriteLine(results[2][1]);
        Console.WriteLine(results[2][2]);
        Console.WriteLine(results[2][3]);
    }

    static void simpleOutput()
    {
        var codeGenerator = new CodeGenerator();
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        
        var settings = new GenerationSettings(
            Language.Kotlin
            , Mode.Builder
            , GenerateStates: true
            , GenerateContext: true
            , GenerateInterface:true
        );
        
        var basePath = "/Users/vnazarov/IdeaProjects/stater-fsm-code/projects/kotlin/src/main/kotlin";
        using var sw = new StreamWriter(basePath + "/" + ".kt");
        var result = codeGenerator.Generate(stateMachine, settings);
        Console.WriteLine(result);
    }

    static void matrixGenerate()
    {
        var stateMachine = DoorStateMachineTestData.CreateDoorStateMachine();
        
        var states =  new List<bool>{false, true};
        var codeGenerator = new CodeGenerator();
 
        foreach (var generateStates in states)
        {
            foreach (var generateContext in states)
            {
                foreach (var generateInterface in states)
                {
                    var basePath = "/Users/vnazarov/IdeaProjects/stater-fsm-code/projects/python/stater_state_machine/door_class";
                    if (generateStates)
                    {
                        basePath += "_states";
                    }
                    if (generateContext)
                    {
                        basePath += "_context";
                    }
                    if (generateInterface)
                    {
                        basePath += "_interface";
                    }
                    
                    var settings = new GenerationSettings(
                        Language.Python3
                        , Mode.Builder
                        , GenerateStates: generateStates
                        , GenerateContext: generateContext
                        , GenerateInterface:generateInterface
                    );

                    basePath += ".py";
                    using var sw = new StreamWriter(basePath);
                    var result = codeGenerator.Generate(stateMachine, settings);
                    sw.WriteLine(result);
                }
            }
        }
    }
}