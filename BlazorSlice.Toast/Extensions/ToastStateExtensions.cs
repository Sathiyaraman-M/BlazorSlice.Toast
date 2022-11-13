namespace BlazorSlice.Toast.Extensions;

internal static class ToastStateExtensions
{
    public static bool IsShowing(this ToastState state) => state == ToastState.Showing;
    public static bool IsVisible(this ToastState state) => state == ToastState.Visible;
    public static bool IsHiding(this ToastState state) => state == ToastState.Hiding;
}