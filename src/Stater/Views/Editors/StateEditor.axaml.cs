using Avalonia.Controls;
using Splat;
using Stater.Models.Editors;
using Stater.ViewModels.Editors;

namespace Stater.Views.Editors;

public partial class StateEditor : UserControl
{
    public StateEditor()
    {
        InitializeComponent();
        var stateEditor = Locator.Current.GetService<IStateEditor>();
        DataContext = new StateEditorViewModel(
            stateEditor!
        );
    }
}