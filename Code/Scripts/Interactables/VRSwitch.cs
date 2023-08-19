using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRSwitch : XRBaseInteractable
{
    public uint currentState = 0;
    [SerializeField] Transform[] states;
    [SerializeField] bool changeCurrentState = true;

    AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;

    void Start()
    {
        transform.SetPositionAndRotation(states[currentState].transform.position, states[currentState].transform.rotation);
        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if(changeCurrentState)
        {
            currentState++;
            if (currentState >= states.Length)
                currentState = 0;
        }
        if(audioSource != null)
        {
            if(currentState >= audioClips.Length)
                audioSource.PlayOneShot(audioClips[^1]);
            else
                audioSource.PlayOneShot(audioClips[currentState]);
        }
        transform.SetPositionAndRotation(states[currentState].transform.position, states[currentState].transform.rotation);
    }
}
