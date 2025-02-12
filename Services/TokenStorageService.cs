using System;
using System.IO;
using clypsy.Common;
using Microsoft.AspNetCore.DataProtection;
public sealed class TokenStorageService
{
    public static TokenStorageService Instance => lazyInstance.Value;
    private static readonly Lazy<TokenStorageService> lazyInstance = new(() => new());
    private readonly IDataProtector _protector;
    private readonly string _filePath;
    private TokenStorageService()
    {
        var provider = DataProtectionProvider.Create(Globals.projectName);
        _protector = provider.CreateProtector("RefreshToken");

        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var folder = Path.Combine(appData, Globals.projectName);
        Directory.CreateDirectory(folder);
        _filePath = Path.Combine(folder, "refreshToken.dat");
    }

    public void SaveRefreshToken(string token)
    {
        var encrypted = _protector.Protect(token);
        File.WriteAllText(_filePath, encrypted);
    }
}