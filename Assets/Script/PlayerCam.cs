using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX = 100f;
    public float sensY = 100f;

    public Transform orientation;

    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Always fetch the current sensitivity multiplier
        float sensitivityMultiplier = PlayerPrefs.GetFloat("MouseSensitivity", 1f);

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX * sensitivityMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY * sensitivityMultiplier;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 30f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
