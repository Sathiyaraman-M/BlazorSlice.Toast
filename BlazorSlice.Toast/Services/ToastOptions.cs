namespace BlazorSlice.Toast.Services;

public class ToastOptions : CommonToastOptions
{
    public Func<Toast, Task> Onclick { get; set; }

    public string Action { get; set; }
    public ButtonVariant? ActionVariant { get; set; }
    public Color ActionColor { get; set; } = Color.Primary;

    public Severity Severity { get; }

    public string SnackbarTypeClass { get; set; }

    public bool CloseAfterNavigation { get; set; }
    public bool HideIcon { get; set; }

    public string Icon { get; set; }
    public Color IconColor { get; set; } = Color.Primary;
    public Size IconSize { get; set; } = Size.Default;

    public ToastDuplicatesBehavior DuplicatesBehavior { get; set; } = ToastDuplicatesBehavior.GlobalDefault;

    [Parameter] public string NormalIcon { get; set; } = TablerIcons.Note;

    [Parameter] public string InfoIcon { get; set; } = TablerIcons.Info_square;

    [Parameter] public string SuccessIcon { get; set; } = TablerIcons.Circle_check;

    [Parameter] public string WarningIcon { get; set; } = TablerIcons.Alert_triangle;

    [Parameter] public string ErrorIcon { get; set; } = TablerIcons.Playstation_x;
    
    public ToastOptions(Severity severity, CommonToastOptions options) : base(options)
    {
        Severity = severity;

        SnackbarTypeClass = "toast";

        Icon = Severity switch
        {
            Severity.Normal => NormalIcon,
            Severity.Info => InfoIcon,
            Severity.Success => SuccessIcon,
            Severity.Warning => WarningIcon,
            Severity.Error => ErrorIcon,
            _ => throw new ArgumentOutOfRangeException(nameof(Severity)),
        };

        IconColor = Severity switch
        {
            Severity.Normal => Color.Primary,
            Severity.Info => Color.Info,
            Severity.Success => Color.Success,
            Severity.Warning => Color.Warning,
            Severity.Error => Color.Danger,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}