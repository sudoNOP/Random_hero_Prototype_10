using UnityEngine;

public class PoisonEffect : StatusEffect
{
    private readonly float _damagePerSecond;

    public PoisonEffect(CharacterBase target, float duration, float damagePerSecond)
        : base(target, duration, "Яд")
    {
        _damagePerSecond = damagePerSecond;
    }

    protected override void OnTick(float deltaTime)
    {
        if (Target.IsDead) return;

        float damage = _damagePerSecond * deltaTime;
        Target.TakeDamage(damage);
    }
}