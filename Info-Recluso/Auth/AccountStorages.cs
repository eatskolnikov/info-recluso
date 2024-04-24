using Microsoft.JSInterop;

namespace Auth;
public partial class AccountStorages
{
    private readonly IJSRuntime _js;
    public static readonly string tokenName = "SecurityToken";

    public AccountStorages(IJSRuntime jS) => (_js) = (jS);

    public async Task<string> GetToken() => await _js.InvokeAsync<string>("getCookie", $"{tokenName}");

    public async Task SetToken(string token) => await _js.InvokeVoidAsync("setCookie", $"{tokenName}", $"{token}", 365);

    public async Task ClearAll() => await SetToken("");
}