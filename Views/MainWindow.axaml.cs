using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using Avalonia.Layout;
using clypsy.Views;

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

        this.Width = 1152;
        this.Height = 648;
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        // Create the SignInView instance
        var signInView = new SignInView();

        // Subscribe to the LoginSuccess event
        signInView.LoginSuccess += OnLoginSuccess;

        // Set the SignInView as the content of the main window or show it
        MainContentControl.Content = signInView;
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

    // Call this function when the user is successfully logged in
    public void OnLoginSuccess()
    {
        // Switch to the main content (Welcome message)
        MainContentControl.Content = new UserControl
        {
            Content = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Welcome to Clypsy!"
            }
        };
    }
}