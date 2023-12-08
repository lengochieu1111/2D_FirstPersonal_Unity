using System.Collections;
using System.Collections.Generic;
using UMVCS.Architecture;
using UnityEngine;

namespace UMVCS.Architecture
{
    public abstract class MVCS_View<Controller> : RyoMonoBehaviour
    {
        [SerializeField] protected Controller controller;

        /*#region LoadComponent

        protected override void LoadComponents()
        {
            base.LoadComponents();

            this.LoadController();
        }

        protected virtual void LoadController()
        {
            if (this.controller != null) return;

            this.controller = GetComponentInParent<Controller>();
        }

        #endregion*/
    }
}
