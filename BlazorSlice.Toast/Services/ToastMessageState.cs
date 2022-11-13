using System.Diagnostics;

namespace BlazorSlice.Toast.Services;

internal class ToastMessageState
{
    private string AnimationId { get; }
    public bool UserHasInteracted { get; set; }
    public ToastOptions Options { get; }
    public ToastState ToastState { get; set; }
    public Stopwatch Stopwatch { get; } = new Stopwatch();

    public ToastMessageState(ToastOptions options)
    {
        Options = options;
        AnimationId = $"snackbar-{Guid.NewGuid()}";
        ToastState = ToastState.Init;
    }
    private string Opacity => ((decimal)Options.MaximumOpacity / 100).ToPercentage();

    public bool ShowActionButton => !string.IsNullOrWhiteSpace(Options.Action);
    public bool ShowCloseIcon => Options.ShowCloseIcon;

    public bool HideIcon => Options.HideIcon;
    public string Icon => Options.Icon;
    public Color IconColor => Options.IconColor;
    public Size IconSize => Options.IconSize;

    public string ProgressBarStyle
    {
        get
        {
            var duration = RemainingTransitionMilliseconds(Options.VisibleStateDuration);
            return $"width:100;animation:{AnimationId} {duration}ms;";
        }
    }

    public string AnimationStyle
    {
        get
        {
            const string template = "opacity: {0}; animation: {1}ms linear {2};";

            switch (ToastState)
            {
                case ToastState.Showing:
                    var showingDuration = RemainingTransitionMilliseconds(Options.ShowTransitionDuration);
                    return string.Format(template, Opacity, showingDuration, AnimationId);

                case ToastState.Hiding:
                    var hidingDuration = RemainingTransitionMilliseconds(Options.HideTransitionDuration);
                    return string.Format(template, 0, hidingDuration, AnimationId);

                case ToastState.Visible:
                    return $"opacity: {Opacity};";

                case ToastState.Init:
                default:
                    return string.Empty;
            }
        }
    }

    public string ToastClass
    {
        get
        {
            var result = "toast show";

            if (Options.Onclick != null && !ShowActionButton)
                result += " force-cursor";

            return result;
        }
    }

    public string TransitionClass
    {
        get
        {
            var template = "@keyframes " + AnimationId + " {{from{{ {0}: {1}; }} to{{ {0}: {2}; }}}}";

            return ToastState switch
            {
                ToastState.Showing => string.Format(template, "opacity", "0%", Opacity),
                ToastState.Hiding => string.Format(template, "opacity", Opacity, "0%"),
                ToastState.Visible => string.Format(template, "width", "100%", "0%"),
                _ => string.Empty,
            };
        }
    }

    private int RemainingTransitionMilliseconds(int transitionDuration)
    {
        var duration = transitionDuration - (int)Stopwatch.ElapsedMilliseconds;

        return duration >= 0 ? duration : 0;
    }
}