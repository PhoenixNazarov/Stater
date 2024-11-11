using System;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stater.Models;
using Stater.Models.Editors;

namespace Stater.ViewModels.Editors;

public class StateEditorViewModel : ReactiveObject
{
    public StateEditorViewModel(IStateEditor stateEditor)
    {
        _stateEditor = stateEditor;

        SaveCommand = ReactiveCommand.Create(Save);

        _stateEditor
            .State
            .Subscribe(x =>
            {
                State = x;
                Name = x.Name;
                Description = x.Description;
                Type = x.Type.ToString();
            });
    }

    private readonly IStateEditor _stateEditor;

    public ICommand SaveCommand { get; }

    [Reactive] private State? State { get; set; }

    [Reactive] public string Name { get; set; }
    [Reactive] public string Description { get; set; }
    [Reactive] public string Type { get; set; }

    private void Save()
    {
        if (State == null) return;
        var tryParse = Enum.TryParse(Type, out StateType type);
        if (tryParse)
        {
            var newStateMachine = State with { Name = Name, Description = Description, Type = type };
            _stateEditor.Update(newStateMachine);
        }
    }
}