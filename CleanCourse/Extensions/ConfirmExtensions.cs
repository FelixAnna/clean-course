using Microsoft.JSInterop;

namespace CleanCourse.Extensions;

public static class ConfirmExtensions
{
    public static ValueTask<bool> Confirm(this IJSRuntime jsRuntime, string message)
    {
        return jsRuntime.InvokeAsync<bool>("confirm", message);
    }
    public static ValueTask<bool> Alert(this IJSRuntime jsRuntime, string message)
    {
        return jsRuntime.InvokeAsync<bool>("alert", message);
    }
}
