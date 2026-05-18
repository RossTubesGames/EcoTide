using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Ingredients")]
    public int mushroomCount = 0;
    public int maxMushrooms = 150;

    public int tomatoCount = 0;
    public int maxTomatoes = 15;

    [Header("Soup")]
    public bool isHoldingEmptyBowl = false;
    public bool isHoldingSoupBowl = false;
    public CarryAbleBowl heldBowl;

    public bool CanPickupMushroom()
    {
        return mushroomCount < maxMushrooms;
    }

    public void AddMushroom()
    {
        mushroomCount++;
        Debug.Log("Picked up a mushroom. Total: " + mushroomCount);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateMushroomUI(mushroomCount);
        }
    }

    public bool CanPickupTomato()
    {
        return tomatoCount < maxTomatoes;
    }

    public void AddTomato()
    {
        tomatoCount++;
        Debug.Log("Picked up a tomato. Total: " + tomatoCount);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateTomatoUI(tomatoCount);
        }
    }

    public bool HasIngredientsForSoup(int mushroomsNeeded, int tomatoesNeeded)
    {
        return mushroomCount >= mushroomsNeeded && tomatoCount >= tomatoesNeeded;
    }

    public void UseIngredients(int mushroomsUsed, int tomatoesUsed)
    {
        mushroomCount -= mushroomsUsed;
        tomatoCount -= tomatoesUsed;

        if (mushroomCount < 0)
        {
            mushroomCount = 0;
        }

        if (tomatoCount < 0)
        {
            tomatoCount = 0;
        }

        Debug.Log("Used ingredients. Mushrooms left: " + mushroomCount + ", Tomatoes left: " + tomatoCount);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateMushroomUI(mushroomCount);
            GameManager.Instance.UpdateTomatoUI(tomatoCount);
        }
    }

    public void SetHeldBowl(CarryAbleBowl bowl)
    {
        heldBowl = bowl;

        if (heldBowl == null)
        {
            isHoldingEmptyBowl = false;
            isHoldingSoupBowl = false;
            Debug.Log("Player is no longer holding a bowl.");
            return;
        }

        isHoldingEmptyBowl = !heldBowl.hasSoup;
        isHoldingSoupBowl = heldBowl.hasSoup;

        Debug.Log("Held bowl updated. Empty bowl: " + isHoldingEmptyBowl + ", Soup bowl: " + isHoldingSoupBowl);
    }

    public bool IsHoldingBowl()
    {
        return heldBowl != null;
    }

    public bool IsHoldingEmptyBowl()
    {
        return heldBowl != null && !heldBowl.hasSoup;
    }

    public bool IsHoldingSoupBowl()
    {
        return heldBowl != null && heldBowl.hasSoup;
    }
}