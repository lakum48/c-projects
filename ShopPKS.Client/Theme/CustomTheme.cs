using MudBlazor;

namespace ShopPKS.Client.Theme;

public static class CustomTheme
{
    public static MudTheme DefaultTheme => new MudTheme
    {
        Palette = new PaletteLight
        {
            Primary = Colors.Blue.Default,
            Secondary = Colors.Green.Accent4,
            AppbarBackground = Colors.Blue.Default,
            Background = Colors.Grey.Lighten5,
            DrawerBackground = Colors.Grey.Lighten4
        },
        PaletteDark = new PaletteDark
        {
            Primary = Colors.Blue.Lighten1,
            Secondary = Colors.Green.Accent4,
            AppbarBackground = Colors.Blue.Darken4,
            Background = Colors.Grey.Darken4,
            Surface = Colors.Grey.Darken4,
            DrawerBackground = Colors.Grey.Darken3
        }
    };
} 