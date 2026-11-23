using UnityEngine;
using UnityEngine.Tilemaps;

// i decided to give this its own script because the tilemap handler is already getting crowded anyway.
// this will also handle the arrow effect when using Move

public class HighlightsHandler : MonoBehaviour
{
    [SerializeField]
    private Tilemap _highlightMap;
    private TileBase _highlightTile;
    private Vector3Int _oldLocation = new Vector3Int(0, 0, 0);

    void Awake()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.black);
        texture.Apply();

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, 1, 1),
            Vector2.zero
        );

        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        tile.color = new Color(0, 0, 0, 0.5f);

        _highlightTile = tile; 
    }


    void HandleHoverEffects(ModeType mode, Vector2 MousePosition, Vector3Int PlayerPos)
    {
        Vector3 screenPos = getCellTileByMousePos(MousePosition);
        if (mode == ModeType.PLACE)
        {
            
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


