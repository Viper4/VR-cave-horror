using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Joystick : XRBaseInteractable
{
    [SerializeField] Transform baseTransform;
    [SerializeField] Transform pivot;

    [SerializeField] float maxDistance = 0.25f;
    [SerializeField] float rotateSpeed = 10;
    [SerializeField] float maxAngle = 35;
    Transform interactingHand = null;

    public Vector3 direction { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(interactingHand != null)
        {
            Vector3 localGrabPosition = baseTransform.InverseTransformPoint(interactingHand.position);
            direction = new Vector3(Mathf.Clamp(localGrabPosition.x, -maxDistance, maxDistance) / maxDistance, 0, Mathf.Clamp(localGrabPosition.z, -maxDistance, maxDistance) / maxDistance);
            pivot.localEulerAngles = new Vector3(direction.z * maxAngle, 0, -direction.x * maxAngle);
        }
        else
        {
            pivot.rotation = Quaternion.RotateTowards(pivot.rotation, baseTransform.rotation, rotateSpeed * Time.deltaTime);
            direction = Vector3.zero;
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        interactingHand = args.interactorObject.transform;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        interactingHand = null;
    }
}
