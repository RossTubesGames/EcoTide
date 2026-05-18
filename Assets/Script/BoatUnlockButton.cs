using System.Collections;
using TMPro;
using UnityEngine;

public class BoatUnlockButton : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI interactionText;

    [Header("Boats")]
    public GameObject BigBoat;
    public GameObject SmallBoat;

    [Header("Interaction")]
    public Transform player;
    public Transform button;
    public float interactionRange = 3f;

    [Header("Cost")]
    public int moneyAmount;

    private bool boatUnlocked = false;

    private void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }

        if (BigBoat != null)
        {
            BigBoat.SetActive(false);
        }

        if (SmallBoat != null)
        {
            SmallBoat.SetActive(true);
        }
    }

    private void Update()
    {
        if (boatUnlocked)
        {
            return;
        }

        if (interactionText == null || button == null || player == null)
        {
            return;
        }

        float distance = Vector3.Distance(button.position, player.position);
        bool isNearButton = distance <= interactionRange;

        if (!isNearButton)
        {
            interactionText.gameObject.SetActive(false);
            return;
        }

        interactionText.gameObject.SetActive(true);
        interactionText.text = "Press F to Unlock Big Boat for $" + moneyAmount;

        if (Input.GetKeyDown(KeyCode.F))
        {
            TryUnlockBoat();
        }
    }

    private void TryUnlockBoat()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        if (GameManager.Instance.Money < moneyAmount)
        {
            interactionText.text = "Not enough money!";
            return;
        }

        if (!GameManager.Instance.SpendMoney(moneyAmount))
        {
            return;
        }

        boatUnlocked = true;

        if (BigBoat != null)
        {
            BigBoat.SetActive(true);
        }

        if (SmallBoat != null)
        {
            Destroy(SmallBoat);
        }

        interactionText.text = "Boat Unlocked!";
        StartCoroutine(HideTextAndDestroyButton());
    }

    private IEnumerator HideTextAndDestroyButton()
    {
        yield return new WaitForSeconds(3f);

        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }

        if (button != null)
        {
            Destroy(button.gameObject);
        }

        Destroy(gameObject);
    }
}