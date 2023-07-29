using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class HoverHighlight : MonoBehaviour
{
    XRBaseInteractable interactable;
    [SerializeField] MeshRenderer highlight;

    void OnEnable()
    {
        interactable = GetComponent<XRBaseInteractable>();
        if(highlight == null)
            highlight = transform.Find("Highlight").GetComponent<MeshRenderer>();
        interactable.hoverEntered.AddListener(HoverEnter);
        interactable.hoverExited.AddListener(HoverExit);
    }

    private void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(HoverEnter);
        interactable.hoverExited.RemoveListener(HoverExit);
    }

    private void HoverEnter(HoverEnterEventArgs args)
    {
        highlight.enabled = true;
    }

    private void HoverExit(HoverExitEventArgs args)
    {
        highlight.enabled = false;
    }
}
