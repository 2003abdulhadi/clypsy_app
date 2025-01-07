using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;

namespace clypsy;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Extend the client area to include custom decorations
        ExtendClientAreaToDecorationsHint = true;
        ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
        ExtendClientAreaTitleBarHeightHint = -1;
    }
    private void OnMinimizeClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }

    private void OnMaximizeRestoreClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (this.WindowState == WindowState.Maximized)
        {
            this.WindowState = WindowState.Normal;
        }
        else
        {
            this.WindowState = WindowState.Maximized;
        }
    }

    private void OnCloseClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }
    private void OnTitleBarPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            this.BeginMoveDrag(e);
        }
    }
    private void OnTitleBarDoubleTapped(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (this.WindowState == WindowState.Maximized)
        {
            this.WindowState = WindowState.Normal;
        }
        else
        {
            this.WindowState = WindowState.Maximized;
        }
    }
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        if (!OperatingSystem.IsWindows()) return;

        var style = new Style();
        style.Selector = Selectors.Is<Window>(null)
          .PropertyEquals(WindowStateProperty, WindowState.Maximized);

        var setter = new Setter();
        setter.Property = PaddingProperty;
        setter.Value = Thickness.Parse("8");
        style.Add(setter);
        Styles.Add(style);
    }
}