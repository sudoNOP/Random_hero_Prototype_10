using UnityEngine;

public class ArcherAttackEffect : MonoBehaviour, IAttackEffect
{
    [SerializeField] private float poisonDuration = 5f;
    [SerializeField] private float poisonDps = 3f;

    public void OnAttack(CharacterBase attacker, CharacterBase target)
    {
        if (target == null || target.IsDead) return;

        var poison = new PoisonEffect(target, poisonDuration, poisonDps);
        target.AddStatusEffect(poison);
    }
}