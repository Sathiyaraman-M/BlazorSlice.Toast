using System.ComponentModel;

namespace BlazorSlice.Toast;

public enum Color : byte
{
    [Description("primary")]
    Primary,
    [Description("secondary")]
    Secondary,
    [Description("success")]
    Success,
    [Description("warning")]
    Warning,
    [Description("danger")]
    Danger,
    [Description("info")]
    Info,
    [Description("dark")]
    Dark,
    [Description("light")]
    Light,
    [Description("azure")]
    Azure,
    [Description("blue")]
    Blue,
    [Description("indigo")]
    Indigo,
    [Description("purple")]
    Purple,
    [Description("pink")]
    Pink,
    [Description("red")]
    Red,
    [Description("orange")]
    Orange,
    [Description("yellow")]
    Yellow,
    [Description("lime")]
    Lime,
    [Description("green")]
    Green,
    [Description("teal")]
    Teal,
    [Description("cyan")]
    Cyan
}