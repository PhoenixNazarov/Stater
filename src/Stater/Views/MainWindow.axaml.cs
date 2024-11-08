using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace Stater.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private const double ScaleFactor = 1.01;
    private double scale = 1;

    private const double TranslateXFactor = 1;
    private double x;
    private double _x;

    private const double TranslateYFactor = 1;
    private double y;
    private double _y;

    private bool pressed;

    private void updateCanvasMatrix(Canvas canvas)
    {
        canvas.RenderTransform =
            new MatrixTransform(Matrix.CreateScale(scale, scale) * Matrix.CreateTranslation(x, y));
    }

    private void InputElement_OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (sender == null) return;
        if (e.Delta.Y >= 0)
        {
            scale *= ScaleFactor;
        }
        else
        {
            scale /= ScaleFactor;
        }

        updateCanvasMatrix((Canvas)sender);
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        pressed = true;
        var point = e.GetCurrentPoint(sender as Control);
        _x = point.Position.X;
        _y = point.Position.Y;
    }

    private void InputElement_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!pressed || sender == null) return;
        var point = e.GetCurrentPoint(sender as Control);
        var positionX = point.Position.X;
        var positionY = point.Position.Y;
        x -= (_x - positionX) * TranslateXFactor;
        y -= (_y - positionY) * TranslateYFactor;
        updateCanvasMatrix((Canvas)sender);
    }

    private void InputElement_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        pressed = false;
    }
}