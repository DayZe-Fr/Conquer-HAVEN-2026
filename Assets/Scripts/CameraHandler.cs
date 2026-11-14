using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;

// aspect stuff
    private const float _aspect_ratio = 16f / 10f;
    
    
    void Start()
    {
        
    }

    
    void Update()
    {
        AspectUpdate();
    }

    private void AspectUpdate()
    {
        float _prevWidth = Screen.width;
        float _prevHeight = Screen.height;

        float currentAspect = _prevWidth / _prevHeight;
        
        if (currentAspect != _aspect_ratio)
        {
            Screen.SetResolution((int) _prevWidth, (int)(_prevHeight/_aspect_ratio), true);
        }
    }
}
