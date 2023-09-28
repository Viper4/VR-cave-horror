using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.Audio;

public class PauseUI : BaseUI
{
    Canvas pauseCanvas;

    [SerializeField] InputActionProperty menuAction;

    Transform mainCameraTransform;
    [SerializeField] float distanceToCamera = 4;

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
    }

    public void ToggleMenuButton(InputAction.CallbackContext callbackContext)
    {
        if (pauseCanvas.enabled)
            Resume();
        else
            Pause();
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
}
