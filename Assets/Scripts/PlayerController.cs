using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;


[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(TileMap_Handler))]
[RequireComponent(typeof(CameraHandler))]
[RequireComponent(typeof(Roundhandler))]
[RequireComponent(typeof(AudioHandler))]
[RequireComponent(typeof(HighlightsHandler))]

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
    [SerializeField]
    private AudioHandler _audioHandler;
    [SerializeField]
    private HighlightsHandler _highlightsHandler;
    





    [SerializeField]
    private AudioClip placeTile;
    [SerializeField]
    private AudioClip playerDead;
    [SerializeField]
    private AudioClip playerMove;

    [SerializeField]
    private GameObject player;



    private bool MoveMode = false;

    
    void Start()
    {
        _tileHandler.SpawnChars();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            if (MoveMode)
            {
                movePlayer(_tileHandler.getCellTileByMousePos(Mouse.current.position.ReadValue()));
            } else
            {
                tilePlacement();
            }
        }
        
        HighlightsHandler.ModeType highlightType = HighlightsHandler.ModeType.PLACE;
        if (MoveMode)
        {
            highlightType = HighlightsHandler.ModeType.MOVE;
        }

        _highlightsHandler.HandleHoverEffects(
            highlightType, 
            Mouse.current.position.ReadValue(), 
            _tileHandler.Player1Position
        );
    }

    private void tilePlacement()
    {
        if (_roundhandler.points <= 0) return;
        bool didPlace = _tileHandler.editTileOnPlayer(
            Mouse.current.position.ReadValue(),
            TileTypes.Fire,
            1,
            Player_Identifier.Player1
        );
        if (didPlace) 
        {
            _audioHandler.playAudio(placeTile);
        }
    }

    private void movePlayer(Vector3Int cellToMove)
    {
        bool canMove = true;
        if (cellToMove.x == TileMap_Handler.player1Pos.x &&
            _tileHandler.checkInBounds(cellToMove) &&
            TileMap_Handler.player1OwnedTiles.Contains(cellToMove)
        ) {
            int dir;

            Vector3Int CurrentPos = TileMap_Handler.player1Pos;

            if (cellToMove.y < CurrentPos.y) dir = -1;
            else dir = 1;

            Vector3Int checkingTile;

            for (int tile_num = 1; 
            tile_num < (int)Mathf.Abs(cellToMove.y - CurrentPos.y);
            tile_num++
            )
            {
                checkingTile = new Vector3Int(CurrentPos.x, CurrentPos.y+(tile_num * dir), 0);
                if (!TileMap_Handler.player1OwnedTiles.Contains(checkingTile)) {
                    canMove = false;
                    break;
                }
            }
            if (canMove) 
            {
                rollover(cellToMove);
                _audioHandler.playAudio(playerMove);
            }
        }



        else if (cellToMove.y == TileMap_Handler.player1Pos.y &&
            _tileHandler.checkInBounds(cellToMove) &&
            TileMap_Handler.player1OwnedTiles.Contains(cellToMove))
        {
            int dir;

            Vector3Int CurrentPos = TileMap_Handler.player1Pos;

            if (cellToMove.x < CurrentPos.x) dir = -1;
            else dir = 1;

            Vector3Int checkingTile;

            for (
                int tile_num = 1;
                tile_num < (int)Mathf.Abs(cellToMove.x - CurrentPos.x);
                tile_num++
            )
            {
                checkingTile = new Vector3Int(
                    CurrentPos.x + (tile_num * dir),
                    CurrentPos.y,
                    0
                );

                if (!TileMap_Handler.player1OwnedTiles.Contains(checkingTile)) {
                    canMove = false;
                    break;
                }
            }
            if (canMove) rollover(cellToMove);
        }
        MoveMode = false;
    }

    private void rollover(Vector3Int celltomove)
    {
        TileMap_Handler.setPlrPos(celltomove);

        Vector3 pos = (Vector3)celltomove + new Vector3(0.5f, 0.5f, 0f);

        DTweenLib.NewTween(pos, 0.2f, () => player.transform.position, v => player.transform.position = v);
    }

    public void setMoveMode()
    {
        MoveMode = true;
    }
}
