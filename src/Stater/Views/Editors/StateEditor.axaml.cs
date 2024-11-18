using System;
using Avalonia.Controls;
using Avalonia.Input;
using Splat;
using Stater.Models;
using Stater.Models.Editors;
using Stater.ViewModels.Editors;

namespace Stater.Views.Editors;

public partial class StateEditor : UserControl
{
    public StateEditor()
    {
        InitializeComponent();
        var stateEditor = Locator.Current.GetService<IStateEditor>();
        var projectManager = Locator.Current.GetService<IProjectManager>();
        DataContext = new StateEditorViewModel(
            stateEditor!,
            projectManager!
        );
    }
}