using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class RoverController : MonoBehaviour
{
    XRBaseInteractable interactable;
    Collider _collider;

    [SerializeField] bool active = false;
    [SerializeField] Canvas screenCanvas;
    [SerializeField] Light screenLight;
    [SerializeField] Color[] lightColors;
    [SerializeField] Slider batterySlider;
    [SerializeField] Joystick joystick;

    [SerializeField] CaveRover rover;
    bool roverConnected = true;
    bool headlightsOn = true;

    uint previousCamera = 0;
    [SerializeField] GameObject[] cameras;
    bool[] cameraStates = new bool[] { true, true, true };
    [SerializeField] TextMeshProUGUI depthText;
    [SerializeField] Image noSignalImage;
    [SerializeField] TextMeshProUGUI noSignalTitle;
    [SerializeField] TextMeshProUGUI noSignalStatusText;

    bool audioConnected = true;

    private void OnEnable()
    {
        interactable = GetComponent<XRBaseInteractable>();
        _collider = GetComponent<Collider>();

        interactable.selectEntered.AddListener(SelectEnter);
        interactable.selectExited.AddListener(SelectExit);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(SelectEnter);
        interactable.selectExited.AddListener(SelectExit);
    }


    void Start()
    {
        
    }

    void Update()
    {
        float depth = Mathf.Round(-rover.transform.position.y * 100) / 100;
        if (depth < 0)
            depth = 0;
        depthText.text = depth.ToString() + "m";
    }

    public void TogglePower()
    {
        active = !active;
        screenCanvas.gameObject.SetActive(active);
        screenLight.enabled = active;
    }

    public void ToggleHeadlights()
    {
        if (active)
        {
            headlightsOn = !headlightsOn;
            rover.ToggleLights(headlightsOn);
        }
    }

    public void OnInteractButtonPress()
    {
        if (active)
        {
            rover.ArmInteract();
        }
    }

    void SelectEnter(SelectEnterEventArgs args)
    {
        _collider.enabled = false;
        args.interactorObject.transform.parent.GetComponentInChildren<Hand>().transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = false;
    }

    void SelectExit(SelectExitEventArgs args)
    {
        _collider.enabled = true;
        args.interactorObject.transform.parent.GetComponentInChildren<Hand>().transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = true;
        joystick.DropControl();
    }

    public void SwitchCamera(VRSwitch vrSwitch)
    {
        vrSwitch.currentState++;
        if (vrSwitch.currentState >= cameraStates.Length)
            vrSwitch.currentState = 0; 
        cameras[previousCamera].SetActive(false);
        if (roverConnected && cameraStates[vrSwitch.currentState])
        {
            if (audioConnected)
            {
                noSignalImage.gameObject.SetActive(false);
            }
            else
            {
                noSignalImage.enabled = false;
                noSignalTitle.text = "No Audio";
                noSignalStatusText.enabled = false;
                noSignalImage.gameObject.SetActive(true);
            }
            screenLight.color = lightColors[vrSwitch.currentState];
            cameras[vrSwitch.currentState].SetActive(true);
        }
        else
        {
            noSignalTitle.text = roverConnected ? "No Video" : "No Signal";

            string roverStatus = roverConnected ? "Controls (SR7): <color=green>OK</color>" : "Controls (SR7): <color=red>001408b (Failed to connect) 0</color>";
            string cameraOneStatus = roverConnected && cameraStates[0] ? "\nCam1 (Normal): <color=green> OK</color>" : "\nCam1 (Normal): <color=red>001408b (Failed to connect) 0</color>";
            string cameraTwoStatus = roverConnected && cameraStates[1] ? "\nCam2 (NV): <color=green> OK</color>" : "\nCam2 (NV): <color=red>001408b (Failed to connect) 0</color>";
            string cameraThreeStatus = roverConnected && cameraStates[2] ? "\nCam3 (TC): <color=green> OK</color>" : "\nCam3 (TC): <color=red>001408b (Failed to connect) 0</color>";
            string audioStatus = roverConnected && audioConnected ? "\nAudio: <color=green>OK</color>" : "\nAudio: <color=red>001408b (Failed to connect) 0</color>";

            noSignalStatusText.text = roverStatus + cameraOneStatus + cameraTwoStatus + cameraThreeStatus + audioStatus;
            noSignalStatusText.enabled = true;
            screenLight.color = Color.grey;
            noSignalImage.gameObject.SetActive(true);
        }

        previousCamera = vrSwitch.currentState;
    }
}
