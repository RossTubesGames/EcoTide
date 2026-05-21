using System.Collections;
using TMPro;
using UnityEngine;

public class Level2ObjectiveManager : MonoBehaviour
{
    [Header("Shed Objective")]
    public GameObject shedObject;

    [Header("Animal Objective")]
    public WoundedAnimal[] animalsToHeal;

    [Header("Turtle Objective")]
    public GameObject turtleObjectiveObjects;
    public TurtleEggNest turtleEggNest;
    public float eggHatchCountdown = 30f;

    [Header("Ending")]
    public GameObject endingSequence;

    [Header("Turtle Saving")]
    public int turtlesNeeded = 5;
    public int turtlesSaved = 0;

    [Header("UI")]
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI introPopupText;
    public float introPopupTime = 10f;

    private bool turtleObjectiveStarted = false;
    private bool eggsHatched = false;

    private void Start()
    {
        if (turtleObjectiveObjects != null)
        {
            turtleObjectiveObjects.SetActive(false);
        }

        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }

        StartCoroutine(ShowIntroPopup());
        UpdateObjectiveText();
    }

    private void Update()
    {
        if (!turtleObjectiveStarted)
        {
            CheckAllAnimalsHealed();
            UpdateObjectiveText();
        }
    }

    private IEnumerator ShowIntroPopup()
    {
        if (introPopupText != null)
        {
            introPopupText.text = "Collect mushrooms and tomatoes with E. Unlock the shed to cook soup for the animals.";
            introPopupText.gameObject.SetActive(true);

            yield return new WaitForSeconds(introPopupTime);

            introPopupText.gameObject.SetActive(false);
        }
    }

    private void CheckAllAnimalsHealed()
    {
        if (shedObject != null && !shedObject.activeInHierarchy)
        {
            return;
        }

        if (animalsToHeal == null || animalsToHeal.Length == 0)
        {
            return;
        }

        for (int i = 0; i < animalsToHeal.Length; i++)
        {
            if (animalsToHeal[i] == null || !animalsToHeal[i].isHealed)
            {
                return;
            }
        }

        StartTurtleObjective();
    }

    private int GetAnimalsHealedCount()
    {
        int count = 0;

        if (animalsToHeal == null)
        {
            return count;
        }

        for (int i = 0; i < animalsToHeal.Length; i++)
        {
            if (animalsToHeal[i] != null && animalsToHeal[i].isHealed)
            {
                count++;
            }
        }

        return count;
    }

    private void StartTurtleObjective()
    {
        turtleObjectiveStarted = true;

        PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.RemoveHeldBowl();
        }

        if (turtleObjectiveObjects != null)
        {
            turtleObjectiveObjects.SetActive(true);
        }

        UpdateObjectiveText();
    }

    public void LeafPickedUp()
    {
        if (!turtleObjectiveStarted || eggsHatched)
        {
            return;
        }

        if (objectiveText != null)
        {
            objectiveText.text = "Return to the beach. Keep the leaf shadow over the baby turtles.";
        }

        StartCoroutine(EggHatchTimer());
    }

    private IEnumerator EggHatchTimer()
    {
        float timeLeft = eggHatchCountdown;

        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }

        while (timeLeft > 0)
        {
            if (timerText != null)
            {
                timerText.text = "Eggs hatch in: " + Mathf.CeilToInt(timeLeft);
            }

            if (objectiveText != null)
            {
                objectiveText.text = "Return to the beach before the eggs hatch.";
            }

            timeLeft -= Time.deltaTime;
            yield return null;
        }

        HatchEggs();
    }

    private void HatchEggs()
    {
        if (eggsHatched)
        {
            return;
        }

        eggsHatched = true;

        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }

        if (turtleEggNest != null)
        {
            turtleEggNest.HatchEggs();
        }

        UpdateObjectiveText();
    }

    public void TurtleSaved()
    {
        turtlesSaved++;
        UpdateObjectiveText();

        if (turtlesSaved >= turtlesNeeded && objectiveText != null)
        {
            objectiveText.text = "You saved all the baby turtles!";
        }

        if (turtlesSaved >= turtlesNeeded)
        {
            if (objectiveText != null)
            {
                objectiveText.text = "You saved all the baby turtles!";
            }

            if (endingSequence != null)
            {
                endingSequence.SetActive(true);
            }
        }
    }

    private void UpdateObjectiveText()
    {
        if (objectiveText == null)
        {
            return;
        }

        if (shedObject != null && !shedObject.activeInHierarchy)
        {
            objectiveText.text = "Unlock the shed: 0/1";
        }
        else if (!turtleObjectiveStarted)
        {
            int healed = GetAnimalsHealedCount();
            int total = animalsToHeal != null ? animalsToHeal.Length : 0;

            objectiveText.text = "Save the forest animals: " + healed + "/" + total;
        }
        else if (!eggsHatched)
        {
            objectiveText.text = "Find the shadow leaf: 0/1";
        }
        else
        {
            objectiveText.text = "Protect the baby turtles: " + turtlesSaved + "/" + turtlesNeeded;
        }
    }
}