using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Header("Stats")] [SerializeField] private string characterName;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float attackCooldown = 1.5f;

    public string CharacterName => characterName;
    public float MaxHealth => maxHealth;
    public float CurrentHealth { get; private set; }
    public float BaseDamage => baseDamage;
    public float AttackCooldown => attackCooldown;

    public bool IsDead => CurrentHealth <= 0f;
    public bool IsStunned { get; set; } = false;
    public float DamageMultiplier { get; set; } = 1f;

  

    public event Action<float, float> OnHealthChanged; // (current, max)
    public event Action<CharacterBase> OnDeath;

    // новые события для дебафф-UI
    public event Action<string> OnStatusEffectAdded; // DisplayName
    public event Action<string> OnStatusEffectRemoved; // DisplayName
    
   // old place call damage
   public event Action<float> OnDamaged; // amount

    private readonly List<StatusEffect> _statusEffects = new List<StatusEffect>();

    private void Awake()
    {
        CurrentHealth = maxHealth;
        DamageMultiplier = 1f;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        for (int i = _statusEffects.Count - 1; i >= 0; i--)
        {
            var effect = _statusEffects[i];

            effect.Tick(deltaTime);

            if (effect.IsFinished)
            {
                effect.OnExpire();
                OnStatusEffectRemoved?.Invoke(effect.DisplayName); // сообщаем UI
                _statusEffects.RemoveAt(i);
            }
        }
    }

    public void SetName(string newName)
    {
        characterName = newName;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        CurrentHealth -= amount;
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        // Показываем только "значимый" урон
        if (amount >= 0.5f)
        { 
            // Сообщаем всем слушателям, что герой получил урон
            OnDamaged?.Invoke(amount);
        }

        if (CurrentHealth <= 0f)
        {
            CurrentHealth = 0f;
            OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
            Die();
        }
    }



    private void Die()
    {
        OnDeath?.Invoke(this);
    }

    public void Heal(float amount)
    {
        if (IsDead) return;

        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void PerformAttack(CharacterBase target)
    {
        if (IsDead || target == null || target.IsDead) return;

        float damage = BaseDamage * DamageMultiplier;
        target.TakeDamage(damage);

        // вызываем все IAttackEffect на этом объекте
        var components = GetComponents<MonoBehaviour>();
        foreach (var component in components)
        {
            if (component is IAttackEffect effect)
            {
                effect.OnAttack(this, target);
            }
        }
    }

    public void AddStatusEffect(StatusEffect effect)
    {
        _statusEffects.Add(effect);
        effect.OnApply();
        OnStatusEffectAdded?.Invoke(effect.DisplayName); // сообщаем UI
    }
}