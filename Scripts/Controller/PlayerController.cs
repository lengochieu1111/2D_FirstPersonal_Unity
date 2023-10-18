using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterController
{
    [Header("Player Input")]
    private DefaultInput _playerInput;

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

        this._playerInput.PlayerInput.Move.performed += OnMovePerformed;
        this._playerInput.PlayerInput.Move.canceled += OnMoveCanceled;

        this._playerInput.PlayerInput.Jump.started += OnJumpStarted;

        this._playerInput.PlayerInput.NormalAttack.started += OnNormalAttackStarted;
        this._playerInput.PlayerInput.StrongAttack.started += OnStrongAttackStarted;

    }

    protected override void OnDisable()
    {
        base.OnDisable();

        this._playerInput.Disable();
        this._playerInput.PlayerInput.Move.performed -= OnMovePerformed;
        this._playerInput.PlayerInput.Move.canceled -= OnMoveCanceled;

        this._playerInput.PlayerInput.Jump.started -= OnJumpStarted;

        this._playerInput.PlayerInput.NormalAttack.started -= OnNormalAttackStarted;
        this._playerInput.PlayerInput.StrongAttack.started -= OnStrongAttackStarted;
    }

    #region Input Action

    private void OnMovePerformed(InputAction.CallbackContext value)
    {
        this.moveInput = value.ReadValue<Vector2>();
        this.HandleMoveInput();
    }

    private void OnMoveCanceled(InputAction.CallbackContext value)
    {
        this.moveInput = Vector2.zero;
        this.HandleMoveInput();
    }

    private void OnJumpStarted(InputAction.CallbackContext value)
    {
        this.HandleJumpInput();
    }

    private void OnNormalAttackStarted(InputAction.CallbackContext value)
    {
        this.HandleNormalAttackInput();
    }

    private void OnStrongAttackStarted(InputAction.CallbackContext value)
    {
        this.HandleStrongAttackInput();
    }

    #endregion

}
