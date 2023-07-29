using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class VRButton : MonoBehaviour
{
    XRBaseInteractable interactable;

    [SerializeField] Transform normalState;
    [SerializeField] Transform pressedState;
    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] float rotateSpeed = 3;

    void OnEnable()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(SelectEnter);
        interactable.selectExited.AddListener(SelectExit);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(SelectEnter);
        interactable.selectExited.RemoveListener(SelectExit);
    }

    void SelectEnter(SelectEnterEventArgs args)
    {
        transform.SetPositionAndRotation(
            Vector3.MoveTowards(transform.position, pressedState.position, moveSpeed * Time.deltaTime), 
            Quaternion.RotateTowards(transform.rotation, pressedState.rotation, rotateSpeed * Time.deltaTime));
    }

    void SelectExit(SelectExitEventArgs args)
    {
        transform.SetPositionAndRotation(
            Vector3.MoveTowards(transform.position, normalState.position, moveSpeed * Time.deltaTime),
            Quaternion.RotateTowards(transform.rotation, normalState.rotation, rotateSpeed * Time.deltaTime));
    }
}
