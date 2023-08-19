using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class Climber : MonoBehaviour
{
    [SerializeField] InputActionProperty leftVelocityAction;
    [SerializeField] InputActionProperty rightVelocityAction;

    public static Hand climbingHand;

    CharacterController characterController;
    ContinuousMoveProviderBase moveProvider;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        moveProvider = GetComponent<ContinuousMoveProviderBase>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (climbingHand != null)
        {
            moveProvider.enabled = false;
            characterController.Move(transform.rotation * -climbingHand.velocity * Time.fixedDeltaTime);
        }
        else
        {
            moveProvider.enabled = true;
        }
    }
}
