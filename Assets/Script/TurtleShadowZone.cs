using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleShadowZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        BabyTurtle turtle = other.GetComponent<BabyTurtle>();

        if (turtle != null)
        {
            turtle.SetShadowState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BabyTurtle turtle = other.GetComponent<BabyTurtle>();

        if (turtle != null)
        {
            turtle.SetShadowState(false);
        }
    }
}