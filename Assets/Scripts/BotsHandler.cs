using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BotsHandler : MonoBehaviour
{
    [SerializeField]
    private static TilemapHandler tileMap_Handler;
    private static List<Bot> bots;

    private static bool initialized = false;

    public void Init(Dictionary<Player_Identifier, Vector3Int> spawnPos)
    {
        if (initialized) return;
        bots = new List<Bot>();
    
        foreach (KeyValuePair<Player_Identifier, Vector3Int> entry in spawnPos)
        {
            if (entry.Key == Player_Identifier.Player1) continue;
            Bot newBot = new Bot(entry.Value, entry.Key);
            bots.Add(newBot);
        }
        tileMap_Handler = gameObject.GetComponent<TilemapHandler>();
        initialized = true;
    }

public static void botsTurn()
    {
        if (bots == null || tileMap_Handler == null) return;

        foreach (Bot bot in bots)
        {
            List<Vector3Int> availableCells = new List<Vector3Int>();

            foreach (Vector3Int ownedCell in bot.ownedTiles)
            {
                Vector3Int[] directions =
                {
                    Vector3Int.up,
                    Vector3Int.down,
                    Vector3Int.left,
                    Vector3Int.right
                };

                foreach (Vector3Int direction in directions)
                {
                    Vector3Int targetCell = ownedCell + direction;

                    if (!availableCells.Contains(targetCell) &&
                        tileMap_Handler.canBotClaim(targetCell, bot.identifer))
                    {
                        availableCells.Add(targetCell);
                    }
                }
            }

            if (availableCells.Count == 0) continue;

            Vector3Int chosenCell = availableCells[Random.Range(0, availableCells.Count)];

            if (tileMap_Handler.claimBotTile(chosenCell, bot.identifer))
            {
                bot.ownedTiles.Add(chosenCell);
            }
        }
    }

    private class Bot
    {
        private Vector3Int _BotPos;
        public Vector3Int BotPos => _BotPos;
        private List<Vector3Int> _ownedTiles;
        public List<Vector3Int> ownedTiles => _ownedTiles;
        private Player_Identifier _identifer;
        public Player_Identifier identifer => _identifer;

        public Bot(Vector3Int SpawnPos, Player_Identifier Botidentifier)
        {
            _BotPos = SpawnPos;
            _ownedTiles = new List<Vector3Int>()
            {
                SpawnPos
            };
            _identifer = Botidentifier;
        }

        public void ChangeBotPos(Vector3Int newPos)
        {
            _BotPos = newPos;
        }
    }

    public static bool isBotPos(Vector3Int cell)
    {
        if (bots == null) return false;
        foreach (Bot bot in bots)
        {
            if (bot.BotPos == cell) return true;
        }
        return false;
    }

    public static void deleteBot(Vector3Int cell)
    {
        Bot bottodelete = getBotByCell(cell);
        tileMap_Handler.deleteCharacterTile(cell);
        bots.Remove(bottodelete);
    }

    private static Bot getBotByCell(Vector3Int cell)
    {
        foreach (Bot bot in bots)
        {
            if (bot.BotPos == cell)
            return bot;
        }

        return null;
    }
}
