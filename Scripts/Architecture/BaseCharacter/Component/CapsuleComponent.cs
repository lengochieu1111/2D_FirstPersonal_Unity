using MVCS.Architecture.BaseCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CapsuleComponent : BaseCharacterAbstract
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    [SerializeField] private BaseCharacterController _controller;
    [SerializeField] private ContactFilter2D _groundFilter;

    private float _groundDistance = 0.05f;
    private float _wallDistance = 0.2f;
    private float _ceilingDistance = 0.05f;
    private RaycastHit2D[] _groundHit = new RaycastHit2D[5];
    private RaycastHit2D[] _wallHit = new RaycastHit2D[5];
    private RaycastHit2D[] _ceilingHit = new RaycastHit2D[5];
    private Vector2 _wallDirection => this.Character.MovementComponent.MovementDirection;

    [SerializeField] private bool _isOnGround = true;
    [SerializeField] private bool _isOnWall;
    [SerializeField] private bool _isOnCeiling;

    [SerializeField] private float _fGravityScale = 0f;
    [SerializeField] private float _fFallingGravityScale = 2f;
    [SerializeField] private float _fReduceGravity = 0.05f;
    private float _fCurrentGravityScale;
    public bool IsOnGround
    {
        get { return _isOnGround; }
        private set { _isOnGround = value; }
    }

    public bool IsOnWall
    {
        get { return _isOnWall; }
        private set { _isOnWall = value; }
    }

    public bool IsOnCeiling
    {
        get { return _isOnCeiling; }
        private set { _isOnCeiling = value; }
    }
    public BaseCharacterController Controller => _controller;


    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCapsuleCollider();
        this.LoadRigidbody();
        this.LoadController();
    }

    private void LoadCapsuleCollider()
    {
        if (this._capsuleCollider != null) return;  
        this._capsuleCollider = GetComponentInParent<CapsuleCollider2D>();
    }

    private void LoadController()
    {
        if (this._controller != null || this.character == null) return;

        this._controller = this.character.Controller;
    }

    private void LoadRigidbody()
    {
        if (this._rigidbody != null) return;

        this._rigidbody = GetComponentInParent<Rigidbody2D>();
    }
    #endregion

    protected override void SetupValues()
    {
        base.SetupValues();
        this.IsOnGround = true;
        this.IsOnWall = false;
        this.IsOnCeiling = false;
        this._groundFilter.useLayerMask = true;
        this._groundFilter.layerMask = LayerMask.GetMask("GroundLayer");
    }

    private void FixedUpdate()
    {
        this.GravityDecreasing();
    }

    private void Update()
    {
        this.CheckIsOnGround();
        this.CheckIsOnWall();
        this.CheckIsCeiling();

        this.ChangeGravity();
    }

    private bool CheckIsOnGround()
    {
        return this.IsOnGround = this._capsuleCollider.Cast(Vector2.down, this._groundFilter, this._groundHit, this._groundDistance) > 0;
    }

    private bool CheckIsOnWall()
    {
        return this.IsOnWall = this._capsuleCollider.Cast(this._wallDirection, this._groundFilter, this._wallHit, this._wallDistance) > 0;
    }

    private bool CheckIsCeiling()
    {
        return this.IsOnCeiling = this._capsuleCollider.Cast(Vector2.up, this._groundFilter, this._ceilingHit, this._ceilingDistance) > 0;
    }

    private void ChangeGravity()
    {
        if (this.IsOnGround || this.Controller.IsFlying)
            this._fCurrentGravityScale = this._fGravityScale;
        else
            this._fCurrentGravityScale = this._fFallingGravityScale;
    }

    private void GravityDecreasing()
    {
        if (this.IsOnGround || this.Controller.IsFlying) return;
        this._rigidbody.AddForce(Physics.gravity * (this._fCurrentGravityScale - this._fReduceGravity) * this._rigidbody.mass);
    }


    /*    private void OnDrawGizmos()
        {
            Bounds CapsuleBounds = this._capsuleCollider.bounds;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(CapsuleBounds.center + Vector3.down * 1.5f, CapsuleBounds.size);
        }*/
}
