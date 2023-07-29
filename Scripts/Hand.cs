using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Hand : MonoBehaviour
{
    Transform XROrigin;
    ContinuousMoveProviderBase moveProvider;

    [SerializeField] InputActionProperty triggerAction;
    [SerializeField] InputActionProperty gripAction;
    [SerializeField] InputActionProperty primaryAxisTouchAction;
    [SerializeField] InputActionProperty primaryAxisClickAction;

    [SerializeField] float walkSpeed = 1;
    [SerializeField] float runSpeed = 3;

    Animator animator;

    public float triggerValue { get; private set; }
    public float gripValue { get; private set; }
    public Vector2 primaryAxisPosition { get; private set; }

    private void OnEnable()
    {
        primaryAxisClickAction.action.performed += PrimaryAxisClick;
    }

    private void OnDisable()
    {
        primaryAxisClickAction.action.performed -= PrimaryAxisClick;
    }

    // Start is called before the first frame update
    void Start()
    {
        XROrigin = FindObjectOfType<XROrigin>().transform;
        moveProvider = XROrigin.GetComponent<ContinuousMoveProviderBase>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        triggerValue = triggerAction.action.ReadValue<float>();
        gripValue = gripAction.action.ReadValue<float>();
        primaryAxisPosition = primaryAxisTouchAction.action.ReadValue<Vector2>();

        animator.SetFloat("Trigger", triggerValue);
        animator.SetFloat("Grip", gripValue);
    }

    private void PrimaryAxisClick(InputAction.CallbackContext callbackContext)
    {
        moveProvider.moveSpeed = moveProvider.moveSpeed == walkSpeed ? runSpeed : walkSpeed;
    }

    public void ToggleMovement(bool value)
    {
        moveProvider.moveSpeed = value ? walkSpeed : 0;
    }
}
