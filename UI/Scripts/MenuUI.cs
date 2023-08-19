using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.Audio;

public class MenuUI : MonoBehaviour
{
    [SerializeField] GameObject menuCanvas;
    [SerializeField] AudioMixer masterMixer;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject creditsMenu;

    [SerializeField] Transform XROrigin;
    DynamicMoveProvider dynamicMoveProvider;
    TwoHandedGrabMoveProvider grabMoveProvider;
    SnapTurnProviderBase snapTurnProvider;
    ContinuousTurnProviderBase smoothTurnProvider;
    TunnelingVignetteController vignetteController;

    public static bool grabMove = true;

    [SerializeField] PauseUI pauseUI;

    void Start()
    {
        dynamicMoveProvider = XROrigin.GetComponent<DynamicMoveProvider>();
        grabMoveProvider = XROrigin.GetComponent<TwoHandedGrabMoveProvider>();
        snapTurnProvider = XROrigin.GetComponent<SnapTurnProviderBase>();
        smoothTurnProvider = XROrigin.GetComponent<ContinuousTurnProviderBase>();
        vignetteController = Camera.main.GetComponent<TunnelingVignetteController>();
    }

    public void NewGame()
    {
        menuCanvas.SetActive(false);
        pauseUI.inGame = true;
    }

    public void ResumeGame()
    {
        menuCanvas.SetActive(false);
        pauseUI.inGame = true;
    }

    public void Settings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void OpenURL(string linkURL)
    {
        Application.OpenURL(linkURL);
    }

    public void ChangeMasterVolume(Slider slider)
    {
        masterMixer.SetFloat("masterVolume", slider.value);
        slider.transform.Find("Value Text").GetComponent<TextMeshProUGUI>().text = slider.value.ToString();
    }

    public void ChangeMusicVolume(Slider slider)
    {
        masterMixer.SetFloat("musicVolume", slider.value);
        slider.transform.Find("Value Text").GetComponent<TextMeshProUGUI>().text = slider.value.ToString();
    }

    public void ChangeAmbientVolume(Slider slider)
    {
        masterMixer.SetFloat("ambientVolume", slider.value);
        slider.transform.Find("Value Text").GetComponent<TextMeshProUGUI>().text = slider.value.ToString();
    }

    public void ChangeSfxVolume(Slider slider)
    {
        masterMixer.SetFloat("sfxVolume", slider.value);
        slider.transform.Find("Value Text").GetComponent<TextMeshProUGUI>().text = slider.value.ToString();
    }

    // Movement
    public void ToggleMovementDirection(Toggle toggle)
    {
        if (toggle.isOn)
        {
            dynamicMoveProvider.leftHandMovementDirection = DynamicMoveProvider.MovementDirection.HandRelative;
            dynamicMoveProvider.rightHandMovementDirection = DynamicMoveProvider.MovementDirection.HandRelative;
        }
        else
        {
            dynamicMoveProvider.leftHandMovementDirection = DynamicMoveProvider.MovementDirection.HeadRelative;
            dynamicMoveProvider.rightHandMovementDirection = DynamicMoveProvider.MovementDirection.HeadRelative;
        }
    }

    public void ToggleGrabMove(Toggle toggle)
    {
        grabMoveProvider.leftGrabMoveProvider.enabled = toggle.isOn;
        grabMoveProvider.rightGrabMoveProvider.enabled = toggle.isOn;
        grabMove = toggle.isOn;
    }

    public void ToggleVisionTunneling(Toggle toggle)
    {
        vignetteController.enabled = toggle.isOn;
    }

    // Turning
    public void ToggleSnapTurn(Toggle toggle)
    {
        snapTurnProvider.enabled = toggle.isOn;
    }

    public void ChangeTurnAngle(Slider slider)
    {
        snapTurnProvider.turnAmount = slider.value;
        slider.transform.Find("Value Text").GetComponent<TextMeshProUGUI>().text = slider.value.ToString() + "°";
    }

    public void ToggleSmoothTurn(Toggle toggle)
    {
        grabMoveProvider.rightGrabMoveProvider.GetComponent<ActionBasedControllerManager>().smoothTurnEnabled = toggle.isOn;
    }

    public void ChangeTurnSpeed(Slider slider)
    {
        smoothTurnProvider.turnSpeed = slider.value;
        slider.transform.Find("Value Text").GetComponent<TextMeshProUGUI>().text = slider.value.ToString() + " °/s";
    }
}
