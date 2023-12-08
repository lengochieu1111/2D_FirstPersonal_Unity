using MVCS.Architecture.BaseCharacter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVCS.Architecture.BaseCharacter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseCharacter : MVCS_Base
    <BaseCharacterModel,
    BaseCharacterController,
    BaseCharacterView,
    BaseCharacterService>
    {
        [Header("Component")]
        [SerializeField] protected CapsuleComponent capsuleComponent;
        [SerializeField] protected MovementComponent movementComponent;
        [SerializeField] protected HealthComponent healthComponent;
        [SerializeField] protected AttackComponent attackComponent;

        public BaseCharacterModel Model => model;
        public BaseCharacterController Controller => controller;
        public BaseCharacterView View => view;
        public BaseCharacterService Service => service;

        public CapsuleComponent CapsuleComponent => capsuleComponent;
        public MovementComponent MovementComponent => movementComponent;
        public HealthComponent HealthComponent => healthComponent;
        public AttackComponent AttackComponent => attackComponent;

        #region LoadComponent

        protected override void LoadComponents()
        {
            base.LoadComponents();

            this.LoadModel();
            this.LoadController();
            this.LoadView();
            this.LoadService();

            this.LoadCapsuleComponent();
            this.LoadMovementComponent();
            this.LoadHealthComponent();
            this.LoadAttackComponent();
        }

        private void LoadController()
        {
            if (this.controller != null) return;

            this.controller = GetComponentInChildren<BaseCharacterController>();
        }

        protected virtual void LoadService()
        {
            if (this.model != null) return;

            this.model = GetComponentInChildren<BaseCharacterModel>();
        }

        protected virtual void LoadView()
        {
            if (this.view != null) return;

            this.view = GetComponentInChildren<BaseCharacterView>();
        }

        protected virtual void LoadModel()
        {
            if (this.service != null) return;

            this.service = GetComponentInChildren<BaseCharacterService>();
        }

        protected virtual void LoadCapsuleComponent()
        {
            if (this.capsuleComponent != null) return;

            this.capsuleComponent = GetComponentInChildren<CapsuleComponent>();
        }

        protected virtual void LoadAttackComponent()
        {
            if (this.attackComponent != null) return;

            this.attackComponent = GetComponentInChildren<AttackComponent>();
        }

        protected virtual void LoadHealthComponent()
        {
            if (this.healthComponent != null) return;

            this.healthComponent = GetComponentInChildren<HealthComponent>();
        }

        protected virtual void LoadMovementComponent()
        {
            if (this.movementComponent != null) return;

            this.movementComponent = GetComponentInChildren<MovementComponent>();
        }

        protected override void SetupComponents()
        {
            base.SetupComponents();

            //this.movementComponent.MovementSpeed = this.model.DefaulSpeed;
        }

        #endregion

    }
}
