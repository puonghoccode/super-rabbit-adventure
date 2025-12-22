public static class LoadingScreenData
{
    public static string TargetScene { get; private set; }

    public static void SetTarget(string sceneName)
    {
        TargetScene = sceneName;
    }

    public static void Clear()
    {
        TargetScene = null;
    }
}
