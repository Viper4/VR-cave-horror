using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Climbable : XRBaseInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("Select Enter: " + args.interactorObject.transform.name);
        Hand interactorHand = args.interactorObject.transform.parent.GetComponent<Hand>();
        interactorHand.GetComponent<GrabMoveProvider>().enabled = false;
        Climber.climbingHand = interactorHand;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (Climber.climbingHand == args.interactorObject.transform.parent.GetComponent<Hand>())
        {
            Debug.Log("Select Exit");
            if(PauseUI.grabMove)
                Climber.climbingHand.GetComponent<GrabMoveProvider>().enabled = true;
            Climber.climbingHand = null;
        }
    }
}
