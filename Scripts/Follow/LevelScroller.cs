using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum LevelPlaneType
{
    None,
    Background,
    Middleground,
    Foreground
}

public class LevelScroller : RyoMonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private LevelPlaneFollowPlayer _levelPlane;

    [Range(-1.0f, 1.0f)]
    [SerializeField] private float _scrollerSpeed = 0.5f;
    [SerializeField] private bool _isScrolling;
    [SerializeField] private int _scrollDirectin = 1;
    [SerializeField] private LevelPlaneType _levelPlaneType;

    private float _offset;

    public bool isScrolling
    {
        get
        {
            return _isScrolling;
        }
        set 
        { 
            _isScrolling = value; 
        }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadMaterial();
        this.LoadLevelPlane();
    }

    private void LoadLevelPlane()
    {
        if (_levelPlane != null) return;

        _levelPlane = GetComponentInParent<LevelPlaneFollowPlayer>();
    }

    private void LoadMaterial()
    {
        if (_material != null) return;

        _material = GetComponent<Renderer>().material;
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this.LevelPlaneType();

    }

    private void LevelPlaneType()
    {
        int layer = this.gameObject.layer;
        switch(layer)
        {
            case 17:
                this._levelPlaneType = global::LevelPlaneType.Background;
                _scrollerSpeed = 0;
                break;
            case 18:
                this._levelPlaneType = global::LevelPlaneType.Middleground;
                _scrollerSpeed = 0.5f;
                break;
            case 19:
                this._levelPlaneType = global::LevelPlaneType.Foreground;
                _scrollerSpeed = 1f;
                break;
            default:
                this._levelPlaneType = global::LevelPlaneType.None;
                break;
        }
    }

    private void Update()
    {
        if (_levelPlane.Player.MovementComponent.IsWalking
            || this._levelPlaneType == global::LevelPlaneType.Middleground)
        {
            if (this._levelPlaneType != global::LevelPlaneType.Middleground)
            {
                    if (_levelPlane.Player.View.IsFlipLeft)
                    _scrollDirectin = -1;
                else
                    _scrollDirectin = 1;
            }

            isScrolling = true;
        }
        else
            isScrolling = false;
    }

    private void FixedUpdate()
    {
        if (_material != null && isScrolling)
        {
            _offset += (Time.fixedDeltaTime * _scrollerSpeed * _scrollDirectin) / 20f;
            _material.SetTextureOffset("_MainTex", new Vector2(_offset, 0));
        }

    }

}
