using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Money = 0;

    // UI references (auto-detected when scene loads)
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI mushroomText;
    public TextMeshProUGUI tomatoText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");

        // Always try to assign money text (exists in all levels)
        moneyText = GameObject.Find("MoneyText")?.GetComponent<TextMeshProUGUI>();

        // Only assign mushroom and tomato in Level 2
        if (scene.name == "Level 2")
        {
            mushroomText = GameObject.Find("MushroomText")?.GetComponent<TextMeshProUGUI>();
            tomatoText = GameObject.Find("TomatoText")?.GetComponent<TextMeshProUGUI>();

            if (mushroomText == null) Debug.LogWarning("MushroomText not found in Level 2.");
            if (tomatoText == null) Debug.LogWarning("TomatoText not found in Level 2.");
        }
        else
        {
            // In Level 1 or other scenes, clear them out
            mushroomText = null;
            tomatoText = null;
        }

        UpdateMoneyUI();
    }


    public void AddMoney(int amount)
    {
        Money += amount;
        UpdateMoneyUI();
    }

    public bool SpendMoney(int amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            UpdateMoneyUI();
            return true;
        }
        return false;
    }

    public void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = "Money: " + Money;
    }

    public void UpdateMushroomUI(int count)
    {
        if (mushroomText != null)
            mushroomText.text = "Mushrooms: " + count;
    }

    public void UpdateTomatoUI(int count)
    {
        if (tomatoText != null)
            tomatoText.text = "Tomatoes: " + count;
    }
}
