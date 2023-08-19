using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRButton : XRBaseInteractable
{
    [SerializeField] Transform normalState;
    [SerializeField] Transform pressedState;
    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] float rotateSpeed = 3;
    bool selected = false;

    private void Update()
    {
        if(selected)
            transform.SetPositionAndRotation(
                Vector3.MoveTowards(transform.position, pressedState.position, moveSpeed * Time.deltaTime),
                Quaternion.RotateTowards(transform.rotation, pressedState.rotation, rotateSpeed * Time.deltaTime));
        else
            transform.SetPositionAndRotation(
                Vector3.MoveTowards(transform.position, normalState.position, moveSpeed * Time.deltaTime),
                Quaternion.RotateTowards(transform.rotation, normalState.rotation, rotateSpeed * Time.deltaTime));
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        selected = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        selected = false;
    }
}
