using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UMVCS.Architecture;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MVCS.Architecture.BaseCharacter
{
    public class BaseCharacterController : MVCS_Controller<BaseCharacterModel, BaseCharacterView, BaseCharacterService>
    {
        public event Action<Vector2> Walk;
        public event Action Idle;
        public event Action<bool> Run;
        public event Action Jump;
        public event Action Landing;
        public event Action<bool> Fly;

        [Header("Component")]
        [SerializeField] protected BaseCharacter character;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private CapsuleCollider2D capsuleCollider;
        [SerializeField] protected CapsuleComponent capsuleComponent;
        [SerializeField] protected MovementComponent movementComponent;
        [SerializeField] protected HealthComponent healthComponent;
        [SerializeField] protected AttackComponent attackComponent;

        [Header("Move")]
        [SerializeField] protected bool canJump = true;
        [SerializeField] protected bool canFly = true;
        [SerializeField] protected float accelerationTime = 0.15f;
        [SerializeField] protected float nextJumpDelayTime = 0.2f;
        [SerializeField] protected float nextFlightDelayTime = 0.2f;
        [SerializeField] protected LayerMask groundLayer;
        private Coroutine moveCoroutine;
        private Coroutine jumpCoroutine;
        private Coroutine flyCoroutine;
        public float AccelerationTime => accelerationTime;
        public float NextJumpDelayTime => nextJumpDelayTime;
        public float NextFlightDelayTime => nextFlightDelayTime;
        private bool CanJump
        {
            get { return this.canJump; }
            set { this.canJump = value; }
        }
        
        private bool CanFly
        {
            get { return this.canFly; }
            set { this.canFly = value; }
        }

        public BaseCharacterModel CharacterModel => model;
        public BaseCharacterView CharacterView => view;
        public BaseCharacterService CharacterService => service;
        public Rigidbody2D Rigidbody => _rigidbody;
        public CapsuleCollider2D CapsuleCollider => capsuleCollider;
        public CapsuleComponent CapsuleComponent => capsuleComponent;
        public MovementComponent MovementComponent => movementComponent;

        /*
         * Service
        */
        public Vector2 InputValueMove => this.CharacterService.InputValueMove;
        public bool IsPressingWalkButton => this.CharacterService.IsPressingWalkButton;
        public bool IsPressingRunButton => this.CharacterService.IsPressingRunButton;

        /*
         * Movement
        */
        public bool IsWalking => this.MovementComponent.IsWalking;
        public bool IsRunning => this.MovementComponent.IsRunning;
        public bool IsJumping => this.MovementComponent.IsJumping;
        public bool IsFlying => this.MovementComponent.IsFlying;

        /*
         * Capsule
        */
        public bool IsOnGround => this.CapsuleComponent.IsOnGround;
        public bool IsRising => this.Rigidbody.velocity.y > 0.001f;
        public bool IsFalling => this.Rigidbody.velocity.y < -0.001f;
        public bool IsOnWall => this.CapsuleComponent.IsOnWall;
        public bool IsOnCeiling => this.CapsuleComponent.IsOnCeiling;

        #region LoadComponent

        protected override void LoadComponents()
        {
            base.LoadComponents();

            this.LoadBaseCharacter();
            this.LoadRigidbody();
            this.LoadCapsuleCollider();

            this.LoadModel();
            this.LoadView();
            this.LoadService();

            this.LoadCapsuleComponent();
            this.LoadMovementComponent();
            this.LoadHealthComponent();
            this.LoadAttackComponent();
        }

        protected virtual void LoadBaseCharacter()
        {
            if (this.character != null) return;

            this.character = GetComponentInParent<BaseCharacter>();
        }
        protected virtual void LoadRigidbody()
        {
            if (this._rigidbody != null) return;

            this._rigidbody = GetComponentInParent<Rigidbody2D>();
        }
        private void LoadCapsuleCollider()
        {
            if (this.capsuleCollider != null) return;
            this.capsuleCollider = GetComponentInParent<CapsuleCollider2D>();
        }

        protected virtual void LoadService()
        {
            if (this.model != null) return;

            this.model = this.character?.Model;
        }

        protected virtual void LoadView()
        {
            if (this.view != null) return;

            this.view = this.character?.View;
        }

        protected virtual void LoadModel()
        {
            if (this.service != null) return;

            this.service = this.character?.Service;
        }

        protected virtual void LoadCapsuleComponent()
        {
            if (this.capsuleComponent != null) return;

            this.capsuleComponent = this.character?.CapsuleComponent;
        }

        protected virtual void LoadAttackComponent()
        {
            if (this.attackComponent != null) return;

            this.attackComponent = this.character?.AttackComponent;
        }

        protected virtual void LoadHealthComponent()
        {
            if (this.healthComponent != null) return;

            this.healthComponent = this.character?.HealthComponent;
        }

        protected virtual void LoadMovementComponent()
        {
            if (this.movementComponent != null) return;

            this.movementComponent = this.character?.MovementComponent;
        }

        #endregion

        #region Setup Component
        protected override void SetupComponents()
        {
            base.SetupComponents();

            this.SetupMovementComponent();
        }

        private void SetupMovementComponent()
        {
            if(this.MovementComponent == null || this.CharacterModel.CharacterSO == null) return;

            this.MovementComponent.WalkSpeed = this.CharacterModel.CharacterSO.DefaulSpeed;
            this.MovementComponent.RunSpeed = this.CharacterModel.CharacterSO.RunSpeed;
            this.MovementComponent.AirWalkingSpeed = this.CharacterModel.CharacterSO.AirWalkingSpeed;
            this.MovementComponent.JumpHeight = this.CharacterModel.CharacterSO.JumpHeight;
            this.MovementComponent.TimeToFlyUp = this.CharacterModel.CharacterSO.TimeToFlyUp;
        }

        protected override void SetupValues()
        {
            base.SetupValues();
            this.groundLayer = LayerMask.GetMask("GroundLayer");
            this.Rigidbody.gravityScale = 0.0f;
            this.Rigidbody.freezeRotation = true;
        }
        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();

            if (this.CharacterService != null)
            {
                this.CharacterService.PressWalkButton += HandlePressWalkButton;
                this.CharacterService.ReleaseWalkButton += HandleReleaseMoveButton;
                
                this.CharacterService.PressRunButton += HandlePressSprintButton;

                this.CharacterService.PressJumpButton += HandlePressJumpButton;
                this.CharacterService.PressFlyButton += HandlePressFlyButton;
                this.Landing += OnLanding;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (this.CharacterService != null)
            {
                this.CharacterService.PressWalkButton -= HandlePressWalkButton;
                this.CharacterService.ReleaseWalkButton -= HandleReleaseMoveButton;

                this.CharacterService.PressRunButton -= HandlePressSprintButton;

                this.CharacterService.PressJumpButton -= HandlePressJumpButton;
                this.CharacterService.PressFlyButton -= HandlePressFlyButton;

                this.Landing -= OnLanding;
            }
        }

        private void Update()
        {
            this.LadingCheck();

        }

        #region Walk

        private void HandlePressWalkButton(Vector2 moveDirection)
        {
            this.RequestWalk(moveDirection);
        }

        private void RequestWalk(Vector2 moveDirection, bool isAcceleration = true)
        {
            if (isAcceleration)
                this.moveCoroutine = StartCoroutine(this.ReadyToWalk(this.AccelerationTime));
            else
                this.moveCoroutine = StartCoroutine(this.ReadyToWalk(0));
        }

        private IEnumerator ReadyToWalk(float accelerationTime)
        {
            yield return new WaitForSecondsRealtime(accelerationTime);

            if (this.IsPressingWalkButton)
            {
                this.Walk?.Invoke(this.InputValueMove);
                this.Run?.Invoke(this.IsPressingRunButton);
            }
        }

        private void HandleReleaseMoveButton()
        {
            if (this.IsJumping) return;

            this.RequestIdle();
        }

        private void RequestIdle()
        {
            this.Idle?.Invoke();
        }

        #endregion

        #region Run

        private void HandlePressSprintButton(bool isPressingSprintButton)
        {
            if(this.IsWalking)
            {
                Run?.Invoke(isPressingSprintButton);
            }
        }

        #endregion

        #region Jump
        private void HandlePressJumpButton()
        {
            if (this.CanJump && this.IsOnGround)
            {
                Jump?.Invoke();
                this.CanJump = false;
            }

        }
        #endregion

        #region Landing
        private void LadingCheck()
        {
            if (this.IsFalling ==  false) return;

            RaycastHit2D hit = Physics2D.BoxCast((Vector2)this.CapsuleCollider.bounds.center,
                (Vector2)this.CapsuleCollider.bounds.size, 0, Vector2.down, 0.05f, this.groundLayer);

            if (hit.collider != null)
            {
                this.Landing?.Invoke();
            }
        }

        private void OnLanding()
        {
            if (this.IsPressingWalkButton)
                this.RequestWalk(this.InputValueMove, false);
            else
                this.RequestIdle();

            this.jumpCoroutine = StartCoroutine(this.ReadyToJump());
            this.flyCoroutine = StartCoroutine(this.ReadyToFly());
        }

        private IEnumerator ReadyToJump()
        {
            yield return new WaitForSecondsRealtime(this.NextJumpDelayTime);

            this.CanJump = true;
        }
        
        private IEnumerator ReadyToFly()
        {
            yield return new WaitForSecondsRealtime(this.NextFlightDelayTime);

            this.CanFly = true;
        }

        #endregion

        #region Fly
        private void HandlePressFlyButton()
        {
            if (this.IsJumping || this.IsFlying || this.CanFly == false) return;
            Fly?.Invoke(true);
            this.CanFly = false;

            StartCoroutine(this.FlyDown());
        }

        private IEnumerator FlyDown()
        {
            yield return new WaitForSecondsRealtime(4f);

            Fly?.Invoke(false);
        }
        #endregion


    }
}
