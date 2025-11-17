using System.Collections;
using TMPro;
using UnityEngine;

public class DamageTextUI : MonoBehaviour
{
    [SerializeField] private CharacterBase target;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float moveUpDistance = 150f;
    [SerializeField] private float duration = 0.7f;

    private RectTransform _rect;
    private Vector2 _startAnchoredPos;
    private Coroutine _currentAnimation;

    private void Awake()
    {
        if (damageText == null)
        {
            damageText = GetComponent<TextMeshProUGUI>();
        }

        _rect = damageText.GetComponent<RectTransform>();
        _startAnchoredPos = _rect.anchoredPosition;

        // ВАЖНО: отключаем только рендер текста, а не весь объект
        damageText.enabled = false;
    }

    private void Start()
    {
        if (target == null)
        {
            target = GetComponentInParent<CharacterBase>();
        }

        if (target == null)
        {
            Debug.LogError("DamageTextUI: target (CharacterBase) не задан и не найден в родителях", this);
            enabled = false;
            return;
        }

        if (damageText == null)
        {
            Debug.LogError("DamageTextUI: damageText (TextMeshProUGUI) не задан", this);
            enabled = false;
            return;
        }

        target.OnDamaged += HandleDamaged;
    }

    private void OnDestroy()
    {
        if (target != null)
            target.OnDamaged -= HandleDamaged;
    }

    private void HandleDamaged(float amount)
    {
        if (_currentAnimation != null)
            StopCoroutine(_currentAnimation);

        _currentAnimation = StartCoroutine(AnimateDamage(amount));
    }

    private IEnumerator AnimateDamage(float amount)
    {
        damageText.enabled = true;
        int shownDamage = Mathf.RoundToInt(amount);

// если по какой-то причине прилетел очень маленький, но положительный урон
        if (shownDamage <= 0 && amount > 0f)
            shownDamage = 1;

        damageText.text = shownDamage.ToString();

        _rect.anchoredPosition = _startAnchoredPos;

        var color = damageText.color;
        color.a = 1f;
        damageText.color = color;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;

            _rect.anchoredPosition = _startAnchoredPos + Vector2.up * moveUpDistance * normalized;

            color.a = 1f - normalized;
            damageText.color = color;

            yield return null;
        }

        // Снова просто прячем текст, объект остаётся активным
        damageText.enabled = false;
    }
}
