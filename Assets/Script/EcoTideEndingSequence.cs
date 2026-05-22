using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EcoTideEndingSequence : MonoBehaviour
{
    [Header("Player/UI")]
    public GameObject player;
    public GameObject gameplayUI;

    [Header("Cameras")]
    public Camera endingCamera;
    public Camera playerCamera;

    public Transform cameraStartPoint;
    public Transform cameraEndPoint;
    public float cameraMoveTime = 8f;

    [Header("Ending Objects")]
    public GameObject dolphinJumpGroup;
    public GameObject fishJumpGroup;
    public TextMeshProUGUI endingText;

    [Header("Failure Settings")]
    public string objectiveFailedMessage = "Objective Failed";
    public bool autoRestartAfterFailure = false;
    public float restartDelayAfterFailure = 10f;

    private bool playFailureSequence = false;
    private bool sequenceStarted = false;

    private void OnEnable()
    {
        if (sequenceStarted)
        {
            return;
        }

        sequenceStarted = true;

        if (playFailureSequence)
        {
            StartCoroutine(FailureRoutine());
        }
        else
        {
            StartCoroutine(EndingRoutine());
        }
    }

    public void PlayFailureAndRestart()
    {
        playFailureSequence = true;

        ForceUnlockCursor();

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        else if (!sequenceStarted)
        {
            sequenceStarted = true;
            StartCoroutine(FailureRoutine());
        }
    }

    private IEnumerator FailureRoutine()
    {
        Time.timeScale = 1f;

        ForceUnlockCursor();

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

        DisablePlayer();
        ForceUnlockCursor();

        if (dolphinJumpGroup != null)
        {
            dolphinJumpGroup.SetActive(false);
        }

        if (fishJumpGroup != null)
        {
            fishJumpGroup.SetActive(false);
        }

        if (endingText != null)
        {
            endingText.text = objectiveFailedMessage;
            endingText.gameObject.SetActive(true);
        }

        ForceUnlockCursor();

        if (autoRestartAfterFailure)
        {
            yield return new WaitForSeconds(restartDelayAfterFailure);

            Time.timeScale = 1f;

            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }

    private IEnumerator EndingRoutine()
    {
        Time.timeScale = 1f;

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

        DisablePlayer();

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

        ForceUnlockCursor();
    }

    private void DisablePlayer()
    {
        if (player == null)
        {
            return;
        }

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

    private void ForceUnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}