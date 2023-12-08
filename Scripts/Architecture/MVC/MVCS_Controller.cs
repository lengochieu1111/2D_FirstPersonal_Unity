using System.Collections;
using System.Collections.Generic;
using MVCS.Architecture.BaseCharacter;
using UnityEngine;

namespace UMVCS.Architecture
{
    public abstract class MVCS_Controller<Model, View, Service> : RyoMonoBehaviour
    {
        [Header("MVCS")]
        [SerializeField] protected Model model;
        [SerializeField] protected View view;
        [SerializeField] protected Service service;

    }

}
