using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private CharacterBase target;
    [SerializeField] private Slider slider;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("HealthBar: target not set");
            return;
        }

        if (slider == null)
        {
            Debug.LogWarning("HealthBar: slider not set");
            return;
        }

        slider.maxValue = target.MaxHealth;
        slider.value = target.MaxHealth;

        target.OnHealthChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        if (target != null)
            target.OnHealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(float current, float max)
    {
        slider.maxValue = max;
        slider.value = current;
    }
}