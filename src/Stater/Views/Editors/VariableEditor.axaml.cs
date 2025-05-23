using Avalonia.Controls;
using Splat;
using Stater.Models.Editors;
using Stater.ViewModels.Editors;

namespace Stater.Views.Editors;

public partial class VariableEditor : UserControl
{
    public VariableEditor()
    {
        InitializeComponent();
        var variableEditor = Locator.Current.GetService<IVariableEditor>();
        DataContext = new VariableEditorViewModel(variableEditor!);
    }
}