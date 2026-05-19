using TMPro;
using UnityEngine;

public class Level2ObjectiveManager : MonoBehaviour
{
    [Header("Animal Objective")]
    public int animalsNeeded = 3;
    public int animalsHealed = 0;

    [Header("Turtle Objective")]
    public GameObject turtleObjectiveObjects;
    public int turtlesNeeded = 5;
    public int turtlesSaved = 0;

    [Header("UI")]
    public TextMeshProUGUI objectiveText;

    private void Start()
    {
        if (turtleObjectiveObjects != null)
        {
            turtleObjectiveObjects.SetActive(false);
        }

        UpdateObjectiveText();
    }

    public void AnimalHealed()
    {
        animalsHealed++;

        if (animalsHealed >= animalsNeeded)
        {
            StartTurtleObjective();
        }

        UpdateObjectiveText();
    }

    public void TurtleSaved()
    {
        turtlesSaved++;

        UpdateObjectiveText();

        if (turtlesSaved >= turtlesNeeded)
        {
            Debug.Log("All baby turtles reached the ocean. Level complete.");
        }
    }

    private void StartTurtleObjective()
    {
        if (turtleObjectiveObjects != null)
        {
            turtleObjectiveObjects.SetActive(true);
        }

        Debug.Log("All animals saved. Turtle objective started.");
    }

    private void UpdateObjectiveText()
    {
        if (objectiveText == null)
        {
            return;
        }

        if (animalsHealed < animalsNeeded)
        {
            objectiveText.text = "Save the forest animals: " + animalsHealed + "/" + animalsNeeded;
        }
        else
        {
            objectiveText.text = "Protect the baby turtles: " + turtlesSaved + "/" + turtlesNeeded;
        }
    }
}