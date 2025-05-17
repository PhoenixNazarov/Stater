using Avalonia.Controls;
using Splat;
using Stater.Models.Editors;
using Stater.ViewModels.Editors;

namespace Stater.Views.Editors;

public partial class StateMachineEditor : UserControl
{
    public StateMachineEditor()
    {
        InitializeComponent();
        var stateMachineEditor = Locator.Current.GetService<IStateMachineEditor>();
        DataContext = new StateMachineEditorViewModel(
            stateMachineEditor!
        );
    }
}