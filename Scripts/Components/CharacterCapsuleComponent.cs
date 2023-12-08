using UnityEngine;

public class CharacterCapsuleComponent : CharacterAbstract
{
    [Header("Components")]
    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    [SerializeField] private Bounds _capsuleBounds;

    private float _groundDistance = 0.05f;
    private float _wallDistance = 0.2f;
    private float _ceilingDistance = 0.05f;
    [SerializeField] private ContactFilter2D _groundFilter;
    private RaycastHit2D[] _groundHit = new RaycastHit2D[5];
    private RaycastHit2D[] _wallHit = new RaycastHit2D[5];
    private RaycastHit2D[] _ceilingHit = new RaycastHit2D[5];
    [SerializeField] private LayerMask _groundLayer;
    private Vector2 _wallDirection => this.baseCharacter.CharacterMesh.bFlipLeft ? Vector2.left : Vector2.right;

    [SerializeField] private bool _bIsFalling;
    [SerializeField] private bool _bIsOnWall;
    [SerializeField] private bool _bIsOnCeiling;
    [SerializeField] private float _fGravityScale = 2.5f;
    [SerializeField] private float _fFallingGravityScale = 4.0f;
    [SerializeField] private float _fReduceGravity = 0.1f;
    private float _fCurrentGravityScale;
    public bool bIsFalling => _bIsFalling;
    public bool bIsOnWall => _bIsOnWall;
    public bool bIsOnCeiling => _bIsOnCeiling;

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
        this.CheckIsOnWall();
        this.CheckIsCeiling();

        this.ChangeGravity();
    }

    private bool CheckIsGround()
    {
        //Bounds _capsuleBounds = this._capsuleCollider.bounds;
        //RaycastHit2D hit = Physics2D.BoxCast(
        //    (Vector2)_capsuleBounds.center + Vector2.down * 1.5f, (Vector2)_capsuleBounds.size, 0, Vector2.down, 0, this._groundLayer);
        //return this._bIsFalling = hit.collider == null;

        return this._bIsFalling = _capsuleCollider.Cast(Vector2.down, _groundFilter, _groundHit, _groundDistance) <= 0;
    }

    private bool CheckIsOnWall()
    {
        //Bounds _capsuleBounds = this._capsuleCollider.bounds;
        //RaycastHit2D hit = Physics2D.CapsuleCast((Vector2)_capsuleBounds.center, (Vector2)_capsuleBounds.size, CapsuleDirection2D.Vertical, 0, this._wallDirection);
        //return this._bIsOnWall = hit.collider != null;

        return this._bIsOnWall = this._capsuleCollider.Cast(this._wallDirection, this._groundFilter, this._wallHit, this._wallDistance) > 0;
    }
    
    private bool CheckIsCeiling()
    {
        return this._bIsOnCeiling = this._capsuleCollider.Cast(Vector2.up, this._groundFilter, this._ceilingHit, this._ceilingDistance) > 0;
    }

    private void FixedUpdate()
    {
        //this.CheckIsGround();
        //this.CheckIsOnWall();
        //this.CheckIsCeiling();

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

/*    private void OnDrawGizmos()
    {
        Bounds CapsuleBounds = this._capsuleCollider.bounds;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(CapsuleBounds.center + Vector3.down * 1.5f, CapsuleBounds.size);
    }*/

}
