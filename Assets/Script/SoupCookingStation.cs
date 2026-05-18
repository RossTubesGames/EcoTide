using UnityEngine;

public class SoupCookingStation : MonoBehaviour
{
    [Header("Interaction")]
    public Transform player;
    public float interactionRange = 3f;

    [Header("Recipe")]
    public int mushroomsNeeded = 3;
    public int tomatoesNeeded = 2;

    [Header("Soup State")]
    public bool soupIsCooked = false;

    [Header("Optional Visuals")]
    public GameObject soupInPanVisual;

    private PlayerInventory playerInventory;

    private void Start()
    {
        if (player != null)
        {
            playerInventory = player.GetComponent<PlayerInventory>();
        }

        if (soupInPanVisual != null)
        {
            soupInPanVisual.SetActive(soupIsCooked);
        }
    }

    private void Update()
    {
        if (player == null || playerInventory == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > interactionRange)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        if (!soupIsCooked)
        {
            TryCookSoup();
            return;
        }

        TryFillBowl();
    }

    private void TryCookSoup()
    {
        if (!playerInventory.HasIngredientsForSoup(mushroomsNeeded, tomatoesNeeded))
        {
            Debug.Log("Not enough ingredients. Need " + mushroomsNeeded + " mushrooms and " + tomatoesNeeded + " tomatoes.");
            return;
        }

        playerInventory.UseIngredients(mushroomsNeeded, tomatoesNeeded);

        soupIsCooked = true;

        if (soupInPanVisual != null)
        {
            soupInPanVisual.SetActive(true);
        }
        
        Debug.Log("Soup cooked. Hold an empty bowl and press F at the pan.");
    }

    private void TryFillBowl()
    {
        if (playerInventory.heldBowl == null)
        {
            Debug.Log("You are not holding a bowl.");
            return;
        }

        if (!playerInventory.isHoldingEmptyBowl)
        {
            Debug.Log("You need to hold an empty bowl.");
            return;
        }

        playerInventory.heldBowl.FillWithSoup();

        soupIsCooked = false;

        if (soupInPanVisual != null)
        {
            soupInPanVisual.SetActive(false);
        }

        Debug.Log("Filled held bowl with soup.");
    }
}