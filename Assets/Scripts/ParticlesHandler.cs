using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

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



    [SerializeField]
    private ParticleSystem _moveArrowTrailEmitterTop;
    private ParticleSystemRenderer _topArrowTrailRenderer;
    [SerializeField]
    private ParticleSystem _moveArrowTrailEmitterBottom;
    private ParticleSystemRenderer _bottomArrowTrailRenderer;


    [SerializeField]
    [Range(0, 20)]
    private float _overlayShiftValue = 2;
    [SerializeField]
    [Range(0.01f, 2f)]
    private float _overlayShiftDur = 0.1f;

    [SerializeField]
    private GameObject _overlayCornerTop;
    private Vector3 _overlayCornerTop_Original;

    [SerializeField]
    private GameObject _overlayCornerBottom;
    private Vector3 _overlayCornerBottom_Original;


    void Awake()
    {
        _gameObject_tileBurstEmmitter = _tileBurstEmitter.gameObject;
        _tileBurstEmitterRenderer = _gameObject_tileBurstEmmitter.GetComponent<ParticleSystemRenderer>();

        _topArrowTrailRenderer = _moveArrowTrailEmitterTop.GetComponent<ParticleSystemRenderer>();
        _bottomArrowTrailRenderer = _moveArrowTrailEmitterBottom.GetComponent<ParticleSystemRenderer>();

        _overlayCornerTop_Original = _overlayCornerTop.transform.position;
        _overlayCornerBottom_Original = _overlayCornerBottom.transform.position;

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


    public void ArrowTrailToggle(bool value)
    {
        _bottomArrowTrailRenderer.enabled = value;
        _topArrowTrailRenderer.enabled = value;

        ToggleCorners(value);
    }

    private void ToggleCorners(bool value)
    {
        //note to self: top values, Dev, TOP values, dont frick it up, idiot
        // multiply by -1 to get the bottom value\

        // I TRIED to make it flexible, but nah ima hardcode it, too annoying without a killtween utility
        float shiftValue = 0f;
        if (value) shiftValue = -_overlayShiftValue;


        Vector3 NewPos = new Vector3(
            _overlayCornerTop_Original.x,
            _overlayCornerTop_Original.y + shiftValue,
            _overlayCornerTop_Original.z
        );

        // _overlayCornerTop.transform.position = NewPos;
        DTweenLib.NewTween(
            NewPos, 
            _overlayShiftDur, 
            () => _overlayCornerTop.transform.position, 
            v => _overlayCornerTop.transform.position = v
        );


        shiftValue *= -1;

        NewPos = new Vector3(
            _overlayCornerBottom_Original.x,
            _overlayCornerBottom_Original.y + shiftValue,
            _overlayCornerBottom_Original.z
        );

        // _overlayCornerBottom.transform.position = NewPos;
        DTweenLib.NewTween(
            NewPos, 
            _overlayShiftDur, 
            () => _overlayCornerBottom.transform.position, 
            v => _overlayCornerBottom.transform.position = v
        );

    }
}
