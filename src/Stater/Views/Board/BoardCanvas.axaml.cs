using System;
using System.Numerics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Stater.Models;
using Stater.ViewModels;

namespace Stater.Views.Board;

public partial class BoardCanvas : UserControl
{
    public BoardCanvas()
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

    private const double StateTranslateXFactor = 1;
    private double stateX;
    private double _stateX;

    private const double StateTranslateYFactor = 1;
    private double stateY;
    private double _stateY;
    private bool statePressed;

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
        if (statePressed) return;
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

    private void State_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var rectangle = (Rectangle)sender;
        if (rectangle?.DataContext is State state)
        {
            var context = (MainWindowViewModel)DataContext;
            context?.StateClickCommand.Execute(state).Subscribe();
            statePressed = true;

            var canvas = TheCanvasPanel;
            var point = e.GetCurrentPoint(canvas);
            stateX = 0;
            stateY = 0;
            _stateX = point.Position.X;
            _stateY = point.Position.Y;
        }
    }

    private void State_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!statePressed || sender == null) return;
        var rectangle = (Rectangle)sender!;
        var canvas = TheCanvasPanel;
        var point = e.GetCurrentPoint(canvas);
        var positionX = point.Position.X;
        var positionY = point.Position.Y;
        stateX = (positionX - _stateX) * StateTranslateXFactor / scale;
        stateY = (positionY - _stateY) * StateTranslateYFactor / scale;
        rectangle.RenderTransform =
            new MatrixTransform(Matrix.CreateTranslation(stateX, stateY));
    }

    private void State_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        statePressed = false;
        var context = (MainWindowViewModel)DataContext;
        context?.UpdateStateCoordsCommand.Execute(new Vector2((float)stateX, (float)stateY)).Subscribe();
    }
}