using UnityEngine;

public class Level2ObjectiveManager : MonoBehaviour
{
    [Header("Animal Objective")]
    public int animalsNeeded = 3;
    public int animalsHealed = 0;

    [Header("Turtle Objective")]
    public GameObject turtleObjectiveObjects;

    private void Start()
    {
        if (turtleObjectiveObjects != null)
        {
            turtleObjectiveObjects.SetActive(false);
        }
    }

    public void AnimalHealed()
    {
        animalsHealed++;

        Debug.Log("Animals healed: " + animalsHealed + "/" + animalsNeeded);

        if (animalsHealed >= animalsNeeded)
        {
            StartTurtleObjective();
        }
    }

    private void StartTurtleObjective()
    {
        Debug.Log("All animals saved. Turtle objective started.");

        if (turtleObjectiveObjects != null)
        {
            turtleObjectiveObjects.SetActive(true);
        }
    }
}