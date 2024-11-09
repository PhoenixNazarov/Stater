using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Stater.ViewModels.Editors;

public class StateEditorViewModel : ReactiveObject
{
    StateEditorViewModel()
    {
        Console.WriteLine("INIT");
    }
    [Reactive] public string Name { get; private set; }
    [Reactive] public string Description { get; private set; }
    [Reactive] public string Type { get; private set; }
}