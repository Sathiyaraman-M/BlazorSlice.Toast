using System.ComponentModel;

namespace BlazorSlice.Toast.Enums;

public enum ToastPosition
{
    [Description("top-0 start-0")]
    TopLeft,
    [Description("top-0 start-50 translate-middle-x")]
    TopCenter,
    [Description("top-0 end-0")]
    TopRight,
    [Description("top-50 start-0 translate-middle-y")]
    MiddleLeft,
    [Description("top-50 start-50 translate-middle")]
    MiddleCenter,
    [Description("top-50 end-0 translate-middle-y")]
    MiddleRight,
    [Description("bottom-0 start-0")]
    BottomLeft,
    [Description("bottom-0 start-50 translate-middle-x")]
    BottomCenter,
    [Description("bottom-0 end-0")]
    BottomRight
}