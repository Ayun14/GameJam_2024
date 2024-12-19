using DG.Tweening;
using System;
using UnityEngine;

public class EntityMover : MonoBehaviour, IEntityComponent, IAfterInitable
{
    public event Action<Vector2> OnMovement;

    [Header("Collision detect")]
    [SerializeField] protected Transform _groundCheckerTrm;
    [SerializeField] protected Vector2 _checkerSize;
    [SerializeField] protected float _checkDistance;
    [SerializeField] protected LayerMask _whatIsGround;

    private Rigidbody2D _rbCompo;
    private EntityRenderer _renderer;

    private float _moveSpeed = 6f;
    private float _movementX;
    private float _moveSpeedMultiplier, _originalGravity;
    [field: SerializeField] public bool CanManualMove { get; set; } = true;

    private Entity _entity;

    public void Initialize(Entity entity)
    {
        _entity = entity;
        _rbCompo = entity.GetComponent<Rigidbody2D>();
        _renderer = entity.GetCompo<EntityRenderer>();

        _originalGravity = _rbCompo.gravityScale;
        _moveSpeedMultiplier = 1f;
    }

    public void AfterInitialize()
    {
    }

    private void FixedUpdate()
    {
        if (CanManualMove)
        {
            Vector2 velocity = _rbCompo.velocity;
            velocity.x = _movementX * _moveSpeed * _moveSpeedMultiplier;
            _rbCompo.velocity = velocity;
        }

        OnMovement?.Invoke(_rbCompo.velocity);

        SetManualMove();
    }

    private void SetManualMove()
    {
        if (CanManualMove == false)
        {
            Vector2 velocity = _rbCompo.velocity;
            if (velocity.x < 0.5f)
            {
                CanManualMove = true;
            }
        }
    }

    public void SetMovement(float xMovement)
    {
        _movementX = xMovement;
        _renderer.FlipController(_movementX);
    }

    public void StopImmediately(bool isYAxisToo = false)
    {
        if (isYAxisToo)
        {
            _rbCompo.velocity = Vector2.zero;
        }
        else
        {
            Vector2 velocity = _rbCompo.velocity;
            velocity.x = 0;
            _rbCompo.velocity = velocity;
        }
        _movementX = 0;
    }

    public void SetMoveSpeedMultiplier(float value) => _moveSpeedMultiplier = value;
    public void SetGravityScale(float value) => _rbCompo.gravityScale = value;

    public void AddForceToEntity(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse)
    {
        _rbCompo.AddForce(force, mode);
    }

    #region KnockBack

    public void KnockBack(Vector2 force)
    {
        _entity.OnKnockBackEvent?.Invoke(force);

        CanManualMove = false;
        StopImmediately(true);
        AddForceToEntity(force);
    }

    #endregion

    #region CheckCollision

    public virtual bool IsGroundDetected()
        => Physics2D.BoxCast(_groundCheckerTrm.position, _checkerSize, 0,
            Vector2.down, _checkDistance, _whatIsGround);

    #endregion

#if UNITY_EDITOR

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_groundCheckerTrm != null)
        {
            Gizmos.DrawWireCube(_groundCheckerTrm.position - new Vector3(0, _checkDistance * 0.5f),
                new Vector3(_checkerSize.x, _checkDistance, 1f));
        }
    }

#endif
}
