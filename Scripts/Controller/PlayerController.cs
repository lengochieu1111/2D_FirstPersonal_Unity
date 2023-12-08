using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterController
{
    [Header("Player Input")]
    private DefaultInput _playerInput;

    [SerializeField] private bool _bSprintInput;
   public bool bSprintInput => _bSprintInput;

    protected override void Awake()
    {
        base.Awake();

        /* Player Input */
        this._playerInput = new DefaultInput();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        this._playerInput.Enable();

        this._playerInput.PlayerInput.Walk.performed += OnMovePerformed;
        this._playerInput.PlayerInput.Walk.canceled += OnMoveCanceled;

        this._playerInput.PlayerInput.Jump.started += OnJumpStarted;

        this._playerInput.PlayerInput.Run.performed += OnSprintPerformed;
        this._playerInput.PlayerInput.Run.canceled += OnSprintCanceled;

        this._playerInput.PlayerInput.NormalAttack.started += OnNormalAttackStarted;
        this._playerInput.PlayerInput.StrongAttack.started += OnStrongAttackStarted;

    }

    protected override void OnDisable()
    {
        base.OnDisable();

        this._playerInput.Disable();
        this._playerInput.PlayerInput.Walk.performed -= OnMovePerformed;
        this._playerInput.PlayerInput.Walk.canceled -= OnMoveCanceled;

        this._playerInput.PlayerInput.Jump.started -= OnJumpStarted;

        this._playerInput.PlayerInput.Run.performed -= OnSprintPerformed;
        this._playerInput.PlayerInput.Run.canceled -= OnSprintCanceled;

        this._playerInput.PlayerInput.NormalAttack.started -= OnNormalAttackStarted;
        this._playerInput.PlayerInput.StrongAttack.started -= OnStrongAttackStarted;
    }

    #region Input Action

    private void OnMovePerformed(InputAction.CallbackContext value)
    {
        this.inputValueMove = value.ReadValue<Vector2>().x;
        this.PressMove();
    }

    private void OnMoveCanceled(InputAction.CallbackContext value)
    {
        this.inputValueMove = 0;
        this.ReleaseMove();
    }

    private void OnSprintPerformed(InputAction.CallbackContext value)
    {
        this._bSprintInput = true;
        this.PressSprint();
    }

    private void OnSprintCanceled(InputAction.CallbackContext value)
    {
        this._bSprintInput = false;
        this.ReleaseSprint();
    }

    private void OnJumpStarted(InputAction.CallbackContext value)
    {
        this.PressJump();
    }

    private void OnNormalAttackStarted(InputAction.CallbackContext value)
    {
        this.PressNormalAttack();
    }

    private void OnStrongAttackStarted(InputAction.CallbackContext value)
    {
        this.PressStrongAttack();
    }

    #endregion

}
