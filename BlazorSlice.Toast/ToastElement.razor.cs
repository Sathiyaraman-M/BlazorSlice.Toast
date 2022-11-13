using BlazorSlice.Toast.Services;

namespace BlazorSlice.Toast;

public partial class ToastElement : IDisposable
{
    [Parameter]
    public string Style { get; set; }
    
    [Parameter]
    public Services.Toast Toast { get; set; }

    [Parameter] 
    public string CloseIcon { get; set; } = TablerIcons.X;
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UserAttributes { get; set; } = new();

    private string Action => Toast?.State.Options.Action;
    private Color ActionColor => Toast?.State.Options.ActionColor ?? Color.Primary;
    private ButtonVariant ActionVariant => Toast?.State.Options.ActionVariant ?? ButtonVariant.Ghost;
    private string AnimationStyle => Toast?.State.AnimationStyle + Style;
    private string ToastClass => Toast?.State.ToastClass;
    private RenderFragment Css;
    private bool ShowActionButton => Toast?.State.ShowActionButton == true;
    private bool ShowCloseIcon => Toast?.State.ShowCloseIcon == true;
    
    private bool HideIcon => Toast?.State.HideIcon == true;
    private string Icon => Toast?.State.Icon;
    private Color IconColor => Toast?.State.Options.IconColor ?? Color.Primary;
    private Size IconSize => Toast?.State.Options.IconSize ?? Size.Default;
    
    private void ActionClicked() => Toast?.Clicked(false);
    private void CloseIconClicked() => Toast?.Clicked(true);
    private ToastMessage Message => Toast?.Message;
    
    private void ToastClicked()
    {
        if (!ShowActionButton)
            Toast?.Clicked(false);
    }

    private void ToastUpdated()
    {
        InvokeAsync(StateHasChanged);
    }

    protected override void OnInitialized()
    {
        if (Toast == null) return;
        Toast.OnUpdate += ToastUpdated;
        Toast.Init();

        Css = builder =>
        {
            var transitionClass = Toast.State.TransitionClass;

            if (string.IsNullOrWhiteSpace(transitionClass)) return;
            builder.OpenElement(1, "style");
            builder.AddContent(2, transitionClass);
            builder.CloseElement();
        };
    }

    public void Dispose()
    {
        if (Toast != null)
            Toast.OnUpdate -= ToastUpdated;
    }
}