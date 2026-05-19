using UnityEngine;
using UnityEngine.AI;

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

    [Header("Wander After Healing")]
    public NavMeshAgent agent;
    public Animator animator;
    public float wanderRadius = 8f;
    public float wanderDelay = 2f;
    public float stuckCheckTime = 2f;
    public float stuckSpeedThreshold = 0.05f;

    private PlayerInventory playerInventory;
    private float wanderTimer;
    private float stuckTimer;

    private void Start()
    {
        if (player != null)
        {
            playerInventory = player.GetComponent<PlayerInventory>();
        }

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (agent != null)
        {
            agent.isStopped = !isHealed;
        }

        SetAnimationSpeed(0f);
        UpdateVisuals();
    }

    private void Update()
    {
        if (isHealed)
        {
            Wander();
            UpdateMovementAnimation();
            return;
        }

        SetAnimationSpeed(0f);

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

        if (agent != null)
        {
            agent.isStopped = false;
        }

        Level2ObjectiveManager objectiveManager = FindObjectOfType<Level2ObjectiveManager>();

        if (objectiveManager != null)
        {
            objectiveManager.AnimalHealed();
        }

        PickNewWanderPoint();

        Debug.Log("Animal healed with soup.");
    }

    private void Wander()
    {
        if (agent == null || !agent.isOnNavMesh)
        {
            return;
        }

        wanderTimer += Time.deltaTime;

        if (!agent.pathPending && agent.remainingDistance <= 0.7f)
        {
            PickNewWanderPoint();
            return;
        }

        if (!agent.pathPending && agent.velocity.magnitude <= stuckSpeedThreshold)
        {
            stuckTimer += Time.deltaTime;

            if (stuckTimer >= stuckCheckTime)
            {
                PickNewWanderPoint();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        if (wanderTimer >= wanderDelay)
        {
            PickNewWanderPoint();
        }
    }

    private void PickNewWanderPoint()
    {
        if (agent == null || !agent.isOnNavMesh)
        {
            return;
        }

        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * wanderRadius;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                wanderTimer = 0f;
                return;
            }
        }
    }

    private void UpdateMovementAnimation()
    {
        if (agent == null)
        {
            SetAnimationSpeed(0f);
            return;
        }

        SetAnimationSpeed(agent.velocity.magnitude);
    }

    private void SetAnimationSpeed(float speed)
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", speed);
        }
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