using System;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stater.Domain.Models;
using Stater.Models.Editors;

namespace Stater.ViewModels.Editors;

public class StateMachineEditorViewModel : ReactiveObject
{
    public StateMachineEditorViewModel(IStateMachineEditor stateMachineEditor)
    {
        _stateMachineEditor = stateMachineEditor;

        SaveCommand = ReactiveCommand.Create(Save);

        stateMachineEditor
            .StateMachine
            .Subscribe(x =>
            {
                StateMachine = x;
                Name = x.Name;
            });
    }

    private readonly IStateMachineEditor _stateMachineEditor;

    public ICommand SaveCommand { get; }


    [Reactive] private StateMachine? StateMachine { get; set; }
    [Reactive] public string Name { get; set; }

    private void Save()
    {
        if (StateMachine == null) return;
        var newStateMachine = StateMachine with { Name = Name };
        _stateMachineEditor.Update(newStateMachine);
    }
}