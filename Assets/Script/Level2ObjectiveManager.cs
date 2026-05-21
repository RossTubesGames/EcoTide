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

    [Header("Fail State")]
    public GameObject failPanel;

    [Header("Turtle Saving")]
    public int turtlesNeeded = 5;
    public int turtlesSaved = 0;

    [Header("UI")]
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI introPopupText;
    public float introPopupTime = 10f;

    [Header("Leaf Tutorial")]
    public TextMeshProUGUI leafTutorialText;
    public float leafTutorialTime = 8f;

    private bool turtleObjectiveStarted = false;
    private bool eggsHatched = false;
    private bool leafHasBeenPickedUp = false;
    private bool endingStarted = false;
    private bool failedTurtleObjective = false;

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

        if (introPopupText != null)
        {
            introPopupText.gameObject.SetActive(false);
        }

        if (leafTutorialText != null)
        {
            leafTutorialText.gameObject.SetActive(false);
        }

        if (endingSequence != null)
        {
            endingSequence.SetActive(false);
        }

        if (failPanel != null)
        {
            failPanel.SetActive(false);
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
        if (introPopupText == null)
        {
            yield break;
        }

        introPopupText.text = "Collect mushrooms and tomatoes with E. Unlock the shed to cook soup for the animals.";
        introPopupText.gameObject.SetActive(true);

        yield return new WaitForSeconds(introPopupTime);

        introPopupText.gameObject.SetActive(false);
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

    private void StartTurtleObjective()
    {
        if (turtleObjectiveStarted)
        {
            return;
        }

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
        if (leafHasBeenPickedUp)
        {
            return;
        }

        if (!turtleObjectiveStarted || eggsHatched || failedTurtleObjective)
        {
            return;
        }

        leafHasBeenPickedUp = true;

        if (objectiveText != null)
        {
            objectiveText.text = "Return to the beach before the eggs hatch.";
        }

        StartCoroutine(ShowLeafTutorial());
        StartCoroutine(EggHatchTimer());
    }

    private IEnumerator ShowLeafTutorial()
    {
        if (leafTutorialText == null)
        {
            yield break;
        }

        leafTutorialText.text = "Cast the leaf shadow over the baby turtles to reset their danger timer and keep them safe.";
        leafTutorialText.gameObject.SetActive(true);

        yield return new WaitForSeconds(leafTutorialTime);

        leafTutorialText.gameObject.SetActive(false);
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
        if (endingStarted || failedTurtleObjective)
        {
            return;
        }

        turtlesSaved++;

        if (turtlesSaved > turtlesNeeded)
        {
            turtlesSaved = turtlesNeeded;
        }

        UpdateObjectiveText();

        if (turtlesSaved >= turtlesNeeded)
        {
            StartEnding();
        }
    }

    public void CheckTurtleFailure()
    {
        if (endingStarted || failedTurtleObjective)
        {
            return;
        }

        if (turtlesSaved < turtlesNeeded)
        {
            failedTurtleObjective = true;

            PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

            if (playerMovement != null)
            {
                playerMovement.UnlockMouse();
                playerMovement.enabled = false;
            }

            PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();

            if (playerInteraction != null)
            {
                playerInteraction.enabled = false;
            }

            if (objectiveText != null)
            {
                objectiveText.text = "Some baby turtles did not make it.";
            }

            if (timerText != null)
            {
                timerText.gameObject.SetActive(false);
            }

            if (leafTutorialText != null)
            {
                leafTutorialText.gameObject.SetActive(false);
            }

            if (failPanel != null)
            {
                failPanel.SetActive(true);
            }
        }
    }

    private void StartEnding()
    {
        if (endingStarted)
        {
            return;
        }

        endingStarted = true;

        if (objectiveText != null)
        {
            objectiveText.text = "You saved all the baby turtles!";
        }

        if (failPanel != null)
        {
            failPanel.SetActive(false);
        }

        if (endingSequence != null)
        {
            endingSequence.SetActive(true);
        }
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

    private void UpdateObjectiveText()
    {
        if (objectiveText == null)
        {
            return;
        }

        if (failedTurtleObjective)
        {
            objectiveText.text = "Some baby turtles did not make it.";
        }
        else if (shedObject != null && !shedObject.activeInHierarchy)
        {
            objectiveText.text = "Unlock the shed: 0/1";
        }
        else if (!turtleObjectiveStarted)
        {
            int healed = GetAnimalsHealedCount();
            int total = animalsToHeal != null ? animalsToHeal.Length : 0;

            objectiveText.text = "Save the forest animals: " + healed + "/" + total;
        }
        else if (!leafHasBeenPickedUp)
        {
            objectiveText.text = "Find the shadow leaf: 0/1";
        }
        else if (!eggsHatched)
        {
            objectiveText.text = "Return to the beach before the eggs hatch.";
        }
        else
        {
            objectiveText.text = "Protect the baby turtles: " + turtlesSaved + "/" + turtlesNeeded;
        }
    }
}