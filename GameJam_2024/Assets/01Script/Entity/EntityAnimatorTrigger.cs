using System;
using UnityEngine;

public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
{
    public event Action OnAnimationEnd;
    public event Action OnAttackTrigger;

    protected Entity _entity;

    public void Initialize(Entity entity)
    {
        _entity = entity;
    }

    protected virtual void AnimationEnd() => OnAnimationEnd?.Invoke();
    protected virtual void AttackTrigger() => OnAttackTrigger?.Invoke();
}
