using UnityEngine;

public class WoundedAnimal : MonoBehaviour
{
    [Header("Interaction")]
    public Transform player;
    public float interactionRange = 3f;

    [Header("State")]
    public bool isHealed = false;

    [Header("Optional Visuals")]
    public GameObject woundedVisual;
    public GameObject healedVisual;

    private PlayerInventory playerInventory;

    private void Start()
    {
        if (player != null)
        {
            playerInventory = player.GetComponent<PlayerInventory>();
        }

        if (playerInventory == null)
        {
            Debug.LogWarning("WoundedAnimal could not find PlayerInventory on the player.");
        }

        UpdateVisuals();
    }

    private void Update()
    {
        if (isHealed)
        {
            return;
        }

        if (player == null || playerInventory == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactionRange && Input.GetKeyDown(KeyCode.F))
        {
            TryFeedAnimal();
        }
    }

    private void TryFeedAnimal()
    {
        if (playerInventory.heldBowl == null)
        {
            Debug.Log("You need to hold a bowl with soup.");
            return;
        }

        if (!playerInventory.heldBowl.hasSoup)
        {
            Debug.Log("The bowl is empty. Fill it with soup first.");
            return;
        }

        playerInventory.heldBowl.EmptyBowl();

        isHealed = true;
        UpdateVisuals();

        Level2ObjectiveManager objectiveManager = FindObjectOfType<Level2ObjectiveManager>();

        if (objectiveManager != null)
        {
            objectiveManager.AnimalHealed();
        }

        Debug.Log("Animal healed with soup.");
    }

    private void UpdateVisuals()
    {
        if (woundedVisual != null)
        {
            woundedVisual.SetActive(!isHealed);
        }

        if (healedVisual != null)
        {
            healedVisual.SetActive(isHealed);
        }
    }
}