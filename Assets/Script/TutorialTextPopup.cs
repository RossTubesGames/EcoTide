using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialTextPopup : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI tutorialText;

    [Header("Watched Object")]
    public GameObject watchedObject;

    [Header("Message")]
    [TextArea(2, 5)]
    public string message;

    public float showTime = 5f;

    private bool hasShown = false;

    private void Start()
    {
        if (tutorialText != null)
        {
            tutorialText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (hasShown)
        {
            return;
        }

        if (watchedObject != null && watchedObject.activeInHierarchy)
        {
            ShowMessage();
        }
    }

    public void ShowMessage()
    {
        if (hasShown)
        {
            return;
        }

        hasShown = true;
        StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        if (tutorialText != null)
        {
            tutorialText.text = message;
            tutorialText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(showTime);

        if (tutorialText != null)
        {
            tutorialText.gameObject.SetActive(false);
        }
    }
}