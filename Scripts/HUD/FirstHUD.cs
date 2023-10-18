using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstHUD : RyoMonoBehaviour
{
    private static FirstHUD instance;
    public static FirstHUD Intance => instance;

    [Header("Widget")]
    [SerializeField] private PlayerWidget _playerWidget;
    public PlayerWidget PlayerWidget => _playerWidget;


    protected override void Awake()
    {
        if (FirstHUD.instance != null) return;

        FirstHUD.instance = this;
        base.Awake();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this._playerWidget == null)
            this._playerWidget = GetComponentInChildren<PlayerWidget>();
    }

}
