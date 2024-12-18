using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRenderer : AnimatorRenderer, IEntityComponent
{
    protected Entity _entity;

    [field: SerializeField] public float FacingDirection { get; private set; } = 1;
    public void Initialize(Entity entity)
    {
        _entity = entity;
    }

    #region Flip Controller
    public void Flip()
    {
        FacingDirection *= -1;
        _entity.transform.Rotate(0, 180f, 0);
    }

    public void FlipController(float normalizedXMove)
    {
        if (Mathf.Abs(FacingDirection + normalizedXMove) < 0.5f)
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
