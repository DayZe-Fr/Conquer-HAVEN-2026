using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[DefaultExecutionOrder(-100)]
public class TilemapUtil : MonoBehaviour
{
    public static TilemapUtil instance { get; private set; }

    [SerializeField]
    private Tilemap _mainMap;
    [SerializeField]
    private Tilemap _charTilemap;



    [SerializeField]
    private List<TileSetting> _tileSettings;
    private Dictionary<TileTypes, TileSetting> _settingsByType;
    private Dictionary<TileBase, TileSetting> _settingsByTile;


    void Awake()
    {
        instance = this;

        _settingsByType = new Dictionary<TileTypes, TileSetting>();
        _settingsByTile = new Dictionary<TileBase, TileSetting>();

        foreach (TileSetting setting in _tileSettings)
        {
            _settingsByType[setting.Type] = setting;
            _settingsByTile[setting.Tile] = setting;
        }
    }

    public Vector3Int getCellByScreenPos(Vector2 screenPos)
    {
        Vector3Int cell = _mainMap.WorldToCell(
            Camera.main.ScreenToWorldPoint(screenPos)
        );
        return cell;
    }

    public bool checkInBounds(Vector3Int cell)
    {
        if (cell.x > MapSize.maxX
            || cell.x < MapSize.minX
            || cell.y > MapSize.maxY
            || cell.y < MapSize.minY)
        {
            return false;
        }
        return true;
    }

    public TileBase getTile(Vector3Int cell)
    {
        return _mainMap.GetTile(cell);
    }

    public bool isTileOwned(Vector3Int cell)
    {
        return _mainMap.GetTile(cell) != null;
    }

    public void setTile(Vector3Int cell, TileBase tile)
    {
        _mainMap.SetTile(cell, tile);
    }

    public void setCharTile(Vector3Int cell, TileBase tile)
    {
        _charTilemap.SetTile(cell, tile);
    }

    public TileSetting getSettingsByType(TileTypes type)
    {
        return _settingsByType[type];
    }

    public TileSetting getSettingByTile(TileBase tile)
    {
        if (tile == null) return null;

        TileSetting settinger;
        _settingsByTile.TryGetValue(tile, out settinger);
        return settinger;
    }

    public TileSetting getSettingByCell(Vector3Int cell)
    {
        return getSettingByTile(_mainMap.GetTile(cell));
    }

}
