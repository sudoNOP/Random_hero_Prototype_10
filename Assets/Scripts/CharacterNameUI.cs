using TMPro;
using UnityEngine;

public class CharacterNameUI : MonoBehaviour
{
    [SerializeField] private CharacterBase target;
    [SerializeField] private TextMeshProUGUI nameText;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("CharacterNameUI: target (CharacterBase) не задан", this);
            enabled = false;
            return;
        }

        if (nameText == null)
        {
            Debug.LogError("CharacterNameUI: nameText (TextMeshProUGUI) не задан", this);
            enabled = false;
            return;
        }

        // При старте просто берём текущее имя
        nameText.text = target.CharacterName;
    }

    // смена имя в рантайме
    public void Refresh()
    {
        if (target != null && nameText != null)
            nameText.text = target.CharacterName;
    }
}