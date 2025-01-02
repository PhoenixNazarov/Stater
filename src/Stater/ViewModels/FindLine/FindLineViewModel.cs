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
using Stater.Models.FindLine;
using Stater.Utils;

namespace Stater.ViewModels.FindLine;

public class FindLineViewModel : ReactiveObject
{
    public FindLineViewModel(IProjectManager projectManager, IEditorManager editorManager)
    {
        _projectManager = projectManager;
        AllPos = 0;
        SearchPos = 0;
        _projectManager
            .IsVisibleFindLine
            .Subscribe(x =>
            {
                IsVisible = x;
                if (!x)
                {
                    _editorManager.DoUnLoadSearch();
                }
            });
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

    [Reactive] public int AllPos { get; set; }
    [Reactive] public int SearchPos { get; set; }

    private int pos;

    public ICommand SearchCommand { get; set; }
    public ICommand NextCommand { get; set; }
    public ICommand PrevCommand { get; set; }

    private void Search()
    {
        if (string.IsNullOrEmpty(SearchText)) return;
        PriviousSearchText = SearchText;
        _searchConteiners = [];
        foreach (var stateMachine in _projectManager.GetStateMachines())
        {
            AddConteiners(stateMachine);
        }
        if(_searchConteiners.Count == 0) return;
        pos = 0;
        SearchPos = 1;
        AllPos = _searchConteiners.Count;
        LoadConteiners();
        ShowCurConteiner();
    }

    private void LoadConteiners()
    {
        _editorManager.DoLoadSearch(_searchConteiners);
    }

    private void AddOneInConteiner(EditorTypeEnum editorType, Guid stateMachineGuid, Guid guid, List<List<int>> pairs,
        bool isDescription = false)
    {
        foreach (var pair in pairs)
        {
            _searchConteiners.Add(new()
            {
                EditorType = editorType,
                StartPos = pair[0],
                EndPos = pair[1],
                Guid = guid,
                IsDescription = isDescription,
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
            AddOneInConteiner(EditorTypeEnum.State, stateMachine.Guid, state.Guid, statePair, true);
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
        SearchPos = pos + 1;
        ShowCurConteiner();
    }

    private void Prev()
    {
        if (_searchConteiners.Count == 0) return;
        if (pos <= 0) pos = _searchConteiners.Count - 1;
        else pos -= 1;
        SearchPos = pos + 1;
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
                _editorManager
                    .DoSelectSubstringStateMachine(stateMachine, curConteiner.StartPos, curConteiner.EndPos);
                break;
            }
            case EditorTypeEnum.State:
            {
                var state = _projectManager.GetStateMachineByGuid(curConteiner.StateMachineGuid)?
                    .GetStateByGuid(curConteiner.Guid);
                if (state == null) return;
                _editorManager
                    .DoSelectSubstringState(state, curConteiner.StartPos, curConteiner.EndPos,
                    curConteiner.IsDescription);
                break;
            }
            case EditorTypeEnum.Variable:
            {
                var variable = _projectManager.GetStateMachineByGuid(curConteiner.StateMachineGuid)?
                    .GetVariableByGuid(curConteiner.Guid);
                if (variable == null) return;
                _editorManager.DoSelectSubstringVariable(variable, curConteiner.StartPos, curConteiner.EndPos);
                break;
            }
            case EditorTypeEnum.Transition:
            {
                var transition = _projectManager.GetStateMachineByGuid(curConteiner.StateMachineGuid)?
                    .GetTransitionByGuid(curConteiner.Guid);
                if (transition == null) return;
                _editorManager.DoSelectSubstringTransition(transition, curConteiner.StartPos, curConteiner.EndPos);
                break;
            }
            case EditorTypeEnum.Null:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}