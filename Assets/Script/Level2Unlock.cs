using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // Needed for scene loading
using TMPro;
using UnityEngine;

public class Level2Unlock : MonoBehaviour
{
    public TextMeshProUGUI interactionText; // Assign in Inspector
    public Transform player; // Assign the player in Inspector
    public Transform button; // Assign the button object in Inspector
    private bool LevelUnlocked = false; // Prevents UI from updating after unlocking
    private bool isNearButton = false;
    public float interactionRange = 3f; // Distance required to interact
    public int moneyAmount; // Cost to unlock
    public string unlockDescription = "Level2"; // Description (editable in Inspector)
    public string sceneToLoad = "Level2"; // Name of the scene to load (set in Inspector)

    private void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false); // Hide text initially
        }
    }

    private void Update()
    {
        if (interactionText == null || button == null || player == null)
        {
            Debug.LogError("Missing References! Make sure all public variables are assigned in the Inspector.");
            return;
        }

        if (LevelUnlocked) return; // Stop UI updates if already unlocked

        // Check if the player is near the button
        isNearButton = Vector3.Distance(button.position, player.position) < interactionRange;

        if (isNearButton)
        {
            interactionText.gameObject.SetActive(true);
            interactionText.text = $"Press F to Unlock {unlockDescription} for {moneyAmount}$";

            // Check if player presses 'F'
            if (Input.GetKeyDown(KeyCode.F) && GameManager.Instance.Money >= moneyAmount)
            {
                if (GameManager.Instance.SpendMoney(moneyAmount)) // Deduct money
                {
                    interactionText.text = $"{unlockDescription} Unlocked!";
                    LevelUnlocked = true;

                    StartCoroutine(LoadNextLevel()); // Start coroutine for scene load
                }
            }
        }
        else
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(2f); // Small delay before transfer (optional)

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Scene name not set in Inspector!");
        }
    }
}