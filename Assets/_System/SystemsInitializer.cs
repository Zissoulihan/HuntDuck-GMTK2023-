using UnityEngine;

public static class SystemsInitializer
{
    //Credit: Tarodev
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute() => Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("[ SYSTEMS ]")));
}
