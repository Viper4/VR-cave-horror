using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class RoverController : MonoBehaviour
{
    XRGrabInteractable interactable;

    [SerializeField] bool active = false;

    [SerializeField] Canvas screenCanvas;
    [SerializeField] Light screenLight;
    [SerializeField] Color[] lightColors;
    [SerializeField] Slider batterySlider;
    [SerializeField] Joystick joystick;

    [SerializeField] CaveRover rover;
    bool headlightsOn = true;
    [SerializeField] float maxConnectDistance = 30;
    [SerializeField] Image staticImage;

    uint currentCamera = 0;
    [SerializeField] GameObject[] cameras;
    bool[] cameraStates = new bool[] { true, true, true };
    [SerializeField] TextMeshProUGUI depthText;
    [SerializeField] Image noSignalImage;
    [SerializeField] TextMeshProUGUI noSignalTitle;
    [SerializeField] TextMeshProUGUI noSignalStatusText;

    bool audioConnected = true;

    void OnEnable()
    {
        interactable = GetComponent<XRGrabInteractable>();

        interactable.hoverEntered.AddListener(HoverEnter);
        interactable.hoverExited.AddListener(HoverExit);

        interactable.selectEntered.AddListener(SelectEnter);
        interactable.selectExited.AddListener(SelectExit);
    }

    private void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(HoverEnter);
        interactable.hoverExited.RemoveListener(HoverExit);

        interactable.selectEntered.RemoveListener(SelectEnter);
        interactable.selectExited.RemoveListener(SelectExit);
    }

    private void HoverEnter(HoverEnterEventArgs args)
    {
        args.interactorObject.transform.GetComponent<XRBaseControllerInteractor>().selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.Toggle;
    }

    private void HoverExit(HoverExitEventArgs args)
    {
        if (interactable.firstInteractorSelecting == null || interactable.firstInteractorSelecting.transform != args.interactorObject.transform)
            args.interactorObject.transform.GetComponent<XRBaseControllerInteractor>().selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.StateChange;
    }

    private void SelectEnter(SelectEnterEventArgs args)
    {

    }

    private void SelectExit(SelectExitEventArgs args)
    {
        args.interactorObject.transform.GetComponent<XRBaseControllerInteractor>().selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.StateChange;
    }

    void Update()
    {
        float depth = Mathf.Round(-rover.transform.position.y * 100) / 100;
        if (depth < 0)
            depth = 0;
        depthText.text = depth.ToString() + "m";

        if (rover.nearbyConnections.Count > 0 && cameraStates[currentCamera])
            staticImage.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, rover.connectionDistance / (maxConnectDistance * maxConnectDistance)));
        else
            staticImage.color = new Color(1, 1, 1, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rover"))
        {
            if(!rover.nearbyConnections.Contains(transform))
                rover.nearbyConnections.Add(transform);
            UpdateConnection();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rover"))
        {
            rover.nearbyConnections.Remove(transform);
            UpdateConnection();
        }
    }

    public void TogglePower()
    {
        active = !active;
        screenCanvas.gameObject.SetActive(active);
        screenLight.enabled = active;
        UpdateConnection();
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

    public void UpdateConnection()
    {
        string roverStatus;
        string[] cameraStatus = new string[3];
        string audioStatus;
        if (rover.nearbyConnections.Count == 0)
        {
            noSignalTitle.text = "No Signal";
            roverStatus = "Controls (SR7): <color=red>001408b (Failed to connect) 0</color>";
            cameraStatus[0] = "\nCam1 (Normal): <color=red>001408b (Failed to connect) 0</color>";
            cameraStatus[1] = "\nCam2 (NV): <color=red>001408b (Failed to connect) 0</color>";
            cameraStatus[2] = "\nCam3 (IR): <color=red>001408b (Failed to connect) 0</color>";
            audioStatus = "\nAudio: <color=red>001408b (Failed to connect) 0</color>";

            noSignalImage.enabled = true;
            noSignalStatusText.text = roverStatus + cameraStatus[0] + cameraStatus[1] + cameraStatus[2] + audioStatus;
            noSignalStatusText.enabled = true;
            screenLight.color = Color.grey;
            noSignalImage.gameObject.SetActive(true);
        }
        else
        {
            if (cameraStates[currentCamera])
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
                cameras[currentCamera].SetActive(true);
            }
            else
            {
                noSignalTitle.text = "No Video";
                roverStatus = "Controls (SR7): <color=green>OK</color>";
                cameraStatus[0] = cameraStates[0] ? "\nCam1 (Normal): <color=green> OK</color>" : "\nCam1 (Normal): <color=red>001408b (Failed to connect) 0</color>";
                cameraStatus[1] = cameraStates[1] ? "\nCam2 (NV): <color=green> OK</color>" : "\nCam2 (NV): <color=red>001408b (Failed to connect) 0</color>";
                cameraStatus[2] = cameraStates[2] ? "\nCam3 (IR): <color=green> OK</color>" : "\nCam3 (IR): <color=red>001408b (Failed to connect) 0</color>";
                audioStatus = audioConnected ? "\nAudio: <color=green>OK</color>" : "\nAudio: <color=red>001408b (Failed to connect) 0</color>";

                noSignalImage.enabled = true;
                noSignalStatusText.text = roverStatus + cameraStatus[0] + cameraStatus[1] + cameraStatus[2] + audioStatus;
                noSignalStatusText.enabled = true;
                screenLight.color = Color.grey;
                noSignalImage.gameObject.SetActive(true);
            }
        }
    }

    public void SwitchCamera(VRSwitch vrSwitch)
    {
        vrSwitch.currentState++;
        if (vrSwitch.currentState >= cameraStates.Length)
            vrSwitch.currentState = 0; 
        cameras[currentCamera].SetActive(false);
        screenLight.color = lightColors[vrSwitch.currentState];

        currentCamera = vrSwitch.currentState;

        UpdateConnection();
    }
}
