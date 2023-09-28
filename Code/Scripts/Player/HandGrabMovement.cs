using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class HandGrabMovement : MonoBehaviour
{
    CharacterController characterController;
    Rigidbody _rigidbody;
    ContinuousMoveProviderBase moveProvider;
    bool usingRigidbody;

    List<Hand> grabbingHands = new List<Hand>(2);
    Vector3 initialOriginEulers;
    Vector3 initialHandsDirection;

    void Start()
    {
        moveProvider = GetComponent<ContinuousMoveProviderBase>();
        if(!TryGetComponent(out characterController))
        {
            _rigidbody = GetComponent<Rigidbody>();
            usingRigidbody = true;
        }
    }

    void FixedUpdate()
    {
        if (usingRigidbody)
        {
            if (grabbingHands.Count > 0)
            {
                if (grabbingHands.Count == 1)
                {
                    _rigidbody.velocity = transform.rotation * -grabbingHands[0].velocity;
                }
                else
                {
                    Vector3 velocity = grabbingHands[0].velocity.sqrMagnitude < grabbingHands[1].velocity.sqrMagnitude ? grabbingHands[0].velocity : grabbingHands[1].velocity;
                    _rigidbody.velocity = transform.rotation * -velocity;

                    Vector3 handsDirection = grabbingHands[0].transform.localPosition - grabbingHands[1].transform.localPosition;
                    float yawSign = Mathf.Sign(Vector3.Dot(initialHandsDirection, handsDirection));
                    float targetYaw = initialOriginEulers.y + Vector3.Angle(initialHandsDirection, handsDirection) * yawSign;

                    /*float targetPitch = initialOriginEulers.x + Vector3.SignedAngle(initialHandsDirection, handsDirection, Vector3.right);
                    float targetYaw = initialOriginEulers.y + Vector3.SignedAngle(initialHandsDirection, handsDirection, Vector3.up);
                    float targetRoll = initialOriginEulers.z + Vector3.SignedAngle(initialHandsDirection, handsDirection, Vector3.forward);
                    transform.eulerAngles = new Vector3(targetPitch, targetYaw, targetRoll);*/

                    //transform.rotation = Quaternion.AngleAxis(targetYaw, Vector3.up);
                }
            }
            else
            {
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }
        else
        {
            if (grabbingHands.Count > 0)
            {
                moveProvider.enabled = false;
                characterController.Move(transform.rotation * -grabbingHands[0].velocity * Time.fixedDeltaTime);
            }
            else
            {
                moveProvider.enabled = true;
            }
        }
    }

    public void AddHand(Hand hand)
    {
        grabbingHands.Add(hand);
        initialOriginEulers = transform.eulerAngles;
        if (grabbingHands.Count > 1)
            initialHandsDirection = grabbingHands[0].transform.localPosition - grabbingHands[1].transform.localPosition;
    }

    public void RemoveHand(Hand hand)
    {
        grabbingHands.Remove(hand);
    }
}
