using UnityEngine;

public class TurtleShadowZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        BabyTurtle turtle = other.GetComponentInParent<BabyTurtle>();

        if (turtle != null)
        {
            turtle.SetShadowState(true);
            Debug.Log("Turtle entered shadow.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BabyTurtle turtle = other.GetComponentInParent<BabyTurtle>();

        if (turtle != null)
        {
            turtle.SetShadowState(false);
            Debug.Log("Turtle left shadow.");
        }
    }
}