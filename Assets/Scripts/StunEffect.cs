public class StunEffect : StatusEffect
{
    public StunEffect(CharacterBase target, float duration)
        : base(target, duration, "Стан")
    {
    }

    public override void OnApply()
    {
        Target.IsStunned = true;
    }

    protected override void OnTick(float deltaTime)
    {
        // логика только по времени, всё уже в базовом классе
    }

    public override void OnExpire()
    {
        Target.IsStunned = false;
    }
}