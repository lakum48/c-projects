using Blazored.LocalStorage;

namespace ShopPKS.Client.Services;

public class ThemeService
{
    private readonly ILocalStorageService _localStorage;
    private bool _isDarkMode;

    public event Action<bool>? OnThemeChanged;

    public ThemeService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public bool IsDarkMode 
    { 
        get => _isDarkMode;
        set
        {
            if (_isDarkMode != value)
            {
                _isDarkMode = value;
                _ = _localStorage.SetItemAsync("darkMode", value);
                OnThemeChanged?.Invoke(value);
            }
        }
    }

    public async Task InitializeAsync()
    {
        try
        {
            _isDarkMode = await _localStorage.GetItemAsync<bool>("darkMode");
            OnThemeChanged?.Invoke(_isDarkMode);
        }
        catch
        {
            _isDarkMode = false;
            await _localStorage.SetItemAsync("darkMode", false);
            OnThemeChanged?.Invoke(false);
        }
    }
} 