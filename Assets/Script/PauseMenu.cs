using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI;
    public Slider sensitivitySlider;
    public Slider volumeSlider;

    private bool isPaused = false;

    private const string MouseSensitivityKey = "MouseSensitivity";
    private const string MasterVolumeKey = "MasterVolume";

    private void Start()
    {
        SetupSensitivitySlider();
        SetupVolumeSlider();

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        Time.timeScale = 1f;
        LockCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(isPaused);
        }

        Time.timeScale = isPaused ? 0f : 1f;

        if (isPaused)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    private void SetupSensitivitySlider()
    {
        if (sensitivitySlider == null)
        {
            Debug.LogWarning("Sensitivity Slider is not assigned.");
            return;
        }

        float savedSensitivity = PlayerPrefs.GetFloat(MouseSensitivityKey, 1f);

        sensitivitySlider.value = savedSensitivity;
        sensitivitySlider.onValueChanged.RemoveListener(SetSensitivity);
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);

        SetSensitivity(savedSensitivity);
    }

    private void SetupVolumeSlider()
    {
        if (volumeSlider == null)
        {
            Debug.LogWarning("Volume Slider is not assigned.");
            return;
        }

        float savedVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);

        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.value = savedVolume;

        volumeSlider.onValueChanged.RemoveListener(SetVolume);
        volumeSlider.onValueChanged.AddListener(SetVolume);

        SetVolume(savedVolume);
    }

    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat(MouseSensitivityKey, value);
        PlayerPrefs.Save();
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;

        PlayerPrefs.SetFloat(MasterVolumeKey, value);
        PlayerPrefs.Save();
    }

    public static float GetSensitivity()
    {
        return PlayerPrefs.GetFloat(MouseSensitivityKey, 1f);
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}