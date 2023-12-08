using System;
using System.Collections;
using System.Collections.Generic;
using UMVCS.Architecture;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MVCS.Architecture.BaseCharacter
{
    public class BaseCharacterService : MVCS_Service<BaseCharacterController>
    {
        #region PROPERTTY

        [Header("Player Input")]
        private DefaultInput _playerInput;

        [SerializeField] private BaseCharacter baseCharacter;

        [Header("Input Value")]
        [SerializeField] protected Vector2 inputValueMove = Vector2.zero;
        [SerializeField] protected bool isPressingWalkButton = false;
        [SerializeField] protected bool isPressingRunButton = false;
        [SerializeField] protected bool isPressJumpButton = false;
        [SerializeField] protected bool isPressFlyButton = false;
        [SerializeField] protected bool isPressNormalAttackButton = false;
        [SerializeField] protected bool isPressStrongAttackButton = false;

        public event Action<Vector2> PressWalkButton;
        public event Action ReleaseWalkButton;
        public event Action<bool> PressRunButton;
        public event Action PressFlyButton;
        public event Action PressJumpButton;
        public event Action PressNormalAttackButton;
        //public event Action PressStringAttackButton;

        public Vector2 InputValueMove
        {
            get { return this.inputValueMove; }
            private set { this.inputValueMove = value; }
        }

        public bool IsPressingWalkButton
        {
            get { return isPressingWalkButton; }

            private set
            {
                if (value)
                    PressWalkButton?.Invoke(this.InputValueMove);
                else
                    ReleaseWalkButton?.Invoke();

                this.isPressingWalkButton = value;
            }
        }
        
        public bool IsPressingRunButton
        {
            get { return isPressingRunButton; }

            private set
            {
                PressRunButton?.Invoke(value);

                this.isPressingRunButton = value;
            }
        }

        public bool IsPressJumpButton
        {
            get { return isPressJumpButton; }

            private set
            {
                if (value)
                {
                    this.isPressJumpButton = true;
                    PressJumpButton?.Invoke();
                    this.isPressJumpButton = false;
                }
            }
        }
        
        public bool IsPressFlyButton
        {
            get { return this.isPressJumpButton; }

            private set
            {
                if (value)
                {
                    this.isPressFlyButton = true;
                    PressFlyButton?.Invoke();
                    this.isPressJumpButton = false;
                }
            }
        }

        public bool IsPressNormalAttackButton
        {
            get { return isPressNormalAttackButton; }

            private set
            {
                if (value)
                {
                    isPressNormalAttackButton = true;
                    // Normal Attack
                    PressNormalAttackButton?.Invoke();
                    isPressNormalAttackButton = false;
                }
            }
        }

        public bool IsPressStrongAttackButton
        {
            get { return isPressStrongAttackButton; }

            private set
            {
                if (value)
                {
                    isPressStrongAttackButton = true;
                    // Strong Attack
                    isPressStrongAttackButton = false;
                }
            }
        }

        #endregion

        protected override void Awake()
        {
            base.Awake();

            /* Player Input */
            this.CreatePlayerInput();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            this._playerInput.Enable();

            this._playerInput.PlayerInput.Walk.performed += OnWalkPerformed;
            this._playerInput.PlayerInput.Walk.canceled += OnWalkCanceled;

            this._playerInput.PlayerInput.Run.performed += OnRunPerformed;
            this._playerInput.PlayerInput.Run.canceled += OnRunCanceled;

            this._playerInput.PlayerInput.Fly.started += OnFlyStarted;

            this._playerInput.PlayerInput.Jump.started += OnJumpStarted;

            this._playerInput.PlayerInput.NormalAttack.started += OnNormalAttackStarted;
            this._playerInput.PlayerInput.StrongAttack.started += OnStrongAttackStarted;

        }

        protected override void OnDisable()
        {
            base.OnDisable();

            this._playerInput.Disable();

            this._playerInput.PlayerInput.Walk.performed -= OnWalkPerformed;
            this._playerInput.PlayerInput.Walk.canceled -= OnWalkCanceled;

            this._playerInput.PlayerInput.Run.performed -= OnRunPerformed;
            this._playerInput.PlayerInput.Run.canceled -= OnRunCanceled;

            this._playerInput.PlayerInput.Fly.performed -= OnFlyStarted;

            this._playerInput.PlayerInput.Jump.started -= OnJumpStarted;

            this._playerInput.PlayerInput.NormalAttack.started -= OnNormalAttackStarted;
            this._playerInput.PlayerInput.StrongAttack.started -= OnStrongAttackStarted;
        }

        #region Load Component

        protected override void LoadComponents()
        {
            base.LoadComponents();

            this.LoadBaseCharacter();

            this.LoadController();
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

        #endregion

        protected virtual void CreatePlayerInput()
        {
            if (this._playerInput != null) return;

            this._playerInput = new DefaultInput();
        }

        #region Input Action

        protected virtual void OnWalkPerformed(InputAction.CallbackContext value)
        {
            this.InputValueMove = value.ReadValue<Vector2>();
            this.IsPressingWalkButton = true;
        }

        protected virtual void OnWalkCanceled(InputAction.CallbackContext value)
        {
            this.InputValueMove = Vector2.zero;
            this.IsPressingWalkButton = false;
        }

        protected virtual void OnRunPerformed(InputAction.CallbackContext value)
        {
            this.IsPressingRunButton = true;
        }

        protected virtual void OnRunCanceled(InputAction.CallbackContext value)
        {
            this.IsPressingRunButton = false;
        }

        private void OnFlyStarted(InputAction.CallbackContext context)
        {
            this.IsPressFlyButton = true;
        }

        protected virtual void OnJumpStarted(InputAction.CallbackContext value)
        {
            this.IsPressJumpButton = true;
        }

        protected virtual void OnNormalAttackStarted(InputAction.CallbackContext value)
        {
            this.IsPressNormalAttackButton = true;
        }

        protected virtual void OnStrongAttackStarted(InputAction.CallbackContext value)
        {
            this.IsPressStrongAttackButton = true;
        }

        #endregion

    }
}
