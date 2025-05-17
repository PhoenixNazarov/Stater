using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Splat;
using Stater.Models;
using Stater.ViewModels.Plugins;
using Avalonia;
using Avalonia.VisualTree;

namespace Stater.Views.Plugins;

public partial class CodeGeneration : UserControl
{
    public CodeGeneration()
    {
        InitializeComponent();
        var projectManager = Locator.Current.GetService<IProjectManager>();
        DataContext = new CodeGenerationViewModel(
            projectManager!
        );
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = this.GetVisualRoot() as TopLevel;
        var files = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Open Folder",
            AllowMultiple = false,
        });

        if (files.Count < 1) return;

        var context = (CodeGenerationViewModel)DataContext;
        Console.WriteLine(files[0].Path.ToString());
        context?.GenerateCommand.Execute(
            files[0].Path.ToString().Replace("file:/", "")
        ).Subscribe();
    }
}