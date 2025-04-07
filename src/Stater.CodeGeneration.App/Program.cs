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

if (!Directory.Exists(outputPath))
{
    Directory.CreateDirectory(outputPath);
    Console.WriteLine($"Folder created: {outputPath}");
}

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

                var path = outputPath;
                var testPath = outputPath;
                var languageS = Language.Python3;
                switch (language)
                {
                    case "python3":
                        path += randomStateMachine.Name + ".py";
                        testPath += "test_" + randomStateMachine.Name + ".py";
                        languageS = Language.Python3;
                        break;
                    case "javascript":
                        path += randomStateMachine.Name + ".js";
                        testPath += randomStateMachine.Name + ".test.js";
                        languageS = Language.JavaScript;
                        break;
                    case "typescript":
                        path += randomStateMachine.Name + ".ts";
                        testPath += randomStateMachine.Name + ".test.ts";
                        languageS = Language.TypeScript;
                        break;
                    case "kotlin":
                        path += "main/kotlin/fsm/" + randomStateMachine.Name.ToLower() + "/";
                        testPath += "test/kotlin/fsm/" + randomStateMachine.Name.ToLower() + "/";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                            Console.WriteLine($"Folder created: {path}");
                        }
                        if (!Directory.Exists(testPath))
                        {
                            Directory.CreateDirectory(testPath);
                            Console.WriteLine($"Folder created: {testPath}");
                        }
                        path += randomStateMachine.Name + ".kt";
                        testPath += "Test" + randomStateMachine.Name + ".kt";
                        
                        languageS = Language.Kotlin;
                        break;
                    case "java":
                        path += "main/java/fsm/" + randomStateMachine.Name.ToLower() + "/";
                        testPath += "test/java/fsm/" + randomStateMachine.Name.ToLower() + "/";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                            Console.WriteLine($"Folder created: {path}");
                        }
                        if (!Directory.Exists(testPath))
                        {
                            Directory.CreateDirectory(testPath);
                            Console.WriteLine($"Folder created: {testPath}");
                        }
                        path += randomStateMachine.Name + ".java";
                        testPath += "Test" + randomStateMachine.Name + ".java";
                        
                        languageS = Language.Java;
                        break;
                    case "csharp":
                        path += "/Stater.StateMachine.Lib/fsm/" + randomStateMachine.Name.ToLower() + "/";
                        testPath += "/Stater.StateMachine.Lib.Tests/fsm/" + randomStateMachine.Name.ToLower() + "/";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                            Console.WriteLine($"Folder created: {path}");
                        }
                        if (!Directory.Exists(testPath))
                        {
                            Directory.CreateDirectory(testPath);
                            Console.WriteLine($"Folder created: {testPath}");
                        }
                        path += randomStateMachine.Name + ".cs";
                        testPath += "Test" + randomStateMachine.Name + ".cs";
                        
                        languageS = Language.CSharp;
                        break;
                    case "cplusplus":
                        path += "fsm/" + randomStateMachine.Name.ToLower() + "/";
                        testPath += "tests/";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                            Console.WriteLine($"Folder created: {path}");
                        }
                        if (!Directory.Exists(testPath))
                        {
                            Directory.CreateDirectory(testPath);
                            Console.WriteLine($"Folder created: {testPath}");
                        }
                        path += randomStateMachine.Name + ".h";
                        testPath += "test_" + randomStateMachine.Name + ".cpp";
                        
                        languageS = Language.CPlusPlus;
                        break;
                }

                var settings = new GenerationSettings(
                    languageS
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