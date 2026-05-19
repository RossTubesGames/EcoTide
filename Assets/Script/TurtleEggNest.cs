using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleEggNest : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject babyTurtlePrefab;
    public Transform[] spawnPoints;
    public Transform waterTarget;
    public float delayBetweenSpawns = 1f;

    [Header("State")]
    public bool hasHatched = false;

    public void HatchEggs()
    {
        if (hasHatched)
        {
            return;
        }

        hasHatched = true;
        StartCoroutine(SpawnTurtles());
    }

    private IEnumerator SpawnTurtles()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject turtle = Instantiate(babyTurtlePrefab, spawnPoints[i].position, spawnPoints[i].rotation);

            BabyTurtle babyTurtle = turtle.GetComponent<BabyTurtle>();

            if (babyTurtle != null)
            {
                babyTurtle.SetTarget(waterTarget);
            }

            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }
}