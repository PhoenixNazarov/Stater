using System;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stater.CodeGeneration;
using Stater.CodeGeneration.Entity;
using Stater.Domain.Models;
using Stater.Models;

namespace Stater.ViewModels.Plugins;

public class CodeGenerationViewModel : ReactiveObject
{
    public CodeGenerationViewModel(IProjectManager projectManager)
    {
        projectManager
            .StateMachine
            .Subscribe(x => { StateMachine = x; });
        GenerateCommand = ReactiveCommand.Create<string>(Generate);
    }


    [Reactive] public StateMachine? StateMachine { get; private set; }

    [Reactive] public int LanguageIndex { get; set; }
    [Reactive] public int ModeIndex { get; set; }
    [Reactive] public bool GenerateStates { get; set; }
    [Reactive] public bool GenerateContext { get; set; }
    [Reactive] public bool GenerateInterface { get; set; }

    private FileGenerator fileGenerator = new();

    public ReactiveCommand<string, Unit> GenerateCommand { get; }

    private void Generate(string outputPath)
    {
        if (StateMachine == null) return;
        var language = LanguageIndex switch
        {
            0 => Language.Java,
            1 => Language.Kotlin,
            2 => Language.JavaScript,
            3 => Language.TypeScript,
            4 => Language.Python3,
            5 => Language.CSharp,
            _ => Language.CPlusPlus
        };

        var mode = ModeIndex switch
        {
            0 => Mode.Clazz,
            _ => Mode.Builder,
        };
        fileGenerator.GenerateCode(
            language,
            StateMachine,
            outputPath,
            mode,
            GenerateStates,
            GenerateContext,
            GenerateInterface
        );
    }
}