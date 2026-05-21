using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleEggNest : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject babyTurtlePrefab;
    public Transform[] spawnPoints;
    public Transform waterTarget;
    public int turtlesToSpawn = 5;
    public float delayBetweenSpawns = 1f;
    public float spawnHeightOffset = 0.2f;

    [Header("Optional")]
    public Transform spawnedTurtleParent;

    [Header("Manager")]
    public Level2ObjectiveManager objectiveManager;

    [Header("State")]
    public bool hasHatched = false;

    private readonly List<GameObject> spawnedTurtles = new List<GameObject>();
    private bool finishedSpawning = false;

    private void Start()
    {
        hasHatched = false;
        finishedSpawning = false;

        if (objectiveManager == null)
        {
            objectiveManager = FindObjectOfType<Level2ObjectiveManager>();
        }
    }

    public void HatchEggs()
    {
        if (hasHatched)
        {
            return;
        }

        if (babyTurtlePrefab == null)
        {
            Debug.LogError("Baby turtle prefab is missing.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No turtle spawn points assigned.");
            return;
        }

        hasHatched = true;
        StartCoroutine(SpawnTurtles());
    }

    private IEnumerator SpawnTurtles()
    {
        int spawnCount = Mathf.Min(turtlesToSpawn, spawnPoints.Length);

        for (int i = 0; i < spawnCount; i++)
        {
            Transform spawnPoint = spawnPoints[i];

            if (spawnPoint == null)
            {
                continue;
            }

            Vector3 spawnPosition = spawnPoint.position + Vector3.up * spawnHeightOffset;

            GameObject turtle = Instantiate(
                babyTurtlePrefab,
                spawnPosition,
                spawnPoint.rotation
            );

            spawnedTurtles.Add(turtle);

            if (spawnedTurtleParent != null)
            {
                turtle.transform.SetParent(spawnedTurtleParent);
            }

            BabyTurtle babyTurtle = turtle.GetComponent<BabyTurtle>();

            if (babyTurtle != null && waterTarget != null)
            {
                babyTurtle.SetTarget(waterTarget);
            }

            yield return new WaitForSeconds(delayBetweenSpawns);
        }

        finishedSpawning = true;
        StartCoroutine(CheckWhenAllTurtlesAreGone());
    }

    private IEnumerator CheckWhenAllTurtlesAreGone()
    {
        while (true)
        {
            spawnedTurtles.RemoveAll(turtle => turtle == null);

            if (finishedSpawning && spawnedTurtles.Count == 0)
            {
                if (objectiveManager != null)
                {
                    objectiveManager.CheckTurtleFailure();
                }

                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}