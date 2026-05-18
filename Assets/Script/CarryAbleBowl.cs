using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryAbleBowl : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float interactionRange = 3f;
    public Transform player;
    public Transform holdPoint;

    [Header("Bowl State")]
    public bool isHeld = false;
    public bool hasSoup = false;

    private PlayerInventory playerInventory;
    private Rigidbody rb;
    private Collider bowlCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        bowlCollider = GetComponent<Collider>();

        if (player != null)
        {
            playerInventory = player.GetComponent<PlayerInventory>();
        }
    }

    private void Update()
    {
        if (player == null || holdPoint == null || playerInventory == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (!isHeld && distance <= interactionRange)
            {
                PickUpBowl();
            }
        }

        if (isHeld)
        {
            transform.position = holdPoint.position;
            transform.rotation = holdPoint.rotation;
        }
    }

    private void PickUpBowl()
    {
        isHeld = true;

        playerInventory.isHoldingEmptyBowl = true;
        playerInventory.isHoldingSoupBowl = false;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if (bowlCollider != null)
        {
            bowlCollider.enabled = false;
        }

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        Debug.Log("Picked up empty bowl.");
    }

    public void FillWithSoup()
    {
        hasSoup = true;

        if (playerInventory != null)
        {
            playerInventory.SetHeldBowl(this);
        }

        Debug.Log("Bowl filled with soup.");
    }

    public void EmptyBowl()
    {
        hasSoup = false;

        if (playerInventory != null)
        {
            playerInventory.isHoldingEmptyBowl = true;
            playerInventory.isHoldingSoupBowl = false;
        }

        Debug.Log("Soup bowl emptied.");
    }
}