using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Unlock : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI interactionText;

    [Header("Interaction")]
    public Transform player;
    public Transform button;
    public float interactionRange = 3f;

    [Header("Unlock Requirements")]
    public GameObject[] requiredBuildings;
    public int moneyAmount = 0;

    [Header("Level Loading")]
    public string unlockDescription = "Level 2";
    public string sceneToLoad = "Level2";

    private bool levelUnlocked = false;
    private bool isNearButton = false;

    private void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (interactionText == null || button == null || player == null)
        {
            return;
        }

        if (levelUnlocked)
        {
            return;
        }

        isNearButton = Vector3.Distance(button.position, player.position) <= interactionRange;

        if (!isNearButton)
        {
            interactionText.gameObject.SetActive(false);
            return;
        }

        interactionText.gameObject.SetActive(true);

        if (!AreAllBuildingsUnlocked())
        {
            interactionText.text = "Unlock all buildings first.";
            return;
        }

        interactionText.text = "Press F to travel to " + unlockDescription;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (GameManager.Instance != null && GameManager.Instance.Money >= moneyAmount)
            {
                if (GameManager.Instance.SpendMoney(moneyAmount))
                {
                    UnlockLevel();
                }
            }
        }
    }

    private bool AreAllBuildingsUnlocked()
    {
        if (requiredBuildings == null || requiredBuildings.Length == 0)
        {
            return false;
        }

        for (int i = 0; i < requiredBuildings.Length; i++)
        {
            if (requiredBuildings[i] == null)
            {
                return false;
            }

            if (!requiredBuildings[i].activeSelf)
            {
                return false;
            }
        }

        return true;
    }

    private void UnlockLevel()
    {
        levelUnlocked = true;
        interactionText.text = unlockDescription + " unlocked!";
        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(1.5f);

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}