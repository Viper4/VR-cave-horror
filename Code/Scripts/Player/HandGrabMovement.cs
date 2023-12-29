using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class HandGrabMovement : MonoBehaviour
{
    CharacterController characterController;
    Rigidbody RB;
    ContinuousMoveProviderBase moveProvider;

    Hand grabbingHand;

    void Start()
    {
        moveProvider = GetComponent<ContinuousMoveProviderBase>();
        characterController = GetComponent<CharacterController>();
        RB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        if (RB != null) // Rigidbody can move freely for space
        {
            if (grabbingHand != null)
            {
                Vector3 velocity = grabbingHand.velocity;

                RB.velocity = transform.rotation * -velocity;
            }
            else
            {
                RB.angularVelocity = Vector3.zero;
            }
        }
        else // Character controller can only move in a plane better for walking
        {
            if (grabbingHand != null)
            {
                moveProvider.enabled = false;

                Vector3 velocity = grabbingHand.velocity;
                characterController.Move(transform.rotation * -velocity);
            }
            else
            {
                moveProvider.enabled = true;
            }
        }
    }

    public void AddHand(Hand hand)
    {
        grabbingHand = hand;
    }

    public void RemoveHand(Hand hand)
    {
        if (hand == grabbingHand)
            grabbingHand = null;
    }
}
