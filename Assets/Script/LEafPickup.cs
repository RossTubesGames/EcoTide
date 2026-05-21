using UnityEngine;

public class LEafPickup : MonoBehaviour
{
    [Header("Interaction")]
    public Transform player;
    public float interactionRange = 3f;
    public KeyCode pickupKey = KeyCode.F;

    [Header("Hold Settings")]
    public Transform holdPoint;
    public GameObject leafVisual;
    public GameObject shadowZone;

    [Header("Hold Offset")]
    public Vector3 holdLocalPosition = new Vector3(0.4f, -0.3f, 0.8f);
    public Vector3 holdLocalRotation = new Vector3(20f, 120f, 20f);

    [Header("State")]
    public bool isPickedUp = false;

    private Collider[] leafColliders;

    private void Start()
    {
        leafColliders = GetComponentsInChildren<Collider>();

        if (shadowZone != null)
        {
            shadowZone.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isPickedUp)
        {
            TryPickup();
            return;
        }

        FollowHoldPoint();
    }

    private void TryPickup()
    {
        if (player == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactionRange && Input.GetKeyDown(pickupKey))
        {
            PickUpLeaf();
        }
    }

    private void PickUpLeaf()
    {
        isPickedUp = true;

        for (int i = 0; i < leafColliders.Length; i++)
        {
            if (leafColliders[i] != null && leafColliders[i].gameObject != shadowZone)
            {
                leafColliders[i].enabled = false;
            }
        }

        if (shadowZone != null)
        {
            shadowZone.SetActive(true);
        }

        if (holdPoint != null)
        {
            transform.SetParent(holdPoint);
        }

        FollowHoldPoint();
    }

    private void FollowHoldPoint()
    {
        if (holdPoint != null)
        {
            transform.SetParent(holdPoint);
        }

        Level2ObjectiveManager objectiveManager = FindObjectOfType<Level2ObjectiveManager>();

        if (objectiveManager != null)
        {
            objectiveManager.LeafPickedUp();
        }

        FollowHoldPoint();
    }
}