using UnityEngine;

public class CharacterCapsuleComponent : CharacterAbstract
{
    [Header("Components")]
    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _bIsFalling;
    [SerializeField] private float _fGravityScale = 11;
    [SerializeField] private float _fFallingGravityScale = 5;
    [SerializeField] private float _fReduceGravity = 10f;
    private float _fCurrentGravityScale;
    public bool BIsFalling => _bIsFalling;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this._capsuleCollider == null)
            this._capsuleCollider = GetComponent<CapsuleCollider2D>();

    }

    protected override void SetupValues()
    {
        base.SetupValues();
        this._bIsFalling = false;
    }

    private void Update()
    {
        this.CheckIsGround();

        this.ChangeGravity();
    }

    private bool CheckIsGround()
    {
        Bounds CapsuleBounds = this._capsuleCollider.bounds;
        RaycastHit2D hit = Physics2D.BoxCast(CapsuleBounds.center, CapsuleBounds.size, 0, Vector2.down, 0, this._groundLayer);
        return this._bIsFalling = hit.collider == null;
    }

    private void FixedUpdate()
    {
        this.GravityDecreasing();
    }

    private void ChangeGravity()
    {
        if (this._bIsFalling == false)
            this._fCurrentGravityScale = this._fFallingGravityScale;
        else
            this._fCurrentGravityScale = this._fGravityScale;
    }

    private void GravityDecreasing()
    {
        if (this._bIsFalling == true)
            this.baseCharacter.Rigidbody.AddForce(Physics.gravity * (this._fCurrentGravityScale - this._fReduceGravity) * this.baseCharacter.Rigidbody.mass);
    }

}
