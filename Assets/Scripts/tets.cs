using UnityEngine;
using UnityEngine.Tilemaps;

public class tets : MonoBehaviour
{
    public Tilemap map;
    public TileBase tiletoplace;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3Int loc = new Vector3Int(0, 0, 0);
        map.SetTile(loc, tiletoplace);
    }
}
