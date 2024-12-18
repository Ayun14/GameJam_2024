public abstract class EntityState
{
    protected Entity _entity;

    protected AnimatorParamSO _stateParam;
    protected bool _isTriggered;

    protected EntityRenderer _renderer;
    protected EntityAnimatorTrigger _animatorTrigger;

    public EntityState(Entity entity, AnimatorParamSO stateParam)
    {
        _entity = entity;
        _stateParam = stateParam;
        _renderer = entity.GetCompo<EntityRenderer>();
        _animatorTrigger = entity.GetCompo<EntityAnimatorTrigger>();
    }

    public virtual void Enter()
    {
        _renderer.SetParam(_stateParam, true);
        _isTriggered = false;
        _animatorTrigger.OnAnimationEnd += AnimationEndTrigger;
    }

    public virtual void Update() { }

    public virtual void Exit()
    {
        _renderer.SetParam(_stateParam, false);
        _animatorTrigger.OnAnimationEnd -= AnimationEndTrigger;
    }

    public void AnimationEndTrigger()
    {
        _isTriggered = true;
    }
}