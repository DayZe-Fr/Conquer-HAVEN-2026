using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;


[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(TileMap_Handler))]
[RequireComponent(typeof(CameraHandler))]
[RequireComponent(typeof(Roundhandler))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private TileMap_Handler _tileHandler;
    [SerializeField]
    private CameraHandler _camHandler;
    [SerializeField]
    private InputHandler _inputHandler;
    [SerializeField]
    private Roundhandler _roundhandler;

    
    void Start()
    {
        _tileHandler.SpawnChars();
    }

    void Update()
    {
        tilePlacement();
    }

    private void tilePlacement()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_roundhandler.points <= 0) return;
            bool didPlace = _tileHandler.editTileOnPlayer(
                Mouse.current.position.ReadValue(),
                TileTypes.Fire,
                1,
                Player_Identifier.Player1
            );
            
            _roundhandler.nextRound();
        }
    }
}
