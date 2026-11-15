using System.Collections.Generic;
using UnityEngine;

public class BotsHandler : MonoBehaviour
{
    private static List<Bot> bots;

    private static bool initialized = false;



    public static void Init(Dictionary<Player_Identifier, Vector3Int> spawnPos)
    {
        if (initialized) return;
        bots = new List<Bot>();
    
        foreach (KeyValuePair<Player_Identifier, Vector3Int> entry in spawnPos)
        {
            if (entry.Key == Player_Identifier.Player1) continue;
            Bot newBot = new Bot(entry.Value, entry.Key);
            bots.Add(newBot);
        }
        
        initialized = true;
    }

    public static void botsTurn()
    {
        Debug.Log("Bots had a turn");
    }

    private class Bot
    {
        private Vector3Int _BotPos;
        public Vector3Int BotPos => _BotPos;
        private List<Vector3Int> _ownedTiles;
        public List<Vector3Int> ownedTiles => _ownedTiles;
        private Player_Identifier _identifer;
        private Player_Identifier identifer => _identifer;

        public Bot(Vector3Int SpawnPos, Player_Identifier Botidentifier)
        {
            _BotPos = SpawnPos;
            _ownedTiles = new List<Vector3Int>()
            {
                new Vector3Int(-6, 2, 0)
            };
            _identifer = Botidentifier;
        }

        public void ChangeBotPos(Vector3Int newPos)
        {
            _BotPos = newPos;
        }
    }
}
