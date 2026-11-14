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

        TileBase targettedcell = _tileSet[tileName];

        _mainMap.SetTile(cell, targettedcell);
    }
}

public enum TileTypes
{
    Water,
    Wind,
    Fire,
    Tar,
    Lays_barbacue_chips
}
