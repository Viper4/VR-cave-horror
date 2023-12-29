using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hand : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] InputActionProperty triggerAction;
    [SerializeField] InputActionProperty gripAction;
    [SerializeField] InputActionProperty primaryAxisTouchAction;
    [SerializeField] InputActionProperty primaryAxisClickAction;
    [SerializeField] InputActionProperty velocityAction;

    [SerializeField] Animator animator;

    public float triggerValue { get; private set; }
    public float gripValue { get; private set; }
    public Vector2 primaryAxisPosition { get; private set; }
    public float primaryAxisClick { get; private set; }
    public Vector3 velocity { get; private set; }

    private void OnEnable()
    {
        primaryAxisClickAction.action.performed += PrimaryAxisClick;
    }

    private void OnDisable()
    {
        primaryAxisClickAction.action.performed -= PrimaryAxisClick;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
            yield return new WaitForEndOfFrame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        triggerValue = triggerAction.action.ReadValue<float>();
        gripValue = gripAction.action.ReadValue<float>();
        primaryAxisPosition = primaryAxisTouchAction.action.ReadValue<Vector2>();
        primaryAxisClick = primaryAxisClickAction.action.ReadValue<float>();
        velocity = velocityAction.action.ReadValue<Vector3>();

        if (animator != null)
        {
            animator.SetFloat("Trigger", triggerValue);
            animator.SetFloat("Grip", gripValue);
        }

        player.running = primaryAxisClick > 0.5f;
    }

    private void PrimaryAxisClick(InputAction.CallbackContext callbackContext)
    {

    }
}