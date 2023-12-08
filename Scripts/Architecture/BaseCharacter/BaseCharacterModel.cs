using System;
using System.Collections;
using System.Collections.Generic;
using UMVCS.Architecture;
using UnityEngine;

namespace MVCS.Architecture.BaseCharacter
{
    public class BaseCharacterModel : MVCS_Model<BaseCharacterController>
    {
        [SerializeField] private BaseCharacter baseCharacter;
        [SerializeField] private CharacterSO characterSO;
        public CharacterSO CharacterSO => characterSO;

        #region LoadComponent

        protected override void LoadComponents()
        {
            base.LoadComponents();

            this.LoadBaseCharacter();

            this.LoadController();
            this.LoadCharacterSO();
        }

        protected virtual void LoadBaseCharacter()
        {
            if (this.baseCharacter != null) return;

            this.baseCharacter = GetComponentInParent<BaseCharacter>();
        }

        protected virtual void LoadController()
        {
            if (this.controller != null) return;

            this.controller = this.baseCharacter?.Controller;
        }

        private void LoadCharacterSO()
        {
            string resPath = "SO_" + this.transform.parent?.name;
            this.characterSO = Resources.Load<CharacterSO>(resPath);
        }

        #endregion

    }
}
