// See https://aka.ms/new-console-template for more information


using Stater.CodeGeneration;
using Stater.CodeGeneration.App;
using Stater.CodeGeneration.Entity;


if (args.Length < 3)
{
    Console.WriteLine("Please write: [output-path] [seed] [language]");
}

var outputPath = args[0];
var seed = int.Parse(args[1]);
var language = args[2];

var randomStateMachineGenerator = new RandomStateMachineGenerator(seed);

var states = new List<bool> { false, true };
var codeGenerator = new CodeGenerator();

foreach (var generateMode in new List<Mode> { Mode.Builder, Mode.Clazz })
{
    foreach (var generateStates in states)
    {
        foreach (var generateContext in states)
        {
            foreach (var generateInterface in states)
            {
                var randomStateMachine = randomStateMachineGenerator.GenerateStateMachine(5, 10, 10, 10, 10);

                var path = outputPath + randomStateMachine.Name + ".py";
                var testPath = outputPath + "test_" + randomStateMachine.Name + ".py";

                var settings = new GenerationSettings(
                    Language.Python3
                    , generateMode
                    , GenerateStates: generateStates
                    , GenerateContext: generateContext
                    , GenerateInterface: generateInterface
                );

                using var sw = new StreamWriter(path);
                var result = codeGenerator.Generate(randomStateMachine, settings);
                sw.WriteLine(result);
                
                
                using var swTest = new StreamWriter(testPath);
                var results = ScenarioFinder.FindScenarios(randomStateMachine);
                var resultTest = codeGenerator.GenerateTests(randomStateMachine, settings, results);
                swTest.WriteLine(resultTest);
            }
        }
    }
}