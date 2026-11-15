using System.Collections.Generic;
using UnityEngine;

public class BotsHandler : MonoBehaviour
{
    



    public void Init()
    {
        
    }

    private class Bot
    {
        private static Vector3Int _BotPos;
        public static Vector3Int BotPos => _BotPos;
        private static List<Vector3Int> _ownedTiles;
        public static List<Vector3Int> ownedTiles => _ownedTiles;
        private Player_Identifier _identifer;
        private Player_Identifier identifer => _identifer;

        public Bot(Vector3Int SpawnPos, Player_Identifier Botidentifier)
        {
            _BotPos = new Vector3Int(-6, 2, 0);
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
