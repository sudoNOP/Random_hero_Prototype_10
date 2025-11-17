using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class StatusLabelUI : MonoBehaviour
{
    [SerializeField] private CharacterBase target;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private float showDuration = 1.5f; // сколько секунд висит надпись

    private readonly HashSet<string> _activeStatuses = new HashSet<string>();
    private Coroutine _hideCoroutine;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("StatusLabelUI: Target (CharacterBase) is not assigned", this);
            enabled = false;
            return;
        }

        if (statusText == null)
        {
            Debug.LogError("StatusLabelUI: statusText (TextMeshProUGUI) is not assigned", this);
            enabled = false;
            return;
        }

        target.OnStatusEffectAdded += HandleStatusAdded;
        target.OnStatusEffectRemoved += HandleStatusRemoved;

        statusText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (target != null)
        {
            target.OnStatusEffectAdded -= HandleStatusAdded;
            target.OnStatusEffectRemoved -= HandleStatusRemoved;
        }
    }

    private void HandleStatusAdded(string displayName)
    {
        // Добавляем статус в множество и смотрим, был ли он уже
        bool isNew = _activeStatuses.Add(displayName);

        UpdateText();

        // ВАЖНО: таймер показываем только когда дебафф появился ВПЕРВЫЕ
        if (isNew)
        {
            RestartHideTimer();
        }
    }

    private void HandleStatusRemoved(string displayName)
    {
        _activeStatuses.Remove(displayName);

        if (_activeStatuses.Count == 0)
        {
            if (_hideCoroutine != null)
                StopCoroutine(_hideCoroutine);

            statusText.gameObject.SetActive(false);
            return;
        }

        UpdateText();
    }

    private void UpdateText()
    {
        if (_activeStatuses.Count == 0)
        {
            statusText.gameObject.SetActive(false);
            return;
        }

        statusText.gameObject.SetActive(true);

        var sb = new StringBuilder();

        if (_activeStatuses.Count == 1)
        {
            sb.Append("Получен дебафф: ");
        }
        else
        {
            sb.Append("Получены дебаффы: ");
        }

        bool first = true;
        foreach (var status in _activeStatuses)
        {
            if (!first)
                sb.Append(", ");

            sb.Append(status);
            first = false;
        }

        statusText.text = sb.ToString();
    }

    private void RestartHideTimer()
    {
        if (_hideCoroutine != null)
            StopCoroutine(_hideCoroutine);

        _hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private System.Collections.IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(showDuration);
        statusText.gameObject.SetActive(false);
    }
}
