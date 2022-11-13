namespace BlazorSlice.Toast.Services;

internal class ToastMessage
{
    internal Type ComponentType { get; }
    internal Dictionary<string, object> ComponentParameters { get; }
    internal string Key { get; }

    internal ToastMessage(Type componentType, Dictionary<string, object> componentParameters = null, string key = "")
    {
        ComponentType = componentType;
        ComponentParameters = componentParameters;
        Key = key;
    }
}