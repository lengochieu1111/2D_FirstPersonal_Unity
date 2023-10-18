using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : RyoMonoBehaviour
{
    [Header("HealthBar")]
    [SerializeField] private Image _image;
    [SerializeField] private Text _text;
    [SerializeField] private float healthCurrent;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this._image != null) return;

        Transform healthBarCurrent = this.transform.Find("HealthBarCurrent");
        this._image = healthBarCurrent?.GetComponent<Image>();
        this._text = GetComponentInChildren<Text>();

    }

    public void UpdateHealthBar(float Health, float MaxHealth)
    {
        if (this._image == null) return;
        string text = Health + "/" + MaxHealth;
        this._text.text = text;
        this._image.fillAmount = Health / MaxHealth;
    }

}
