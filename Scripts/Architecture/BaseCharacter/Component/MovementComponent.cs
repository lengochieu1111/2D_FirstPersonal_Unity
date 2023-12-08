using MVCS.Architecture.BaseCharacter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementComponent : BaseCharacterAbstract
{
    #region PROPERTTY

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private BaseCharacterController controller;
    [SerializeField] private float _defaultSpeed;
    [SerializeField] private float _airWalkingSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _timeToFlyUp;

    [SerializeField] private bool _isWalking;
    [SerializeField] private bool _isRunning;
    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _isFlying;

    Vector2 _movementDirection;
    private float _movementSpeed;
    private Coroutine flyCoroutine;

    public BaseCharacterController Controller => controller;
    public Rigidbody2D Rigidbody => _rigidbody;

    public bool IsWalking
    {
        get { return this._isWalking; }
        private set 
        { 
            if(value)
            {
                this.MovementSpeed = this.WalkSpeed;
                this.Move();
            }
            else
            {
                this.MovementSpeed = 0;
                this.Idle();
            }

            this._isWalking = value; 
        }
    }
    
    public bool IsRunning
    {
        get { return this._isRunning; }
        private set 
        {
            if (value)
                this.MovementSpeed = this.RunSpeed;
            else
                this.MovementSpeed = this.WalkSpeed;

            this._isRunning = value; 
        }
    }

    public bool IsFlying
    {
        get { return this._isFlying; }
        private set 
        {
            if (value)
            {
                this.Fly();
                this.MovementSpeed = this.AirWalkingSpeed;
            }
            else
                this.MovementSpeed = this.WalkSpeed;

            this._isFlying = value;
        }
    }
    
    public bool IsJumping
    {
        get { return this._isJumping; }
        private set 
        {
            if (value)
            {
                Jump();
                this.MovementSpeed = this.AirWalkingSpeed;
            }
            else
                this.MovementSpeed = this.WalkSpeed;

            this._isJumping = value; 
        }
    }

    public Vector2 MovementDirection
    {
        get { return this._movementDirection; }
        private set { this._movementDirection = value; }
    }

    public float WalkSpeed
    {
        get { return _defaultSpeed; }
        set { this._defaultSpeed = value; }
    }

    public float RunSpeed
    {
        get { return _sprintSpeed; }
        set { this._sprintSpeed = value; }
    }
    
    public float AirWalkingSpeed
    {
        get { return _airWalkingSpeed; }
        set { this._airWalkingSpeed = value; }
    }

    public float MovementSpeed
    {
        get { return _movementSpeed; }
        private set { this._movementSpeed = value; }
    }
    
    public float JumpHeight
    {
        get { return this._jumpHeight; }
        set { this._jumpHeight = value; }
    }
    
    public float TimeToFlyUp
    {
        get { return this._timeToFlyUp; }
        set { this._timeToFlyUp = value; }
    }

    #endregion

    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadController();
        this.LoadRigidbody();
    }

    private void LoadController()
    {
        if (this.controller != null || this.character == null) return;

        this.controller = this.character.Controller;
    }

    protected virtual void LoadRigidbody()
    {
        if (this._rigidbody != null) return;

        this._rigidbody = GetComponentInParent<Rigidbody2D>();
    }

    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        if (this.Controller != null)
        {
            this.Controller.Walk += OnWalk;
            this.Controller.Idle += OnIdle;
            this.Controller.Run += OnRun;
            this.Controller.Jump += OnJump;
            this.Controller.Landing += OnLanding;
            this.Controller.Fly += OnFly;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (this.Controller != null)
        {
            this.Controller.Walk -= OnWalk;
            this.Controller.Idle -= OnIdle;
            this.Controller.Run -= OnRun;
            this.Controller.Jump -= OnJump;
            this.Controller.Landing -= OnLanding;
            this.Controller.Fly -= OnFly;

        }
    }

    private void Update()
    {
        this.NotWalkOnWall();
    }

    private void NotWalkOnWall()
    {
        if (this.IsWalking && this.Controller.IsOnWall && this.Controller.IsOnGround == false)
        {
            this.IsWalking = false;
        }
    }

    #region Walk
    private void OnWalk(Vector2 movementDirection)
    {
        if (movementDirection.x == 0) return;

        if (movementDirection.x < 0)
            this.MovementDirection = Vector2.left;
        else
            this.MovementDirection = Vector2.right;

        this.IsWalking = true;
    }

    private void Move()
    {
        if (this.Rigidbody == null) return;
        this.Rigidbody.velocity =
            new Vector2(this.MovementDirection.x * this.MovementSpeed, this.Rigidbody.velocity.y);
    }
    #endregion

    #region Idle
    private void OnIdle()
    {
        this.MovementDirection = Vector2.zero;
        this.IsWalking = false;
        this.IsRunning = false;
    }

    private void Idle()
    {
        if (this.Rigidbody == null) return;
        this.Rigidbody.velocity = Vector2.zero;
    }
    #endregion

    #region Run
    private void OnRun(bool isRunning)
    {
        this.IsRunning = isRunning;
    }
    #endregion

    #region Jump
    private void OnJump()
    {
        this.IsJumping = true;
    }

    private void Jump()
    {
        if (this.Rigidbody == null) return;
        this.Rigidbody.velocity = new Vector2(this.Rigidbody.velocity.x, this.JumpHeight);
    }
    #endregion

    #region Landing
    private void OnLanding()
    {
        this.IsJumping = false;
        this.IsFlying = false;
    }
    #endregion

    #region Fly
    private void OnFly(bool isFlying)
    {
        this.IsFlying = isFlying;
    }

    private void Fly()
    {
        this.Jump();

        this.flyCoroutine = StartCoroutine(this.FlyReady());
    }

    private IEnumerator FlyReady()
    {
        yield return new WaitForSecondsRealtime(this.TimeToFlyUp);
        this.Rigidbody.velocity = new Vector2(this.Rigidbody.velocity.x, 0);
    }
    #endregion

}
