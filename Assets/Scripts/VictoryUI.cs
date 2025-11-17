using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] private GameObject rootPanel;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Button newBattleButton;
    [SerializeField] private BattleManager battleManager;

    private void Awake()
    {
        if (newBattleButton != null)
        {
            newBattleButton.onClick.AddListener(OnNewBattleClicked);
        }

        Hide();
    }

    public void Show(string winnerName)
    {
        if (winnerText != null)
        {
            winnerText.text = $"Победил: {winnerName}";
        }

        if (rootPanel != null)
            rootPanel.SetActive(true);
    }

    public void Hide()
    {
        if (rootPanel != null)
            rootPanel.SetActive(false);
    }

    private void OnNewBattleClicked()
    {
        if (battleManager != null)
        {
            battleManager.StartNewBattle();
        }
    }
}