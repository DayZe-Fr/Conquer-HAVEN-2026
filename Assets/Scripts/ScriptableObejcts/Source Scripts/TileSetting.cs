using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileSetting", menuName = "Data/TileSetting")]
public class TileSetting : ScriptableObject
{
    [SerializeField]
    private TileTypes _type;
    public TileTypes Type => _type;

    [SerializeField]
    private Player_Identifier _accociate;
    public Player_Identifier Accociate => _accociate;

    [SerializeField]
    private Texture2D _texture;
    public Texture2D Texture => _texture;

    [SerializeField]
    private Tile _tile;
    public Tile Tile => _tile;
}
