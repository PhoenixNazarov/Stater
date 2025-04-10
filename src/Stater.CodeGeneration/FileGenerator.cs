using Stater.CodeGeneration.Entity;
using Stater.Domain.Models;

namespace Stater.CodeGeneration;

public class FileGenerator
{
    private CodeGenerator codeGenerator = new();


    public void GenerateCode(
        Language language,
        StateMachine stateMachine,
        string outputPath,
        Mode generateMode,
        bool generateStates,
        bool generateContext,
        bool generateInterface)
    {
        var path = outputPath;
        var testPath = outputPath;
        if (!outputPath.EndsWith('/'))
        {
            path += "/";
            testPath += "/";
        }

        switch (language)
        {
            case Language.Python3:
                path += stateMachine.Name + ".py";
                testPath += "test_" + stateMachine.Name + ".py";
                break;
            case Language.JavaScript:
                path += stateMachine.Name + ".js";
                testPath += stateMachine.Name + ".test.js";
                break;
            case Language.TypeScript:
                path += stateMachine.Name + ".ts";
                testPath += stateMachine.Name + ".test.ts";
                break;
            case Language.Kotlin:
                path += "main/kotlin/fsm/" + stateMachine.Name.ToLower() + "/";
                testPath += "test/kotlin/fsm/" + stateMachine.Name.ToLower() + "/";
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

                path += stateMachine.Name + ".kt";
                testPath += "Test" + stateMachine.Name + ".kt";

                break;
            case Language.Java:
                path += "main/java/fsm/" + stateMachine.Name.ToLower() + "/";
                testPath += "test/java/fsm/" + stateMachine.Name.ToLower() + "/";
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

                path += stateMachine.Name + ".java";
                testPath += "Test" + stateMachine.Name + ".java";

                break;
            case Language.CSharp:
                path += "/Stater.StateMachine.Lib/fsm/" + stateMachine.Name.ToLower() + "/";
                testPath += "/Stater.StateMachine.Lib.Tests/fsm/" + stateMachine.Name.ToLower() + "/";
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

                path += stateMachine.Name + ".cs";
                testPath += "Test" + stateMachine.Name + ".cs";

                break;
            case Language.CPlusPlus:
                path += "fsm/" + stateMachine.Name + "/";
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

                path += stateMachine.Name + ".h";
                testPath += "test_" + stateMachine.Name + ".cpp";

                break;
        }

        var settings = new GenerationSettings(
            language
            , generateMode
            , GenerateStates: generateStates
            , GenerateContext: generateContext
            , GenerateInterface: generateInterface
        );

        var newStateMachine = stateMachine with { Name = stateMachine.Name.Replace(" ", "_") };

        using var sw = new StreamWriter(path);
        var result = codeGenerator.Generate(newStateMachine, settings);
        sw.WriteLine(result);


        using var swTest = new StreamWriter(testPath);
        var results = ScenarioFinder.FindScenarios(newStateMachine);
        var resultTest = codeGenerator.GenerateTests(newStateMachine, settings, results);
        swTest.WriteLine(resultTest);
    }
}