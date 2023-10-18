﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : CharacterController
{
    [Header("AI Controller")]
    [SerializeField] private EnemyCharacter _enemyCharacter;
    [SerializeField] private EAIState _AIState;
    [SerializeField] private float _fTargetDistance = 1;
    [SerializeField] private Transform _targetLocation;

    [Header("Patrol State")]
    [SerializeField] private float _fSightRadius;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private List<Transform> _patrolPoints;
    [SerializeField] private int _patrolIndex;

    [Header("Patrol State")]
    [SerializeField] private BaseCharacter _playerCharacter;

    public EAIState AIState => _AIState;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this._enemyCharacter = this.baseCharacter.GetComponent<EnemyCharacter>();
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this._AIState = EAIState.Patrol;
        this._patrolIndex = 0;
        this._targetLocation = this._patrolPoints[0];

        this._fSightRadius = this.baseCharacter.CharacterSO.FSightRadius;
        this._targetLayer = this.baseCharacter.CharacterAttack.TracerLayer;
    }

    protected override void Start()
    {
        base.Start();

        this.MoveToTarget();
    }

    private void Update()
    {
        if (this._AIState == EAIState.Attack)
            this.Attack();
        else if (this._AIState == EAIState.Regen)
            this.Regen();
        else if (this._AIState == EAIState.Combat)
            this.Combat();
        else
            this.Patrol();

    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.CircleCast(this.transform.position, this._fSightRadius, Vector2.left, 0, this._targetLayer);

        if (hit.collider == null) return false;
        this._playerCharacter = hit.collider.GetComponentInParent<BaseCharacter>();

        if (this._playerCharacter == null) return false;
        this._targetLocation = this._playerCharacter?.transform;

        return true;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;

    //    Gizmos.DrawWireSphere(this.transform.position, this._fSightRadius);
    //}

    private void Patrol()
    {
        if (this.ArrivedTheTargetLocation())
        {
            this._patrolIndex = (this._patrolIndex + 1) % this._patrolPoints.Count;
            this._targetLocation = this._patrolPoints[this._patrolIndex];
            this.MoveToTarget();
        }

        if (this.PlayerInSight())
        {
            this._AIState = EAIState.Combat;
            this._targetLocation = this._playerCharacter?.transform;
            this.MoveToTarget();
            this._enemyCharacter?.I_HandleSeePlayer(this._playerCharacter);
        }
    }
    
    private void Combat()
    {
        if (this.ArrivedTheTargetLocation() == true)
            this._AIState = EAIState.Attack;
        else
            this.MoveToTarget();

    }

    private bool ArrivedTheTargetLocation()
    {
        float distanceToPlayer = Vector2.Distance(this.transform.position, this._targetLocation.position);

        if (distanceToPlayer <= this._fTargetDistance)
        {
            this.moveInput = Vector2.zero;
            this.HandleMoveInput();
        }

        return distanceToPlayer <= this._fTargetDistance;
    }

    private void MoveToTarget()
    {
        if (this.transform.position.x < this._targetLocation.position.x && this.moveInput == Vector2.right) return;
        if (this.transform.position.x > this._targetLocation.position.x && this.moveInput == Vector2.left) return;

        if (this.transform.position.x < this._targetLocation.position.x)
        {            
            this.moveInput = Vector2.right;
            this.HandleMoveInput();
        }
        else
        {
            this.moveInput = Vector2.left;
            this.HandleMoveInput();
        }
    }

    private void Regen()
    {

    }
    
    private void Attack()
    {
        if (this.ArrivedTheTargetLocation() == false)
        {
            this._AIState = EAIState.Combat;
            this.MoveToTarget();
        }
        else
            this.baseCharacter.HandlePressedAttack(EAttackType.Normal);
    }

}