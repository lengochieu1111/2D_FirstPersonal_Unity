using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWidget : RyoMonoBehaviour
{
    private static PlayerWidget instance;
    public static PlayerWidget Instance => instance;

    [Header("HealthBar")]
    [SerializeField] private HealthBar _healthBar_Player;
    [SerializeField] private HealthBar _healthBar_Enemy;

    protected override void Awake()
    {
        if (PlayerWidget.instance != null) return;
        PlayerWidget.instance = this;

        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        this.SetActiveHealthBar_Enemy(false);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this._healthBar_Player == null)
        {
            Transform healthBar_Player = this.transform.Find("HealthBar_Player");
            this._healthBar_Player = healthBar_Player?.GetComponent<HealthBar>();
        }
        
        if (this._healthBar_Enemy == null)
        {
            Transform healthBar_Enemy = this.transform.Find("HealthBar_Enemy");
            this._healthBar_Enemy = healthBar_Enemy?.GetComponent<HealthBar>();
        }

    }

    public void UpdateHealthBar_Player(float Health, float MaxHealth)
    {
        this._healthBar_Player?.UpdateHealthBar(Health, MaxHealth);
    }
    
    public void UpdateHealthBar_Enemy(float Health, float MaxHealth)
    {
        this._healthBar_Enemy?.UpdateHealthBar(Health, MaxHealth);
    }

    public void SetActiveHealthBar_Enemy(bool active)
    {
        this._healthBar_Enemy?.gameObject.SetActive(active);
    }

}
