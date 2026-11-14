using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMap_Handler : MonoBehaviour
{
    [SerializeField]
    private Tilemap _mainMap;

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

    [SerializeField]
    private Dictionary<TileTypes, TileBase> _tileSet;





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
    }

    public void editTile(Vector2 mouseScreenPos, TileTypes tileName)
    {
        Vector3 targetpos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        
        Vector3Int cell = _mainMap.WorldToCell(targetpos);

        if (!checkInBounds(cell))
        {
            return;
        }

        TileBase targettedcell = _tileSet[tileName];

        _mainMap.SetTile(cell, targettedcell);
    }

    private bool checkInBounds(Vector3Int cell)
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
        Vector3Int playerCellPos;
        List<Vector3Int> playerOwnedTiles;

        if (playerIdentifier == Player_Identifier.Player1)
        {
            playerCellPos = player1.playerPos;
            playerOwnedTiles = player1.ownedTiles;
        } else
        {
            return false;
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
    
        editTile(mouseScreenPos, tileName);
    
        playerOwnedTiles.Add(clickedCell);

        return true;
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
    Player1
}