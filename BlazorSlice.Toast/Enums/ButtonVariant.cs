using System.ComponentModel;

namespace BlazorSlice.Toast;

public enum ButtonVariant
{
    [Description("")]
    Default,
    [Description("ghost")]
    Ghost,
    [Description("outline")]
    Outline
}