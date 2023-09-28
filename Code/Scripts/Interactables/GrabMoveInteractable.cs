using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabMoveInteractable : XRBaseInteractable
{
    HandGrabMovement handGrabMovement;

    private void Start()
    {
        handGrabMovement = Player.instance.GetComponent<HandGrabMovement>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {

        base.OnSelectEntered(args);
        Debug.Log("Select Enter: " + args.interactorObject.transform.name);
        Hand interactorHand = args.interactorObject.transform.parent.GetComponent<Hand>();
        handGrabMovement.AddHand(interactorHand);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        Debug.Log("Select Exit: " + args.interactorObject.transform.name);
        handGrabMovement.RemoveHand(args.interactorObject.transform.parent.GetComponent<Hand>());
    }
}
