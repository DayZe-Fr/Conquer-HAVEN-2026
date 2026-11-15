using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMap_Handler : MonoBehaviour
{
    [SerializeField]
    private Tilemap _mainMap;
    [SerializeField]
    private Tilemap _charTilemap;

    [SerializeField]
    private TileBase water;
    [SerializeField]
    private TileBase wind;
    [SerializeField]
    private TileBase fire;
    [SerializeField]
    private TileBase tar;
    [SerializeField]
    private TileBase lays;


    //

    [SerializeField]
    private TileBase p1Icon;
    [SerializeField]
    private TileBase bot2Icon;
    [SerializeField]
    private TileBase bot3Icon;
    [SerializeField]
    private TileBase bot4Icon;

    
    private Dictionary<TileTypes, TileBase> _tileSet;
    private Dictionary<Player_Identifier, List<Vector3Int>> _ownedTiles;

    private Dictionary<Player_Identifier, Vector3Int> _spawnPositions;

    private Dictionary<Player_Identifier, TileTypes> _spawnTiles;

    [SerializeField]
    private Roundhandler _roundhandler;




    private bool spawned = false;
    public Vector3Int Player1Position => player1.playerPos;
    //

    private static class player1
    {
        public static Vector3Int playerPos = new Vector3Int(-6, 2, 0);
        public static List<Vector3Int> ownedTiles = new List<Vector3Int>()
        {
            new Vector3Int(-6, 2, 0)
        };
    }

    public static Vector3Int player1Pos => player1.playerPos;
    public static List<Vector3Int> player1OwnedTiles => player1.ownedTiles;

    public static void setPlrPos(Vector3Int newCell)
    {
        player1.playerPos = newCell;
    }

    void Awake()
    {
        _tileSet = new Dictionary<TileTypes, TileBase>()
        {
            { TileTypes.Water, water },
            { TileTypes.Fire, fire },
            { TileTypes.Wind, wind },
            { TileTypes.Tar, tar },
            { TileTypes.Lays_barbacue_chips, lays }
        };
        _ownedTiles = new Dictionary<Player_Identifier, List<Vector3Int>>()
        {
            { Player_Identifier.Player1, new List<Vector3Int>() },
            { Player_Identifier.Bot2, new List<Vector3Int>() },
            { Player_Identifier.Bot3, new List<Vector3Int>() },
            { Player_Identifier.Bot4, new List<Vector3Int>() }
        };
        _spawnPositions = new Dictionary<Player_Identifier, Vector3Int>()
        {
            { Player_Identifier.Player1, new Vector3Int(-6, 2, 0) }, 
            { Player_Identifier.Bot2, new Vector3Int(-1, 2, 0) }, // lays barbacue chips
            { Player_Identifier.Bot3, new Vector3Int(-6, -3, 0) }, // water
            { Player_Identifier.Bot4, new Vector3Int(-1, -3, 0) } // wind
        };

        _spawnTiles = new Dictionary<Player_Identifier, TileTypes>()
        {
            { Player_Identifier.Player1, TileTypes.Fire },
            { Player_Identifier.Bot2, TileTypes.Lays_barbacue_chips },
            { Player_Identifier.Bot3, TileTypes.Water },
            { Player_Identifier.Bot4, TileTypes.Wind }
        };
        
    
        _charTilemap.SetTile(_spawnPositions[Player_Identifier.Bot2], bot2Icon);
        _charTilemap.SetTile(_spawnPositions[Player_Identifier.Bot3], bot3Icon);
        _charTilemap.SetTile(_spawnPositions[Player_Identifier.Bot4], bot3Icon);

        BotsHandler.Init(_spawnPositions);
    }

    public void editTile(Vector2 mouseScreenPos, TileTypes tileName)
    {
        Vector3 targetpos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        
        Vector3Int cell = _mainMap.WorldToCell(targetpos);

        if (!checkInBounds(cell)) return;

        TileBase targettedcell = _tileSet[tileName];

        _mainMap.SetTile(cell, targettedcell);
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

    public bool editTileOnPlayer(
        Vector2 mouseScreenPos, 
        TileTypes tileName, 
        int rad, 
        Player_Identifier playerIdentifier)
    {

        if (!_ownedTiles.ContainsKey(playerIdentifier))
        return false;

        List<Vector3Int> playerOwnedTiles;

        if (playerIdentifier == Player_Identifier.Player1)
        {
            playerOwnedTiles = player1.ownedTiles;
        } else
        {
            playerOwnedTiles = _ownedTiles[playerIdentifier];
        }

        Vector3Int clickedCell = _mainMap.WorldToCell(
             Camera.main.ScreenToWorldPoint(mouseScreenPos)
        );
    
        if (!checkInBounds(clickedCell)) return false;
    
        
        if (playerOwnedTiles.Contains(clickedCell)) return false;
    
        bool isInRange = false;
    
        for (int x = -rad; x <= rad; x++) 
        {
            for (int y = -rad; y <= rad; y++)
            {
                Vector3Int checkCell = clickedCell + new Vector3Int(x, y, 0);
    
                if (playerOwnedTiles.Contains(checkCell))
                {
                    isInRange = true;
                    break;
                }
            }
    
            if (isInRange) break;
        }
    
        if (!isInRange) return false;

        int cost = getTileCost(clickedCell);

        if (!_roundhandler.spendPoints(cost)) return false;

        editTile(mouseScreenPos, tileName);

        playerOwnedTiles.Add(clickedCell);

        return true;
    }

    private int getTileCost(Vector3Int cell)
    {
        TileBase targetTile = _mainMap.GetTile(cell);

        if (targetTile == null) return 1;
    
        return 4;
    }

    public void SpawnChars()
    {
        if (spawned) return;
        foreach (var participant in _spawnPositions)
        {
            Player_Identifier id = participant.Key;
            Vector3Int position = participant.Value;

            TileTypes tileType = _spawnTiles[id];

            _mainMap.SetTile(position, _tileSet[tileType]);

            _ownedTiles[id].Add(position);
        }
        spawned = true;
    }

    public Vector3Int getCellTileByMousePos(Vector2 mousepos)
    {
        Vector3Int cell = _mainMap.WorldToCell(
            Camera.main.ScreenToWorldPoint(mousepos)
        );
        return cell;
    }
}

public static class MapSize
{    
    public static readonly int minX = -8;
    public static readonly int maxX = 1;
    public static readonly int minY = -5;
    public static readonly int maxY = 4;
} 


public enum TileTypes
{
    Water,
    Wind,
    Fire,
    Tar,
    Lays_barbacue_chips
}

public enum Player_Identifier
{
    Player1,
    Bot2,
    Bot3,
    Bot4
}