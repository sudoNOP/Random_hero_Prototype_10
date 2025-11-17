using UnityEngine;

public class MageAttackEffect : MonoBehaviour, IAttackEffect
{
    [SerializeField] private float debuffDuration = 4f;
    [SerializeField, Range(0.1f, 1f)] private float damageMultiplier = 0.7f;

    public void OnAttack(CharacterBase attacker, CharacterBase target)
    {
        if (target == null || target.IsDead) return;

        var debuff = new DamageDebuffEffect(target, debuffDuration, damageMultiplier);
        target.AddStatusEffect(debuff);
    }
}