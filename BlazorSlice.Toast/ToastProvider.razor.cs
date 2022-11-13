namespace BlazorSlice.Toast;

public partial class ToastProvider : IDisposable
{
    [Inject] public IToast Toasts { get; set; }
    
    [Parameter]
    public string Class { get; set; }
    
    [Parameter]
    public string Style { get; set; }
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UserAttributes { get; set; } = new();
    
    protected IEnumerable<Services.Toast> Toast => Toasts.Configuration.NewestOnTop
        ? Toasts.ShownToasts.Reverse()
        : Toasts.ShownToasts;

    private string ClassName => new CssBuilder()
        .AddClass("toast-container")
        .AddClass("position-absolute")
        .AddClass(Toasts.Configuration.PositionClass)
        .AddClass("p-3")
        .AddClass(Class)
        .Build();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Toasts.OnToastsUpdated += OnToastsUpdated;
    }

    private void OnToastsUpdated()
    {
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        Toasts.OnToastsUpdated -= OnToastsUpdated;
    }
}