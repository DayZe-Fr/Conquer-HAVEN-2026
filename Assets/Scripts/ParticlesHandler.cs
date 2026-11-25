using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParticlesHandler : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _tileBurstEmitter;
    private GameObject _gameObject_tileBurstEmmitter;
    private ParticleSystemRenderer _tileBurstEmitterRenderer;



    [SerializeField]
    private Texture2D _FireTex;
    [SerializeField]
    private Texture2D _WaterTex;
    [SerializeField]
    private Texture2D _WindTex;
    [SerializeField]
    private Texture2D _LaysTex;
    [SerializeField]
    private Texture2D _TarTex;

    private Dictionary<TileTypes, Texture2D> _textureDict;


    void Awake()
    {
        _gameObject_tileBurstEmmitter = _tileBurstEmitter.gameObject;
        _tileBurstEmitterRenderer = _gameObject_tileBurstEmmitter.GetComponent<ParticleSystemRenderer>();
        _textureDict = new Dictionary<TileTypes, Texture2D>()
        {
            {TileTypes.Fire, _FireTex},
            {TileTypes.Lays_barbacue_chips, _LaysTex},
            {TileTypes.Water, _WaterTex},
            {TileTypes.Wind, _WindTex},
            {TileTypes.Tar, _TarTex}
        };
    }

    public void EmitTileBurstAtCell(
        int amount,
        TileTypes textureType,
        Vector3Int CellPos
    )
    {
        Texture2D textureToSet = _textureDict[textureType];
        _tileBurstEmitterRenderer.material.mainTexture = textureToSet;

        Vector3 Pos = (Vector3)CellPos + new Vector3(0.5f, 0.5f, -1);

        _gameObject_tileBurstEmmitter.transform.position = Pos;

        _tileBurstEmitter.Emit(amount);    
    }
}
