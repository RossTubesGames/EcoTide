using System.Collections;
using UnityEngine;

public class MushroomInteractable : MonoBehaviour
{
    private Transform mushroomVisual;
    private Vector3 originalScale;
    private bool isGrowing = false;

    public bool isInRange = false;
    private Transform player;

    private void Start()
    {
        // Look for 'MushroomMesh' in the parent or its children
        Transform root = transform.parent;
        if (root != null && root.name == "MushroomMesh")
        {
            mushroomVisual = root;
        }
        else if (root != null)
        {
            mushroomVisual = root.Find("MushroomMesh");
        }

        if (mushroomVisual == null)
        {
            Debug.LogError("[Mushroom] ERROR: Could not find 'MushroomMesh' in or as parent of: " + gameObject.name);
            return;
        }

        originalScale = mushroomVisual.localScale;
        Debug.Log("[Mushroom] Setup complete on: " + mushroomVisual.name);
    }

    private void Update()
    {
        if (isInRange && !isGrowing)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("[Mushroom] E pressed.");

                if (player == null)
                {
                    Debug.LogWarning("[Mushroom] Player reference is NULL!");
                    return;
                }

                PlayerInventory inventory = player.GetComponentInChildren<PlayerInventory>();

                if (inventory == null)
                {
                    Debug.LogWarning("[Mushroom] No PlayerInventory found on Player!");
                    return;
                }

                if (!inventory.CanPickupMushroom())
                {
                    Debug.Log("[Mushroom] Inventory full.");
                    return;
                }

                Debug.Log("[Mushroom] Pickup confirmed!");
                inventory.AddMushroom();
                StartCoroutine(RespawnMushroom());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            player = other.transform.root;
            Debug.Log("[Mushroom] Player entered range.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            player = null;
            Debug.Log("[Mushroom] Player left range.");
        }
    }

    private IEnumerator RespawnMushroom()
    {
        isGrowing = true;

        if (mushroomVisual == null)
        {
            Debug.LogError("[Mushroom] ERROR: mushroomVisual is null during respawn!");
            yield break;
        }

        // Hide mesh without disabling logic
        MeshRenderer renderer = mushroomVisual.GetComponent<MeshRenderer>();
        if (renderer != null)
            renderer.enabled = false;
        else
            Debug.LogWarning("[Mushroom] WARNING: No MeshRenderer found on mushroomVisual.");

        yield return new WaitForSeconds(10f);

        mushroomVisual.localScale = Vector3.zero;

        if (renderer != null)
            renderer.enabled = true;

        float growDuration = 3f;
        float time = 0f;

        while (time < growDuration)
        {
            mushroomVisual.localScale = Vector3.Lerp(Vector3.zero, originalScale, time / growDuration);
            time += Time.deltaTime;
            yield return null;
        }

        mushroomVisual.localScale = originalScale;
        isGrowing = false;

        Debug.Log("[Mushroom] Finished regrowing.");
    }
}
