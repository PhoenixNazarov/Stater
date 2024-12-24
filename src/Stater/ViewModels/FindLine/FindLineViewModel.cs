using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stater.Models;
using Stater.Models.Editors;
using Stater.Utils;

namespace Stater.ViewModels.FindLine;

public class FindLineViewModel : ReactiveObject
{
    private class SearchConteiner
    {
        public EditorTypeEnum EditorType;
        public int StartPos;
        public int EndPos;
        public Guid Guid { get; set; }
        public Guid StateMachineGuid;
    }

    public FindLineViewModel(IProjectManager projectManager, IEditorManager editorManager)
    {
        _projectManager = projectManager;
        _projectManager
            .IsVisibleFindLine
            .Subscribe(x => IsVisible = x);
        _editorManager = editorManager;
        IsVisible = true;
        SearchCommand = ReactiveCommand.Create(Search);
        NextCommand = ReactiveCommand.Create(Next);
        PrevCommand = ReactiveCommand.Create(Prev);
    }

    private IProjectManager _projectManager;
    private IEditorManager _editorManager;

    [Reactive] public string SearchText { get; set; } = "";

    private string PriviousSearchText { get; set; } = "";

    [Reactive] public bool IsVisible { get; set; }

    private List<SearchConteiner> _searchConteiners = [];

    private int pos;

    public ICommand SearchCommand { get; set; }
    public ICommand NextCommand { get; set; }
    public ICommand PrevCommand { get; set; }

    private void Search()
    {
        if (string.IsNullOrEmpty(SearchText) || SearchText == PriviousSearchText) return;
        PriviousSearchText = SearchText;
        _searchConteiners = [];
        foreach (var stateMachine in _projectManager.GetStateMachines())
        {
            AddConteiners(stateMachine);
        }

        ShowCurConteiner();
    }

    private void AddOneInConteiner(EditorTypeEnum editorType, Guid stateMachineGuid, Guid guid, List<List<int>> pairs)
    {
        foreach (var pair in pairs)
        {
            _searchConteiners.Add(new()
            {
                EditorType = editorType,
                StartPos = pair[0],
                EndPos = pair[1],
                Guid = guid,
                StateMachineGuid = stateMachineGuid
            });
        }
    }
    
    private void AddConteiners(StateMachine stateMachine)
    {
        var stateMachinePairs = FindUtils.FindAllSubstings(stateMachine.Name, PriviousSearchText);
        AddOneInConteiner(EditorTypeEnum.StateMachine, stateMachine.Guid, stateMachine.Guid, stateMachinePairs);
        foreach (var state in stateMachine.States)
        {
            var statePair = FindUtils.FindAllSubstings(state.Name, PriviousSearchText);
            AddOneInConteiner(EditorTypeEnum.State, stateMachine.Guid, state.Guid, statePair);
            statePair = FindUtils.FindAllSubstings(state.Description, PriviousSearchText);
            AddOneInConteiner(EditorTypeEnum.State, stateMachine.Guid, state.Guid, statePair);
        }

        foreach (var transition in stateMachine.Transitions)
        {
            var transitionPair = FindUtils.FindAllSubstings(transition.Name, PriviousSearchText);
            AddOneInConteiner(EditorTypeEnum.Transition, stateMachine.Guid, transition.Guid, transitionPair);
        }

        foreach (var variable in stateMachine.Variables)
        {
            var variablePair = FindUtils.FindAllSubstings(variable.Name, PriviousSearchText);
            AddOneInConteiner(EditorTypeEnum.Variable, stateMachine.Guid, variable.Guid, variablePair);
        }
    }

    private void Next()
    {
        if (_searchConteiners.Count == 0) return;
        if (pos >= _searchConteiners.Count - 1) pos = 0;
        else pos += 1;
        ShowCurConteiner();
    }

    private void Prev()
    {
        if (_searchConteiners.Count == 0) return;
        if (pos <= 0) pos = _searchConteiners.Count - 1;
        else pos -= 1;
        ShowCurConteiner();
    }

    private void ShowCurConteiner()
    {
        if (_searchConteiners.Count == 0)
        {
            _editorManager.DoSelectNull();
            return;
        }

        var curConteiner = _searchConteiners[pos];
        switch (curConteiner.EditorType)
        {
            case EditorTypeEnum.StateMachine:
            {
                var stateMachine = _projectManager.GetStateMachineByGuid(curConteiner.StateMachineGuid);
                if (stateMachine == null) return;
                _editorManager.DoSelectStateMachine(stateMachine);
                break;
            }
            case EditorTypeEnum.State:
            {
                var state = _projectManager.GetStateMachineByGuid(curConteiner.StateMachineGuid)?
                    .GetStateByGuid(curConteiner.Guid);
                if (state == null) return;
                _editorManager.DoSelectState(state);
                break;
            }
            case EditorTypeEnum.Variable:
            {
                var variable = _projectManager.GetStateMachineByGuid(curConteiner.StateMachineGuid)?
                    .GetVariableByGuid(curConteiner.Guid);
                if (variable == null) return;
                _editorManager.DoSelectVariable(variable);
                break;
            }
            case EditorTypeEnum.Transition:
            {
                var transition = _projectManager.GetStateMachineByGuid(curConteiner.StateMachineGuid)?
                    .GetTransitionByGuid(curConteiner.Guid);
                if (transition == null) return;
                _editorManager.DoSelectTransition(transition);
                break;
            }
            case EditorTypeEnum.Null:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}