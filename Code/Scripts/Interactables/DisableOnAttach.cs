using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class DisableOnAttach : MonoBehaviour
{
    [SerializeField] bool disableCollider = true;
    [SerializeField] bool disableHandModel = true;
    XRBaseInteractable interactable;
    Collider _collider;

    private void OnEnable()
    {
        interactable = GetComponent<XRBaseInteractable>();
        _collider = GetComponent<Collider>();

        interactable.selectEntered.AddListener(SelectEnter);
        interactable.selectExited.AddListener(SelectExit);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(SelectEnter);
        interactable.selectExited.AddListener(SelectExit);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void SelectEnter(SelectEnterEventArgs args)
    {
        if(disableCollider)
            _collider.enabled = false;
        if (disableHandModel)
            args.interactorObject.transform.parent.GetComponent<ActionBasedController>().model.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = false;
    }

    void SelectExit(SelectExitEventArgs args)
    {
        if (disableCollider)
            _collider.enabled = true;
        if(disableHandModel)
            args.interactorObject.transform.parent.GetComponent<ActionBasedController>().model.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = true;
    }
}
