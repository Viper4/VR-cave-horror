using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class LeverInteractable : XRBaseInteractable
{
    enum Axis { X, Y, Z, None };
    [SerializeField] Axis xDistanceMap = Axis.X;
    [SerializeField] Axis yDistanceMap = Axis.Y;
    [SerializeField] Axis zDistanceMap = Axis.Z;

    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;
    [SerializeField] float startAngle;
    [SerializeField] float endAngle;
    [SerializeField] float stabilizeSpeed = 20;

    [SerializeField] Transform pivot;
    [SerializeField] Transform stableTransform;
    [SerializeField] Vector3[] triggerRotations;
    [SerializeField] float minTriggerAngle = 5;
    [SerializeField] bool snapToTrigger;

    public UnityEvent[] triggers;

    Transform interactingHand;

    private void Start()
    {
        if(endAngle < startAngle)
        {
            Debug.LogWarning("Lever wont rotate correctly for " + transform.name + " because its end angle is less than the start angle");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (interactingHand != null)
        {
            Vector3 localGrabPosition = stableTransform.InverseTransformPoint(interactingHand.position);
            float xDistance = (Mathf.Clamp(localGrabPosition.x, minDistance, maxDistance) - minDistance) / (maxDistance - minDistance);
            float yDistance = (Mathf.Clamp(localGrabPosition.y, minDistance, maxDistance) - minDistance) / (maxDistance - minDistance);
            float zDistance = (Mathf.Clamp(localGrabPosition.z, minDistance, maxDistance) - minDistance) / (maxDistance - minDistance);

            float xAngle = pivot.localEulerAngles.x;
            float yAngle = pivot.localEulerAngles.y;
            float zAngle = pivot.localEulerAngles.z;

            switch (xDistanceMap)
            {
                case Axis.X:
                    xAngle = Mathf.Lerp(startAngle, endAngle, xDistance);
                    break;
                case Axis.Y:
                    yAngle = Mathf.Lerp(startAngle, endAngle, xDistance);
                    break;
                case Axis.Z:
                    zAngle = Mathf.Lerp(startAngle, endAngle, xDistance);
                    break;
            }
            switch (yDistanceMap)
            {
                case Axis.X:
                    xAngle = Mathf.Lerp(startAngle, endAngle, yDistance);
                    break;
                case Axis.Y:
                    yAngle = Mathf.Lerp(startAngle, endAngle, yDistance);
                    break;
                case Axis.Z:
                    zAngle = Mathf.Lerp(startAngle, endAngle, yDistance);
                    break;
            }
            switch (zDistanceMap)
            {
                case Axis.X:
                    xAngle = Mathf.Lerp(startAngle, endAngle, zDistance);
                    break;
                case Axis.Y:
                    yAngle = Mathf.Lerp(startAngle, endAngle, zDistance);
                    break;
                case Axis.Z:
                    zAngle = Mathf.Lerp(startAngle, endAngle, zDistance);
                    break;
            }

            pivot.localEulerAngles = new Vector3(xAngle, yAngle, zAngle);
        }
        else if (stabilizeSpeed > 0)
        {
            pivot.rotation = Quaternion.RotateTowards(pivot.rotation, stableTransform.rotation, stabilizeSpeed * Time.deltaTime);
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
        if(minTriggerAngle > 0)
        {
            for (int i = 0; i < triggerRotations.Length; i++)
            {
                if (Quaternion.Angle(pivot.localRotation, Quaternion.Euler(triggerRotations[i])) < minTriggerAngle)
                {
                    triggers[i].Invoke();
                    if (snapToTrigger)
                        pivot.localEulerAngles = triggerRotations[i];
                    break;
                }
            }
        }
    }
}
