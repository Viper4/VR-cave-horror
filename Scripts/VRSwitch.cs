using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class VRSwitch : MonoBehaviour
{
    XRBaseInteractable interactable;

    public uint currentState = 0;
    [SerializeField] Transform[] states;
    [SerializeField] bool changeCurrentState = true;

    void OnEnable()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(SelectEnter);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(SelectEnter);
    }

    void Start()
    {
        transform.SetPositionAndRotation(states[currentState].transform.position, states[currentState].transform.rotation);
    }

    void SelectEnter(SelectEnterEventArgs args)
    {
        if (changeCurrentState)
        {
            currentState++;
            if (currentState >= states.Length)
                currentState = 0;
        }
        transform.SetPositionAndRotation(states[currentState].transform.position, states[currentState].transform.rotation);
    }
}
