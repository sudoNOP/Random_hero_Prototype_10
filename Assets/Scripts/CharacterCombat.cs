using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    [SerializeField] private CharacterBase self;
    [SerializeField] private CharacterBase target;

    private float _attackTimer;
    private bool _isActive = false;

    private void Awake()
    {
        if (self == null)
            self = GetComponent<CharacterBase>();
    }

    private void Update()
    {
        if (!_isActive || self == null || target == null) return;
        if (self.IsDead || target.IsDead) return;
        if (self.IsStunned) return;

        _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0f)
        {
            self.PerformAttack(target);
            _attackTimer = self.AttackCooldown;
        }
    }

    public void SetTarget(CharacterBase newTarget)
    {
        target = newTarget;
    }

    public void StartCombat()
    {
        _isActive = true;
        _attackTimer = 0f; // сразу можно ударить
    }

    public void StopCombat()
    {
        _isActive = false;
    }
}