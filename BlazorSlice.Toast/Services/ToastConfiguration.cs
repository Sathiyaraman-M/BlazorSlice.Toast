namespace BlazorSlice.Toast.Services;

public class ToastConfiguration : CommonToastOptions
{
    private bool _newestOnTop;
    private bool _preventDuplicates;
    private int _maxDisplayedToasts;
    private string _positionClass;
    private bool _clearAfterNavigation;

    internal event Action OnUpdate;

    public bool NewestOnTop
    {
        get => _newestOnTop;
        set
        {
            _newestOnTop = value;
            OnUpdate?.Invoke();
        }
    }

    public bool PreventDuplicates
    {
        get => _preventDuplicates;
        set
        {
            _preventDuplicates = value;
            OnUpdate?.Invoke();
        }
    }

    public int MaxDisplayedToasts
    {
        get => _maxDisplayedToasts;
        set
        {
            _maxDisplayedToasts = value;
            OnUpdate?.Invoke();
        }
    }

    public string PositionClass
    {
        get => _positionClass;
        set
        {
            _positionClass = value;
            OnUpdate?.Invoke();
        }
    }

    public bool ClearAfterNavigation
    {
        get => _clearAfterNavigation;
        set
        {
            _clearAfterNavigation = value;
            OnUpdate?.Invoke();
        }
    }

    public ToastConfiguration()
    {
        PositionClass = ToastPosition.TopRight.ToDescriptionString();
        NewestOnTop = false;
        PreventDuplicates = true;
        MaxDisplayedToasts = 5;
    }
}