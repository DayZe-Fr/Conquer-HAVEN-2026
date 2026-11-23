using UnityEngine;

public class DZUtil_Driver : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DTweenLib.RunTween(Time.deltaTime);
    }
}
