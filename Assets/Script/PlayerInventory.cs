using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
        public int mushroomCount = 0;
        public int maxMushrooms = 150;

        public int tomatoCount = 0;
        public int maxTomatoes = 10;

        public bool CanPickupMushroom()
        {
            return mushroomCount < maxMushrooms;
        }

        public void AddMushroom()
        {
            mushroomCount++;
            Debug.Log("Picked up a mushroom! Total: " + mushroomCount);
            GameManager.Instance?.UpdateMushroomUI(mushroomCount);
        }

        public bool CanPickupTomato()
        {
            return tomatoCount < maxTomatoes;
        }

        public void AddTomato()
        {
            tomatoCount++;
            Debug.Log("Picked up a tomato! Total: " + tomatoCount);
            GameManager.Instance?.UpdateTomatoUI(tomatoCount);
        }
    }
