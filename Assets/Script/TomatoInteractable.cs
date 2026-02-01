using System.Collections;
using UnityEngine;

public class TomatoInteractable : MonoBehaviour
{
    private MeshRenderer tomatoRenderer;
    private Vector3 originalScale;
    private bool isGrowing = false;
    private Transform player;
    public bool isInRange = false;

    private void Start()
    {
        tomatoRenderer = GetComponent<MeshRenderer>();
        if (tomatoRenderer == null)
        {
            Debug.LogError("[Tomato] No MeshRenderer found on tomato object!");
            return;
        }

        originalScale = transform.localScale;
        Debug.Log("[Tomato] Setup complete on: " + name);
    }

    private void Update()
    {
        if (isInRange && !isGrowing && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[Tomato] E pressed.");

            if (player == null)
            {
                Debug.LogWarning("[Tomato] Player reference is NULL!");
                return;
            }

            PlayerInventory inventory = player.GetComponentInChildren<PlayerInventory>();
            if (inventory == null)
            {
                Debug.LogWarning("[Tomato] No PlayerInventory found!");
                return;
            }

            if (!inventory.CanPickupTomato())
            {
                Debug.Log("[Tomato] Inventory full.");
                return;
            }

            inventory.AddTomato();
            StartCoroutine(RespawnTomato());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            player = other.transform.root;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            player = null;
        }
    }

    private IEnumerator RespawnTomato()
    {
        isGrowing = true;

        if (tomatoRenderer != null)
            tomatoRenderer.enabled = false;

        yield return new WaitForSeconds(8f); // shorter regrow for tomatoes?

        transform.localScale = Vector3.zero;

        if (tomatoRenderer != null)
            tomatoRenderer.enabled = true;

        float growTime = 2f;
        float t = 0f;

        while (t < growTime)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, t / growTime);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        isGrowing = false;
        Debug.Log("[Tomato] Tomato has regrown.");
    }
}
