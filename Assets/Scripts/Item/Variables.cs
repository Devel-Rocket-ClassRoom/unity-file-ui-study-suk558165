using System;

public static class Variables
{
    public static Languages language;
    public static event Action OnLanguageChanged;

    public static void InvokeLanguageChanged()
    {
        OnLanguageChanged?.Invoke();
    }
}
