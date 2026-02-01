using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI mushroomText;
    public TextMeshProUGUI tomatoText;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.moneyText = moneyText;
            GameManager.Instance.mushroomText = mushroomText;
            GameManager.Instance.tomatoText = tomatoText;

            GameManager.Instance.UpdateMoneyUI();
        }
    }
}
