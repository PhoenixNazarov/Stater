using System;
using System.IO;
using System.Numerics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Stater.Models;
using Stater.ViewModels;

namespace Stater.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private async void OpenFileButton_Clicked(object sender, RoutedEventArgs args)
    {
        var topLevel = GetTopLevel(this);
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Project",
            AllowMultiple = false
        });

        if (files.Count < 1) return;
        await using var stream = await files[0].OpenReadAsync();
        using var streamReader = new StreamReader(stream);
        var context = (MainWindowViewModel)DataContext;
        context?.OpenCommand.Execute(streamReader).Subscribe();
    }
    
    private async void SaveFileButton_Clicked(object sender, RoutedEventArgs args)
    {
        var topLevel = GetTopLevel(this);
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Project"
        });

        if (file is null) return;
        await using var stream = await file.OpenWriteAsync();
        await using var streamWriter = new StreamWriter(stream);
        var context = (MainWindowViewModel)DataContext;
        context?.SaveCommand.Execute(streamWriter).Subscribe();
    }
}