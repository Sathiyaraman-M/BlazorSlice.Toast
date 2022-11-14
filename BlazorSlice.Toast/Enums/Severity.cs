using System.ComponentModel;

namespace BlazorSlice.Toast;

public enum Severity
{
    [Description("normal")]
    Normal,
    [Description("info")]
    Info,
    [Description("success")]
    Success,
    [Description("warning")]
    Warning,
    [Description("error")]
    Error
}