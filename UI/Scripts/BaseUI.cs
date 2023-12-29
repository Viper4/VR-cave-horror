using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.Audio;

public class BaseUI : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;

    public GameObject mainMenu;
    public GameObject settingsMenu;

    [SerializeField] Transform XROrigin;
    DynamicMoveProvider dynamicMoveProvider;
    SnapTurnProviderBase snapTurnProvider;
    ContinuousTurnProviderBase smoothTurnProvider;
    TunnelingVignetteController vignetteController;

    [SerializeField] ActionBasedControllerManager rightHandControllerManager;

    void Start()
    {
        dynamicMoveProvider = XROrigin.GetComponent<DynamicMoveProvider>();
        snapTurnProvider = XROrigin.GetComponent<SnapTurnProviderBase>();
        smoothTurnProvider = XROrigin.GetComponent<ContinuousTurnProviderBase>();
        vignetteController = Camera.main.GetComponent<TunnelingVignetteController>();
    }

    public void Quit()
    {
        Player.instance.SaveData();
        Application.Quit();
    }

    public void ChangeMasterVolume(Slider slider)
    {
        masterMixer.SetFloat("masterVolume", Mathf.Log(slider.value) * 20);
        slider.transform.Find("Value Text").GetComponent<TextMeshProUGUI>().text = Mathf.Round(slider.value * 100).ToString();
    }

    public void ChangeMusicVolume(Slider slider)
    {
        masterMixer.SetFloat("musicVolume", Mathf.Log(slider.value) * 20);
        slider.transform.Find("Value Text").GetComponent<TextMeshProUGUI>().text = Mathf.Round(slider.value * 100).ToString();
    }

    public void ChangeAmbientVolume(Slider slider)
    {
        masterMixer.SetFloat("ambientVolume", Mathf.Log(slider.value) * 20);
        slider.transform.Find("Value Text").GetComponent<TextMeshProUGUI>().text = Mathf.Round(slider.value * 100).ToString();
    }

    public void ChangeSfxVolume(Slider slider)
    {
        masterMixer.SetFloat("sfxVolume", Mathf.Log(slider.value) * 20);
        slider.transform.Find("Value Text").GetComponent<TextMeshProUGUI>().text = Mathf.Round(slider.value * 100).ToString();
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
        rightHandControllerManager.smoothTurnEnabled = toggle.isOn;
    }

    public void ChangeTurnSpeed(Slider slider)
    {
        smoothTurnProvider.turnSpeed = slider.value;
        slider.transform.Find("Value Text").GetComponent<TextMeshProUGUI>().text = slider.value.ToString() + " °/s";
    }
}
