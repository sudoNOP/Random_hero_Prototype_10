using UnityEngine;

/// <summary>
/// Базовый класс для любого дебаффа/статуса.
/// Не является MonoBehaviour, живёт внутри CharacterBase.
/// </summary>
public abstract class StatusEffect
{
    public string DisplayName { get; }   // Имя для UI ("Стан", "Яд", "Снижен урон")

    protected readonly CharacterBase Target;

    private float _timeLeft;

    public bool IsFinished => _timeLeft <= 0f;

    protected StatusEffect(CharacterBase target, float duration, string displayName)
    {
        Target = target;
        _timeLeft = duration;
        DisplayName = displayName;
    }

    public void Tick(float deltaTime)
    {
        if (IsFinished) return;

        _timeLeft -= deltaTime;
        OnTick(deltaTime);
    }

    //Что происходит каждый кадр, пока эффект активен
    protected abstract void OnTick(float deltaTime);

    // Вызывается сразу при наложении эффекта
    public virtual void OnApply() { }

    // <Вызывается, когда эффект заканчивается
    public virtual void OnExpire() { }
}
