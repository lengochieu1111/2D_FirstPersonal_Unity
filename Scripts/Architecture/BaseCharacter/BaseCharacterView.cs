using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UMVCS.Architecture;
using Unity.VisualScripting;
using UnityEngine;

namespace MVCS.Architecture.BaseCharacter
{
    public class BaseCharacterView : MVCS_View<BaseCharacterController>
    {
        #region PROPERTY

        [Header("Component")]
        [SerializeField] protected BaseCharacter _baseCharacter;
        [SerializeField] protected Animator animator;
        [SerializeField] protected Rigidbody2D rigidbody_;
        [SerializeField] protected CapsuleCollider2D capsuleCollider;
        [Header("Property")]
        [SerializeField] protected bool isFlipLeft;
        [SerializeField] protected float landingHeight = 2f;
        [SerializeField] protected LayerMask _groundLayer;
        public BaseCharacterController Controller => controller;
        public Animator Animator => animator;
        public Rigidbody2D Rigidbody => rigidbody_;
        public CapsuleCollider2D CapsuleCollider => capsuleCollider;
        public LayerMask GroundLayer
        { 
            get { return this._groundLayer; }
            set { this._groundLayer = value; }
        }

        public float LandingHeight => landingHeight;

        public bool IsFlipLeft
        {
            get { return this.isFlipLeft; }

            set 
            { 
                this.isFlipLeft = value;

                this.FlipMesh();
            }
        }
        #endregion

        #region 
        protected override void OnEnable()
        {
            base.OnEnable();

            if (this.controller != null)
            {
                this.controller.Walk += OnWalk;
                this.controller.Idle += OnIdle;
                this.controller.Run += OnRun;
                this.controller.Jump += OnJump;
                this.Controller.Fly += OnFly;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (this.controller != null)
            {
                this.controller.Walk -= OnWalk;
                this.controller.Idle -= OnIdle;
                this.controller.Run -= OnRun;
                this.controller.Jump -= OnJump;
                this.Controller.Fly -= OnFly;
            }
        }
        #endregion

        #region LoadComponent

        protected override void LoadComponents()
        {
            base.LoadComponents();

            this.LoadBaseCharacter();

            this.LoadController();
            this.LoadRigidbody();
            this.LoadCapsuleCollider();
            this.LoadAnimator();
        }

        private void LoadCapsuleCollider()
        {
            if (this.capsuleCollider != null) return;

            this.capsuleCollider = GetComponentInParent<CapsuleCollider2D>();
        }

        protected virtual void LoadRigidbody()
        {
            if(this.rigidbody_ != null) return;

            this.rigidbody_ = GetComponentInParent<Rigidbody2D>();
        }

        protected virtual void LoadBaseCharacter()
        {
            if (this._baseCharacter != null) return;

            this._baseCharacter = GetComponentInParent<BaseCharacter>();
        }

        protected virtual void LoadController()
        {
            if (this.controller != null) return;

            this.controller = this._baseCharacter?.Controller;
        }
        
        protected virtual void LoadAnimator()
        {
            if (this.animator != null) return;

            this.animator = GetComponent<Animator>();
        }

        #endregion

        #region Setup
        protected override void SetupValues()
        {
            base.SetupValues();

            this.GroundLayer = LayerMask.GetMask("GroundLayer");
        }
        #endregion

        #region Walk
        private void OnWalk(Vector2 movementDirection)
        {
            if (movementDirection.x == 0) return;
            this.WalkAnimation();

            if (movementDirection.x < 0)
                this.IsFlipLeft = true;
            else if (movementDirection.x > 0)
                this.IsFlipLeft = false;

        }
        private void WalkAnimation()
        {
            this.Animator.SetBool(AnimationString.isWalking, true);
        }
        #endregion

        #region Idle
        private void OnIdle()
        {
            this.IdleAnimation();
            this.RunAnimation(false);
        }

        private void IdleAnimation()
        {
            this.Animator.SetBool(AnimationString.isWalking, false);
        }
        #endregion

        #region Run
        private void OnRun(bool isRunning)
        {
            this.RunAnimation(isRunning);
        }

        private void RunAnimation(bool isRunning)
        {
            this.Animator.SetBool(AnimationString.isRunning, isRunning);
        }
        #endregion

        #region Jump
        private void OnJump()
        {
            this.JumpAnimation();
        }

        private void JumpAnimation()
        {
            if (this.Animator == null) return;
            this.Animator.SetTrigger(AnimationString.onAirTrigger);
            this.Animator.SetBool(AnimationString.isJumping, true);
        }

        public void EA_JumpStart()
        {
            if(this.Animator == null) return;
            this.Animator.SetBool(AnimationString.isFalling, false);
            this.Animator.SetBool(AnimationString.isLanding, false);
        }
        
        public void EA_Landing()
        {
            if(this.Animator == null) return;
            this.Animator.SetBool(AnimationString.isJumping, false);
            this.Animator.SetBool(AnimationString.isFlying, false);
        }
        
        public void EA_RisingToFalling()
        {
            if(this.Animator == null) return;
            this.Animator.SetBool(AnimationString.isFalling, this.Rigidbody.velocity.y < 0.05f);
        }

        public void EA_FallingToLanding()
        {
            if (this.Animator == null || this.CapsuleCollider == null) return;
            RaycastHit2D hit = Physics2D.BoxCast((Vector2)this.CapsuleCollider.bounds.center ,
                (Vector2)this.CapsuleCollider.bounds.size, 0, Vector2.down, this.LandingHeight, this.GroundLayer);

            this.Animator.SetBool(AnimationString.isLanding, hit.collider != null);
        }
        #endregion

        private void FlipMesh()
        {
            float fAngle;
            if (this.IsFlipLeft)
                fAngle = -180;
            else
                fAngle = 0;

            Vector3 rotator = new Vector3(this.transform.rotation.x, fAngle, this.transform.rotation.z);
            this.transform.rotation = Quaternion.Euler(rotator);
        }

        #region Fly
        private void OnFly(bool isFlying)
        {
            if (isFlying)
            {
                this.Animator.SetTrigger(AnimationString.onAirTrigger);
            }

            this.FlyAnimation(isFlying);
        }

        private void FlyAnimation(bool isFlying)
        {
            this.Animator.SetBool(AnimationString.isFlying, isFlying);
        }
        #endregion

    }
}
