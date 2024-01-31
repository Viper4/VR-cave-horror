using UnityEngine;
using UnityEngine.InputSystem;

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
        AudioListener.pause = true;
        Time.timeScale = 0;
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
        AudioListener.pause = false;
        Time.timeScale = 1;
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        pauseCanvas.enabled = false;
    }
}
