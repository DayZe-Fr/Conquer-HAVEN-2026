using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;


[RequireComponent(typeof(InputHandler))]

[RequireComponent(typeof(CameraHandler))]

[RequireComponent(typeof(Roundhandler))]

[RequireComponent(typeof(AudioHandler))]

[RequireComponent(typeof(HighlightsHandler))]

[RequireComponent(typeof(ParticlesHandler))]

[RequireComponent(typeof(CameraHandler))]

[RequireComponent(typeof(Scheduler))]

[RequireComponent(typeof(TilemapHandler))]
[RequireComponent(typeof(TilemapUtil))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private TilemapHandler _tileHandler;
    [SerializeField]
    private TilemapUtil _tileUtil;
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
    private ParticlesHandler _particlesHandler;
    

    [SerializeField]
    private SoundEffect playerMove;

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
                movePlayer(_tileUtil.getCellByScreenPos(Mouse.current.position.ReadValue()));
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
            _tileHandler.Player1Pos
        );
    }

    private void tilePlacement()
    {
        // if (_roundhandler.points <= 0) return;
        bool didPlace = _tileHandler.editTileOnPlayer(
            Mouse.current.position.ReadValue(),
            TileTypes.Fire,
            1,
            Player_Identifier.Player1
        );
    }

    private void movePlayer(Vector3Int cellToMove)
    {
        Player_Identifier TEMPORARY_HARDCODED_PLAYER_IDENTIFIER = Player_Identifier.Player1; // Full caps to grab attention, like yelling, "THERES DIRT HERE". hardcoded for noww but i will flexible-ify it laters.
        bool canMove = true;
        if (cellToMove.x == _tileHandler.Player1Pos.x &&
            _tileUtil.checkInBounds(cellToMove) &&
            _tileHandler.isOwnedBy(cellToMove, TEMPORARY_HARDCODED_PLAYER_IDENTIFIER)
        ) {
            int dir;

            Vector3Int CurrentPos = _tileHandler.Player1Pos;

            if (cellToMove.y < CurrentPos.y) dir = -1;
            else dir = 1;

            Vector3Int checkingTile;

            for (int tile_num = 1; 
            tile_num < (int)Mathf.Abs(cellToMove.y - CurrentPos.y);
            tile_num++
            )
            {
                checkingTile = new Vector3Int(CurrentPos.x, CurrentPos.y+(tile_num * dir), 0);
                if (!_tileHandler.isOwnedBy(checkingTile, TEMPORARY_HARDCODED_PLAYER_IDENTIFIER)) {
                    canMove = false;
                    break;
                }
            }
            if (canMove) 
            {
                _audioHandler.playAudio(playerMove);
                rollover(cellToMove);
            }
        }



        else if (cellToMove.y == _tileHandler.Player1Pos.y &&
            _tileUtil.checkInBounds(cellToMove) &&
            _tileHandler.isOwnedBy(cellToMove, TEMPORARY_HARDCODED_PLAYER_IDENTIFIER))
        {
            int dir;

            Vector3Int CurrentPos = _tileHandler.Player1Pos;

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

                if (!_tileHandler.isOwnedBy(checkingTile, TEMPORARY_HARDCODED_PLAYER_IDENTIFIER)) {
                    canMove = false;
                    break;
                }
            }
            if (canMove) 
            {
                _audioHandler.playAudio(playerMove);
                rollover(cellToMove);
            }
        }
        
        MoveMode = false;
        _particlesHandler.ArrowTrailToggle(false);
    }

    private void rollover(Vector3Int celltomove)
    {
        TilemapHandler.setPlrPos(celltomove);

        Vector3 pos = (Vector3)celltomove + new Vector3(0.5f, 0.5f, 0f);

        DTweenLib.NewTween(pos, 0.2f, () => player.transform.position, v => player.transform.position = v);
    }

    public void setMoveMode()
    {
        if (MoveMode) return;
        MoveMode = true;
        _particlesHandler.ArrowTrailToggle(true);
    }
}
