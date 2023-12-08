using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MVCS_Base<Model, Controller, View, Service> : RyoMonoBehaviour
{
    [Header("MVCS")]
    [SerializeField] protected Model model;
    [SerializeField] protected Controller controller;
    [SerializeField] protected View view;
    [SerializeField] protected Service service;

}
