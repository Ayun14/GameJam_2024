using UnityEngine;

public class EntityRenderer : AnimatorRenderer, IEntityComponent
{
    protected SpriteRenderer _renderer;

    [field: SerializeField] public float FacingDirection { get; private set; } = 1;
    public void Initialize(Entity entity)
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    #region Flip Controller
    public void Flip()
    {
        FacingDirection *= -1;
        _renderer.flipX = FacingDirection == 1f ? false : true;

    }

    public void FlipController(float normalizedXMove)
    {
        if (Mathf.Abs(FacingDirection + normalizedXMove) < 1f)
            Flip();
    }

    #endregion

    #region Jump Controller

    public void Jump(AnimatorParamSO param, float count)
    {
        SetParam(param, count);
    }

    #endregion
}
