using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.Audio;

public class PauseUI : MonoBehaviour
{
    Canvas pauseCanvas;

    [SerializeField] AudioMixer masterMixer;
    public bool inGame = true;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;

    [SerializeField] InputActionProperty menuAction;

    Transform mainCameraTransform;
    [SerializeField] float distanceToCamera = 4;

    [SerializeField] Transform XROrigin;
    DynamicMoveProvider dynamicMoveProvider;
    TwoHandedGrabMoveProvider grabMoveProvider;
    SnapTurnProviderBase snapTurnProvider;
    ContinuousTurnProviderBase smoothTurnProvider;
    TunnelingVignetteController vignetteController;

    public static bool grabMove = true;

    private void OnEnable()
    {
        menuAction.action.performed += ToggleMenuButton;
    }

    private void OnDisable()
    {
        menuAction.action.performed -= ToggleMenuButton;
    }

    void Start()
    {
        pauseCanvas = GetComponent<Canvas>();
        mainCameraTransform = Camera.main.transform;

        dynamicMoveProvider = XROrigin.GetComponent<DynamicMoveProvider>();
        grabMoveProvider = XROrigin.GetComponent<TwoHandedGrabMoveProvider>();
        snapTurnProvider = XROrigin.GetComponent<SnapTurnProviderBase>();
        smoothTurnProvider = XROrigin.GetComponent<ContinuousTurnProviderBase>();
        vignetteController = Camera.main.GetComponent<TunnelingVignetteController>();
    }

    void Update()
    {
        
    }

    public void ToggleMenuButton(InputAction.CallbackContext callbackContext)
    {
        if (inGame)
        {
            if (pauseCanvas.enabled)
                Resume();
            else
                Pause();
        }
    }

    private void Pause()
    {
        Vector3 flatCameraForward = mainCameraTransform.forward;
        flatCameraForward.y = 0;
        flatCameraForward.Normalize();
        if (flatCameraForward == Vector3.zero)
            flatCameraForward = Vector3.forward;
        pauseCanvas.transform.SetPositionAndRotation(mainCameraTransform.position + (flatCameraForward * distanceToCamera), Quaternion.LookRotation(flatCameraForward, Vector3.up));

        pauseCanvas.enabled = true;
    }

    public void Resume() // In game
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        pauseCanvas.enabled = false;
    }

    public void Settings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        if (settingsMenu.activeSelf)
        {
            mainMenu.SetActive(true);
            settingsMenu.SetActive(false);
        }
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
