using UnityEngine;

public class DShakeLib : MonoBehaviour
{
    public static bool IsInitialized = false;
    public static void setInitialized(bool val)
    {
        DTweenLib.IsInitialized = val;
        IsInitialized = val;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

}
