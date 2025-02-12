using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.Input;
using clypsy.Common;
using System.Net.Http.Json;

namespace clypsy.Views;

public partial class SignInView : UserControl
{
    private static readonly HttpClient _httpClient = new HttpClient();

    // Define the LoginSuccess event that MainWindow will listen for
    public event Action? LoginSuccess;
    public SignInView()
    {
        InitializeComponent();
    }

    private async void OnSignInClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Retrieve input values
        var email = EmailTextBox.Text?.Trim();
        var password = PasswordTextBox.Text;

        // Validate input
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowErrorMessage("Please enter both email and password.");
            return;
        }

        try
        {
            // Create payload
            var payload = new { email, password };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send request to server
            var response = await _httpClient.PostAsync($"{Globals.apiBaseUrl}/auth/signin", content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // // Parse token from response
                var responseBody = await response.Content.ReadFromJsonAsync<AuthResponse>() ?? throw new InvalidOperationException("Auth response was null");
                TokenStorageService.Instance.SaveRefreshToken(responseBody.RefreshToken!);
                responseBody = null;
                // Notify MainWindow that login was successful
                LoginSuccess?.Invoke();
            }
            else
            {
                ShowErrorMessage("Incorrect email or password.");
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage("An error occurred. Please try again.");
            Console.WriteLine(ex);
        }
    }

    private void ShowErrorMessage(string message)
    {
        ErrorMessageTextBlock.Text = message;
        ErrorMessageTextBlock.IsVisible = true;

        // Hide error message after 5 seconds
        Task.Delay(5000).ContinueWith(_ =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                ErrorMessageTextBlock.IsVisible = false;
            });
        });
    }

    private void StoreToken(string? token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            // For secure storage, use platform-specific solutions.
            // For example:
            // - Windows: Windows.Security.Credentials.PasswordVault
            // - macOS/Linux: Keyring or Secure Storage

            Console.WriteLine("Token stored successfully!");
        }
    }

    private void OnHyperlinkClick(object? sender, PointerPressedEventArgs e)
    {
        // Open the Forgot Password page in the default browser
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = "https://clypsy.ai/signin/forgot",
            UseShellExecute = true
        });
    }

    private class AuthResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
