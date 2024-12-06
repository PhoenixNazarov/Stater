using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stater.Models;
using Stater.Models.Editors;

namespace Stater.ViewModels.Editors;

public class TransitionEditorViewModel : ReactiveObject
{
    public TransitionEditorViewModel(ITransitionEditor transitionEditor, IProjectManager projectManager)
    {
        _transitionEditor = transitionEditor;
        _projectManager = projectManager;

        SaveCommand = ReactiveCommand.Create(Save);

        _transitionEditor
            .Transition
            .Subscribe(x =>
            {
                Transition = x;
                Name = x.Name;

                var condition = x.Condition;

                if (condition is Condition.VariableCondition)
                {
                    var t = (Condition.VariableCondition)condition;
                    Condition = t.ConditionType.ToString();
                    Variable = t.VariableGuid.ToString();
                    Value = t.Value.ToString();
                }
            });

        projectManager
            .StateMachine
            .Subscribe(x => { Variables = x.Variables.ConvertAll(y => y.Guid.ToString()); }
            );
    }

    private ITransitionEditor _transitionEditor;
    private IProjectManager _projectManager;
    [Reactive] private Transition? Transition { get; set; }

    [Reactive] public string Name { get; set; }

    [Reactive] public List<string> Variables { get; set; }
    [Reactive] public string Variable { get; set; }
    [Reactive] public string Condition { get; set; }
    [Reactive] public string Value { get; set; }

    public ICommand SaveCommand { get; }

    private void Save()
    {
        if (Transition == null) return;
        Condition? condition = null;
        var tryParse = Enum.TryParse(Condition, out Condition.VariableCondition.ConditionTypeEnum conditionType);
        if (!tryParse)
        {
            condition = new Condition.VariableCondition(
                VariableGuid: Guid.Parse(Variable),
                ConditionType: conditionType,
                Value: VariableValueBuilder.fromString(Value)
            );
        }
        
        var newTransition = Transition with { Name = Name, Condition = condition };
        _transitionEditor.Update(newTransition);
    }
}