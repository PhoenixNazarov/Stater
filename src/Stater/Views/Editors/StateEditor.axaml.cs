using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
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
        var editorManage = Locator.Current.GetService<IEditorManager>();
        DataContext = new StateEditorViewModel(
            stateEditor!,
            projectManager!,
            editorManage!
        );
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var transition = (AssociateTransition)button.DataContext;
        var context = (StateEditorViewModel)DataContext;
        context?.RemoveTransitionCommand.Execute(transition.Transition).Subscribe();
    }
}