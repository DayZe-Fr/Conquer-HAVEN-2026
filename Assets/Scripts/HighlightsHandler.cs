using UnityEngine;
using UnityEngine.Tilemaps;

// i decided to give this its own script because the tilemap handler is already getting crowded anyway.
// this will also handle the arrow effect when using Move
[RequireComponent(typeof(TilemapUtil))]
public class HighlightsHandler : MonoBehaviour
{
    [SerializeField]
    private Tilemap _highlightMap;
    [SerializeField]
    private TileBase _highlightTile;
    private Vector3Int _oldLocation = new Vector3Int(0, 0, 0);

    [SerializeField]
    private TilemapUtil _tilemapUtil;


    public void HandleHoverEffects(ModeType mode, Vector2 MousePosition, Vector3Int PlayerPos)
    {
        Vector3Int CellPos = getCellTileByMousePos(MousePosition);
        if (!_tilemapUtil.checkInBounds(CellPos))
        {
            _highlightMap.SetTile(_oldLocation, null);
            return;
        }
        if (CellPos == _oldLocation)
        {
            return;
        }

        // 

        if (mode == ModeType.PLACE)
        {
            _highlightMap.SetTile(_oldLocation, null);
            _highlightMap.SetTile(CellPos, _highlightTile);

            _oldLocation = CellPos;
        }
    }

    public enum ModeType
    {
        PLACE,
        MOVE
    }

    private Vector3Int getCellTileByMousePos(Vector2 mousepos)
    {
        Vector3Int cell = _highlightMap.WorldToCell(
            Camera.main.ScreenToWorldPoint(mousepos)
        );
        return cell;
    }
}


