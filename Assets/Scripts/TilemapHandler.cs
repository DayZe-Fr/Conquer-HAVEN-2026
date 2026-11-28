using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapHandler : MonoBehaviour
{
    private TilemapUtil _util => TilemapUtil.instance;


    [SerializeField]
    private Tilemap _mainMap;

    [SerializeField]
    private Tilemap _charTilemap;

    [Space(10)]

    // TILE things

    [SerializeField]
    private TileSetting water;

    [SerializeField]
    private TileSetting wind;

    [SerializeField]
    private TileSetting fire;

    [SerializeField]
    private TileSetting lays;

    [Space(10)]

    // ICONS and stuff

    [SerializeField]
    private TileBase bot2Icon;

    [SerializeField]
    private TileBase bot3Icon;

    [SerializeField]
    private TileBase bot4Icon;

    [Space(10)]

    //OBJECTS i need

    [SerializeField]
    private AudioHandler _audioHandler;
    [SerializeField]
    private Roundhandler _roundhandler;
    [SerializeField]
    private ParticlesHandler _particleHandler;
    [SerializeField]
    public BotsHandler _bothandler;

    [Space(10)]

    //SOUNDS and shiz

    [SerializeField]
    private SoundEffect playerDead;
    [SerializeField]
    private SoundEffect placeTile;
    [SerializeField]
    private SoundEffect actionDenied;

    // other stuff idk

    private Dictionary<Player_Identifier, Vector3Int> _spawnPositions;

    private Dictionary<Player_Identifier, TileTypes> _spawnTiles;
    private bool spawned = false;
    public Vector3Int Player1Pos => player1.playerPos;

    private static class player1
    {
        public static Vector3Int playerPos = new Vector3Int(-6, 2, 0);
    }

    public static void setPlrPos(Vector3Int newCell)
    {
        player1.playerPos = newCell;
    }

    void Awake()
    {
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

        _util.setCharTile(_spawnPositions[Player_Identifier.Bot2], bot2Icon);
        _util.setCharTile(_spawnPositions[Player_Identifier.Bot3], bot3Icon);
        _util.setCharTile(_spawnPositions[Player_Identifier.Bot4], bot4Icon);

        _bothandler.Init(_spawnPositions);
    }

    public void deleteCharacterTile(Vector3Int cell)
    {
        _util.setCharTile(cell, null);
        editTile(cell, TileTypes.Fire);
    }

    public void editTile(Vector2 mouseScreenPos, TileTypes tileName)
    {
        editTile(_util.getCellByScreenPos(mouseScreenPos), tileName);
    }

    public void editTile(Vector3Int cellPosition, TileTypes tileName)
    {
        if (!_util.checkInBounds(cellPosition)) return;

        TileBase targettedcell = _util.getSettingsByType(tileName).Tile;

        _util.setTile(cellPosition, targettedcell);
    }

    public bool isOwnedBy(Vector3Int cell, Player_Identifier id)
    {
        TileSetting setting = _util.getSettingByCell(cell);

        bool val = setting != null && setting.Accociate == id;
        return val;
    }

    public List<Vector3Int> getOwnedcells(Player_Identifier id)
    {
        List<Vector3Int> ownedcellsfromthisguy = new List<Vector3Int>();
        for (int x = MapSize.minX; x <= MapSize.maxX; x++)
        {
            for (int y = MapSize.minY; y <= MapSize.maxY; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);

                if (isOwnedBy(cell, id)) ownedcellsfromthisguy.Add(cell);
            }
        }

        return ownedcellsfromthisguy;
    }

    private bool isInRangeOfOwned(Vector3Int cell, int rad, Player_Identifier id)
    {
        for (int x = -rad; x <= rad; x++) 
        {
            for (int y = -rad; y <= rad; y++)
            {
                Vector3Int checkCell = cell + new Vector3Int(x, y, 0);

                if (isOwnedBy(checkCell, id)) return true;
            }
        }
        return false;
    } 

    public bool editTileOnPlayer(
        Vector2 mouseScreenPos, 
        TileTypes tileName, 
        int rad, 
        Player_Identifier playerIdentifier)
    {
        Vector3Int clickedCell = _util.getCellByScreenPos(mouseScreenPos);
    
        if (!_util.checkInBounds(clickedCell)) return false;
    
        if (isOwnedBy(clickedCell, playerIdentifier))
        {
            _audioHandler.playAudio(actionDenied);
            return false;
        }
    
        if (!isInRangeOfOwned(clickedCell, rad, playerIdentifier))
        {
            _audioHandler.playAudio(actionDenied);
            return false;
        };

        int cost = getTileCost(clickedCell);

        if (!_roundhandler.spendPoints(cost)) 
        {
            _audioHandler.playAudio(actionDenied);
            return false;
        }
        
        if (BotsHandler.isBotPos(clickedCell))
        {
            BotsHandler.deleteBot(clickedCell);
            _audioHandler.playAudio(playerDead);
        } else _audioHandler.playAudio(placeTile);

        _particleHandler.EmitTileBurstAtCell(10, getTiletypeByCell(clickedCell), clickedCell);
        editTile(clickedCell, tileName);

        _roundhandler.nextRound();
        return true;
    }

    private int getTileCost(Vector3Int cell)
    {
        TileBase targetTile = _util.getTile(cell);

        if (targetTile == null) return 1;
        if (BotsHandler.isBotPos(cell)) return 8;
    
        return 4;
    }

    public bool canBotClaim(Vector3Int cell, Player_Identifier id) //this mneeds to be modifiled again cuz it still aint taking other tiles, but thast a bit of work for the bot ai rework, so ima leave it fer now
    {
        if (!_util.checkInBounds(cell)) return false;

        if (_util.isTileOwned(cell)) return false;

        return true;
    }

    public bool claimBotTile(Vector3Int cell, Player_Identifier id)
    {
        if (!canBotClaim(cell, id)) return false;

        _util.setTile(cell, _util.getSettingsByType(_spawnTiles[id]).Tile);

        return true;
    }

    public void SpawnChars() //this shiz shoulda gone in the bot handler but cant be bothered to account for it. Maybe for a future steam release.
    {
        if (spawned) return;
        foreach (var participant in _spawnPositions)
        {
            Player_Identifier id = participant.Key;
            Vector3Int position = participant.Value;

            TileTypes tileType = _spawnTiles[id];

            _util.setTile(position, _util.getSettingsByType(tileType).Tile);
        }
        spawned = true;
    }

    public TileTypes getTiletypeByCell(Vector3Int cell)
    {
        TileSetting setting = _util.getSettingByCell(cell);

        if (setting == null) return TileTypes.Tar;

        return setting.Type;
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