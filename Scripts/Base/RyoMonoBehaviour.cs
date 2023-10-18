using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RyoMonoBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        this.LoadComponents();
    }

    protected virtual void OnEnable()
    {
        this.LoadComponents();
        this.SetupComponents();
        this.SetupValues(); 

    }

    protected virtual void Reset()
    {
        this.LoadComponents();
        this.SetupComponents();
        this.SetupValues();

    }

    protected virtual void Start()
    {

    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void LoadComponents()
    {

    }

    protected virtual void SetupComponents()
    {

    }

    protected virtual void SetupValues()
    {

    }

}
