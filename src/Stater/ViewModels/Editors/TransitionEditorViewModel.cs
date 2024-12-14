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
                try
                {
                    Transition = x;
                    Name = x.Name;

                    var condition = x.Condition;
                    Condition = null;
                    Variable = null;
                    Value = null;

                    VariableMath = null;
                    MathType = null;
                    EventValue = null;

                    if (condition is Condition.VariableCondition)
                    {
                        var t = (Condition.VariableCondition)condition;
                        Condition = t.ConditionType.ToString();
                        Variable = t.VariableGuid.ToString();
                        Value = t.Value.ToString();
                    }

                    var event_ = x.Event;

                    if (event_ is Event.VariableMath)
                    {
                        var t = (Event.VariableMath)event_;
                        VariableMath = t.VariableGuid.ToString();
                        MathType = t.MathType.ToString();
                        EventValue = t.Value.ToString();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
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


    [Reactive] public string VariableMath { get; set; }
    [Reactive] public string MathType { get; set; }
    [Reactive] public string EventValue { get; set; }

    public ICommand SaveCommand { get; }

    private void Save()
    {
        if (Transition == null) return;
        Condition? condition = null;
        Event? @event = null;

        if (Variable != null && Value != null)
        {
            var type = Condition switch
            {
                "<" => Models.Condition.VariableCondition.ConditionTypeEnum.Lt,
                "<=" => Models.Condition.VariableCondition.ConditionTypeEnum.Le,
                "==" => Models.Condition.VariableCondition.ConditionTypeEnum.Eq,
                "!=" => Models.Condition.VariableCondition.ConditionTypeEnum.Ne,
                ">" => Models.Condition.VariableCondition.ConditionTypeEnum.Gt,
                _ => Models.Condition.VariableCondition.ConditionTypeEnum.Ge,
            };
            condition = new Condition.VariableCondition(
                VariableGuid: Guid.Parse(Variable),
                ConditionType: type,
                Value: VariableValueBuilder.fromString(Value)
            );
        }

        if (VariableMath != null && EventValue != null)
        {
            var type = MathType switch
            {
                "+" => Event.VariableMath.MathTypeEnum.Sum,
                "-" => Event.VariableMath.MathTypeEnum.Sub,
                "*" => Event.VariableMath.MathTypeEnum.Mul,
                _ => Event.VariableMath.MathTypeEnum.Div,
            };
            @event = new Event.VariableMath(
                VariableGuid: Guid.Parse(VariableMath),
                MathType: type,
                Value: VariableValueBuilder.fromString(EventValue)
            );
        }


        var newTransition = Transition with { Name = Name, Condition = condition, Event = @event };
        _transitionEditor.Update(newTransition);
    }
}