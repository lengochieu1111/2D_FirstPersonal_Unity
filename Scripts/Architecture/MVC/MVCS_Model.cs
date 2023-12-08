using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UMVCS.Architecture
{
    public abstract class MVCS_Model<Controller> : RyoMonoBehaviour
    {
        [SerializeField] protected Controller controller;

    }
}
