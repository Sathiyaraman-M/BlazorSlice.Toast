namespace BlazorSlice.Toast.Services;

public class Toast : IDisposable
{
    private Timer Timer { get; set; }
    internal ToastMessageState State { get; }
    internal ToastMessage Message { get; }
    public event Action<Toast> OnClose;
    public event Action OnUpdate;
    public Severity Severity => State.Options.Severity;

    internal Toast(ToastMessage message, ToastOptions options)
    {
        Message = message;
        State = new ToastMessageState(options);
        Timer = new Timer(TimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
    }
    
    internal void Init() => TransitionTo(ToastState.Showing);

    internal void Clicked(bool fromCloseIcon)
    {
        if (!fromCloseIcon)
        {
            // Do not start the hiding transition if no click action
            if (State.Options.Onclick == null)
                return;

            // Click action is executed only if it's not from the close icon
            State.Options.Onclick.Invoke(this);
        }

        State.UserHasInteracted = true;
        TransitionTo(ToastState.Hiding);
    }
    private void TransitionTo(ToastState state)
    {
        StopTimer();
        State.ToastState = state;
        var options = State.Options;

        if (state.IsShowing())
        {
            if (options.ShowTransitionDuration <= 0) TransitionTo(ToastState.Visible);
            else StartTimer(options.ShowTransitionDuration);
        }
        else if (state.IsVisible() && !options.RequireInteraction)
        {
            if (options.VisibleStateDuration <= 0) TransitionTo(ToastState.Hiding);
            else StartTimer(options.VisibleStateDuration);
        }
        else if (state.IsHiding())
        {
            if (options.HideTransitionDuration <= 0) OnClose?.Invoke(this);
            else StartTimer(options.HideTransitionDuration);
        }

        OnUpdate?.Invoke();
    }
    
    private void TimerElapsed(object state)
    {
        switch (State.ToastState)
        {
            case ToastState.Showing:
                TransitionTo(ToastState.Visible);
                break;
            case ToastState.Visible:
                TransitionTo(ToastState.Hiding);
                break;
            case ToastState.Hiding:
                OnClose?.Invoke(this);
                break;
            case ToastState.Init:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(State.ToastState));
        }
    }

    private void StartTimer(int duration)
    {
        State.Stopwatch.Restart();
        Timer?.Change(duration, Timeout.Infinite);
    }

    private void StopTimer()
    {
        State.Stopwatch.Stop();
        Timer?.Change(Timeout.Infinite, Timeout.Infinite);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        StopTimer();

        var timer = Timer;
        Timer = null;

        timer?.Dispose();
    }
}