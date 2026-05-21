using UnityEngine;

public class FishGroupJump : MonoBehaviour
{
    [Header("Jump Points")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Jump Settings")]
    public float jumpDuration = 4f;
    public float jumpHeight = 6f;
    public bool loop = true;

    [Header("Rotation")]
    public bool faceMoveDirection = true;

    private float timer;

    private void Start()
    {
        if (startPoint != null)
        {
            transform.position = startPoint.position;
        }
    }

    private void Update()
    {
        if (startPoint == null || endPoint == null)
        {
            return;
        }

        timer += Time.deltaTime;

        float t = timer / jumpDuration;

        if (t >= 1f)
        {
            if (loop)
            {
                timer = 0f;
                t = 0f;
            }
            else
            {
                t = 1f;
            }
        }

        Vector3 flatPosition = Vector3.Lerp(startPoint.position, endPoint.position, t);

        float arc = Mathf.Sin(t * Mathf.PI) * jumpHeight;

        Vector3 finalPosition = new Vector3(
            flatPosition.x,
            flatPosition.y + arc,
            flatPosition.z
        );

        if (faceMoveDirection)
        {
            Vector3 direction = endPoint.position - startPoint.position;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        transform.position = finalPosition;
    }
}