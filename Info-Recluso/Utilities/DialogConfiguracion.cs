using MudBlazor;

namespace Utilities;
public class DialogConfiguracion
{
    public static DialogOptions options1 = new()
    {
        CloseOnEscapeKey = true,
        Position = DialogPosition.Center,
        FullWidth = true,
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.Medium,
        NoHeader = true,
    };

    public static DialogOptions options2 = new()
    {
        CloseButton = true,
        CloseOnEscapeKey = true,
        Position = DialogPosition.Center,
        FullWidth = true,
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.Medium
    };

    public static DialogOptions options3 = new()
    {
        CloseButton = true,
        CloseOnEscapeKey = true,
        Position = DialogPosition.Center,
        FullWidth = true,
        DisableBackdropClick = false,
        MaxWidth = MaxWidth.Small
    };
}