using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;


[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(TileMap_Handler))]
[RequireComponent(typeof(CameraHandler))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private TileMap_Handler _tileHandler;
    [SerializeField]
    private CameraHandler _camHandler;
    [SerializeField]
    private InputHandler _inputHandler;

    
    void Start()
    {
        
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _tileHandler.editTile(Mouse.current.position.ReadValue(), TileTypes.Fire);
        }
    }
}

public class MapSize
{
    private MapSize() {}

    
    public readonly int minX = -8;
    public readonly int maxX = 1;
    public readonly int minY = -5;
    public readonly int maxY = 4;
    
} 
