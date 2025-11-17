using UnityEngine;

public class WarriorAttackEffect : MonoBehaviour, IAttackEffect
{
    [SerializeField] private float stunChance = 0.3f;
    [SerializeField] private float stunDuration = 2f;

    private System.Random _rng = new System.Random();

    public void OnAttack(CharacterBase attacker, CharacterBase target)
    {
        if (target == null || target.IsDead) return;

        double roll = _rng.NextDouble();
        if (roll <= stunChance)
        {
            var stun = new StunEffect(target, stunDuration);
            target.AddStatusEffect(stun);
        }
    }
}