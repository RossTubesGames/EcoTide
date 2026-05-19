using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BabyTurtle : MonoBehaviour
{
    [Header("Movement")]
    public NavMeshAgent agent;
    public Transform waterTarget;

    [Header("Protection")]
    public bool isInShadow = false;
    public float dangerTimer = 0f;
    public float maxTimeWithoutShadow = 4f;

    [Header("Goal")]
    public float goalDistance = 1f;

    private bool hasReachedWater = false;

    private void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (waterTarget != null && agent != null)
        {
            agent.SetDestination(waterTarget.position);
        }
    }

    private void Update()
    {
        if (hasReachedWater)
        {
            return;
        }

        if (waterTarget != null && agent != null)
        {
            agent.SetDestination(waterTarget.position);
        }

        CheckShadowSafety();
        CheckReachedWater();
    }

    public void SetTarget(Transform target)
    {
        waterTarget = target;

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (agent != null && waterTarget != null)
        {
            agent.SetDestination(waterTarget.position);
        }
    }

    public void SetShadowState(bool value)
    {
        isInShadow = value;

        if (isInShadow)
        {
            dangerTimer = 0f;
        }
    }

    private void CheckShadowSafety()
    {
        if (isInShadow)
        {
            return;
        }

        dangerTimer += Time.deltaTime;

        if (dangerTimer >= maxTimeWithoutShadow)
        {
            Debug.Log("Baby turtle was exposed too long.");
            Destroy(gameObject);
        }
    }

    private void CheckReachedWater()
    {
        float distance = Vector3.Distance(transform.position, waterTarget.position);

        if (distance <= goalDistance)
        {
            hasReachedWater = true;

            Level2ObjectiveManager objectiveManager = FindObjectOfType<Level2ObjectiveManager>();

            if (objectiveManager != null)
            {
                objectiveManager.TurtleSaved();
            }

            Destroy(gameObject);
        }
    }
}