using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Joystick : MonoBehaviour
{
    XRSimpleInteractable interactable;
    [SerializeField] Transform baseTransform;
    [SerializeField] Transform pivot;
    [SerializeField] GameObject tip;
    bool showTip = true;

    [SerializeField] float maxAngle = 35;
    Hand snappedHand = null;

    public Vector3 direction { get; private set; }

    private void OnEnable()
    {
        interactable = GetComponent<XRSimpleInteractable>();

        interactable.selectEntered.AddListener(SelectEnter);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(SelectEnter);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(snappedHand != null)
        {
            if (showTip && snappedHand.primaryAxisPosition != Vector2.zero)
            {
                showTip = false;
                tip.SetActive(false);
            }
            direction = new Vector3(snappedHand.primaryAxisPosition.x, 0, snappedHand.primaryAxisPosition.y);
            pivot.localEulerAngles = new Vector3(direction.x * maxAngle, 0, direction.z * maxAngle);
        }
        else
        {
            pivot.rotation = Quaternion.RotateTowards(pivot.rotation, baseTransform.rotation, 5 * Time.deltaTime);
            direction = Vector3.zero;
        }
    }

    void SelectEnter(SelectEnterEventArgs args)
    {
        if (snappedHand == null)
        {
            snappedHand = args.interactorObject.transform.parent.GetComponentInChildren<Hand>();
            snappedHand.ToggleMovement(false);
            tip.SetActive(showTip);
        }
        else
        {
            DropControl();
        }
    }

    public void DropControl()
    {
        if(snappedHand != null)
        {
            snappedHand.ToggleMovement(true);
            snappedHand = null;
            tip.SetActive(false);
        }
    }
}
