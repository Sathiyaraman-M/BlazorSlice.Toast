using Microsoft.AspNetCore.Components.Routing;

namespace BlazorSlice.Toast.Services;

public class ToastService : IToast
{
    public ToastConfiguration Configuration { get; }
    public event Action OnToastsUpdated;

    private NavigationManager _navigationManager;
    private ReaderWriterLockSlim ToastLock { get; }
    private IList<Toast> ToastList { get; }

    public ToastService(NavigationManager navigationManager, ToastConfiguration configuration = null)
    {
        _navigationManager = navigationManager;
        configuration ??= new ToastConfiguration();

        Configuration = configuration;
        Configuration.OnUpdate += ConfigurationUpdated;
        navigationManager.LocationChanged += NavigationManager_LocationChanged;

        ToastLock = new ReaderWriterLockSlim();
        ToastList = new List<Toast>();
    }

    public IEnumerable<Toast> ShownToasts
    {
        get
        {
            ToastLock.EnterReadLock();
            try
            {
                return ToastList.Take(Configuration.MaxDisplayedToasts);
            }
            finally
            {
                ToastLock.ExitReadLock();
            }
        }
    }
    
    private Toast Add(ToastMessage message, Severity severity = Severity.Normal, Action<ToastOptions> configure = null)
    {
        var options = new ToastOptions(severity, Configuration);
        configure?.Invoke(options);

        var toast = new Toast(message, options);

        ToastLock.EnterWriteLock();
        try
        {
            if (ResolvePreventDuplicates(options) && ToastAlreadyPresent(toast)) return null;
            toast.OnClose += Remove;
            ToastList.Add(toast);
        }
        finally
        {
            ToastLock.ExitWriteLock();
        }

        OnToastsUpdated?.Invoke();

        return toast;
    }
    
    public Toast Add(string title, string message, Severity severity = Severity.Normal, Action<ToastOptions> configure = null, string key = "")
    {
        if (message.IsEmpty()) return null;
        message = message.Trimmed();

        var componentParams = new Dictionary<string, object>
        {
            { "Message", new MarkupString(message) }, 
            { "Title", title }
        };

        return Add
        (
            new ToastMessage(typeof(ToastMessageText), componentParams, string.IsNullOrEmpty(key) ? message : key),
            severity,
            configure
        );
    }

    public Toast Add(string title, RenderFragment message, Severity severity = Severity.Normal, Action<ToastOptions> configure = null, string key = "")
    {
        if (message == null) return null;

        var componentParams = new Dictionary<string, object>
        {
            { "Title", title },
            { "Message", message }
        };

        return Add
        (
            new ToastMessage(typeof(ToastMessageRenderFragment), componentParams, key),
            severity,
            configure
        );
    }

    public Toast Add<T>(Dictionary<string, object> componentParameters = null, Severity severity = Severity.Normal, Action<ToastOptions> configure = null,
        string key = "") where T : IComponent
    {
        var type = typeof(T);
        var message = new ToastMessage(type, componentParameters, key);

        return Add(message, severity, configure);
    }

    public void Clear()
    {
        ToastLock.EnterWriteLock();
        try
        {
            RemoveAllToasts(ToastList);
        }
        finally
        {
            ToastLock.ExitWriteLock();
        }

        OnToastsUpdated?.Invoke();
    }

    public void Remove(Toast toast)
    {
        toast.Dispose();
        toast.OnClose -= Remove;

        ToastLock.EnterWriteLock();
        try
        {
            var index = ToastList.IndexOf(toast);
            if (index < 0) return;
            ToastList.RemoveAt(index);
        }
        finally
        {
            ToastLock.ExitWriteLock();
        }

        OnToastsUpdated?.Invoke();
    }
    
    private bool ResolvePreventDuplicates(ToastOptions options)
    {
        return options.DuplicatesBehavior == ToastDuplicatesBehavior.Prevent
               || (options.DuplicatesBehavior == ToastDuplicatesBehavior.GlobalDefault && Configuration.PreventDuplicates);
    }

    private bool ToastAlreadyPresent(Toast newToast)
    {
        return !string.IsNullOrEmpty(newToast.Message.Key) && ToastList.Any(snackbar => newToast.Message.Key == snackbar.Message.Key);
    }
    
    private void ConfigurationUpdated()
    {
        OnToastsUpdated?.Invoke();
    }

    private void NavigationManager_LocationChanged(object sender, LocationChangedEventArgs e)
    {
        if (Configuration.ClearAfterNavigation)
        {
            Clear();
        }
        else
        {
            ShownToasts.Where(s => s.State.Options.CloseAfterNavigation).ToList().ForEach(Remove);
        }
    }
    
    private void RemoveAllToasts(IEnumerable<Toast> toasts)
    {
        if (ToastList.Count == 0) return;

        foreach (var toast in toasts)
        {
            toast.OnClose -= Remove;
            toast.Dispose();
        }

        ToastList.Clear();
    }
    
    public void Dispose()
    {
        Configuration.OnUpdate -= ConfigurationUpdated;
        _navigationManager.LocationChanged -= NavigationManager_LocationChanged;
        RemoveAllToasts(ToastList);
    }
}
