using UnityEngine;

public static class DShakeLib
{
    public static bool IsInitialized = false;


    private static float remainingTime = 0;

    public static void setInitialized(bool val)
    {
        DTweenLib.IsInitialized = val;
        IsInitialized = val;
    }
    

    public static void runShake()
    {
        
    }

}
