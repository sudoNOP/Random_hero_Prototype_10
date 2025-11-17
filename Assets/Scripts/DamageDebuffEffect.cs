public class DamageDebuffEffect : StatusEffect
{
    private readonly float _multiplier;
    private float _previousMultiplier;

    public DamageDebuffEffect(CharacterBase target, float duration, float multiplier)
        : base(target, duration, "Снижен урон")
    {
        _multiplier = multiplier;
    }

    public override void OnApply()
    {
        _previousMultiplier = Target.DamageMultiplier;
        Target.DamageMultiplier *= _multiplier;
    }

    protected override void OnTick(float deltaTime)
    {
        // просто ждём окончания
    }

    public override void OnExpire()
    {
        Target.DamageMultiplier = _previousMultiplier;
    }
}