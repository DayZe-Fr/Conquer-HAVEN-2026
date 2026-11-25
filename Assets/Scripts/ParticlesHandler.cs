using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesHandler : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _tileBurstEmitter;
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

    private Dictionary<Player_Identifier, Texture2D> _textureDict;


    void Awake()
    {
        _tileBurstEmitterRenderer = GetComponent<ParticleSystemRenderer>();
        _textureDict = new Dictionary<Player_Identifier, Texture2D>()
        {
            {Player_Identifier.Player1, _FireTex},
            {Player_Identifier.Bot2, _LaysTex},
            {Player_Identifier.Bot3, _WaterTex},
            {Player_Identifier.Bot4, _WindTex},
        };
    }

    public void playTileBurstAtMouse(
        int amount,
        TileTypes textureType
    )
    {
        Texture2D textureToSet;


    }
}
