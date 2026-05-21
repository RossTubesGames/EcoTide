using System.Collections;
using TMPro;
using UnityEngine;

public class EcoTideEndingSequence : MonoBehaviour
{
    public GameObject player;
    public GameObject gameplayUI;

    public Camera endingCamera;
    public Camera playerCamera;

    public Transform cameraStartPoint;
    public Transform cameraEndPoint;
    public float cameraMoveTime = 8f;

    public GameObject dolphinJumpGroup;
    public GameObject fishJumpGroup;
    public TextMeshProUGUI endingText;

    private void OnEnable()
    {
        StartCoroutine(EndingRoutine());
    }

    private IEnumerator EndingRoutine()
    {
        if (gameplayUI != null)
        {
            gameplayUI.SetActive(false);
        }

        if (playerCamera != null)
        {
            playerCamera.enabled = false;
        }

        if (endingCamera != null)
        {
            endingCamera.gameObject.SetActive(true);
            endingCamera.enabled = true;

            if (cameraStartPoint != null)
            {
                endingCamera.transform.position = cameraStartPoint.position;
                endingCamera.transform.rotation = cameraStartPoint.rotation;
            }
        }

        if (player != null)
        {
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.enabled = false;
            }

            PlayerInteraction interaction = player.GetComponent<PlayerInteraction>();
            if (interaction != null)
            {
                interaction.enabled = false;
            }

            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }

        if (dolphinJumpGroup != null)
        {
            dolphinJumpGroup.SetActive(true);
        }

        if (fishJumpGroup != null)
        {
            fishJumpGroup.SetActive(true);
        }

        if (endingText != null)
        {
            endingText.gameObject.SetActive(false);
        }

        float timer = 0f;

        while (timer < cameraMoveTime)
        {
            timer += Time.deltaTime;
            float t = timer / cameraMoveTime;

            if (endingCamera != null && cameraStartPoint != null && cameraEndPoint != null)
            {
                endingCamera.transform.position = Vector3.Lerp(cameraStartPoint.position, cameraEndPoint.position, t);
                endingCamera.transform.rotation = Quaternion.Slerp(cameraStartPoint.rotation, cameraEndPoint.rotation, t);
            }

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        if (endingText != null)
        {
            endingText.text = "Thank you for playing EcoTide";
            endingText.gameObject.SetActive(true);
        }
    }
}