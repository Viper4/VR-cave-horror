using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Door : MonoBehaviour
{
    XRBaseInteractable interactable;
    Rigidbody _rigidbody;

    [SerializeField] HingeJoint doorHinge;
    [SerializeField] AudioSource doorAudio;
    [SerializeField] AudioClip openClip;
    [SerializeField] AudioClip closeClip;
    [SerializeField] AudioClip lockClip;
    [SerializeField] AudioClip unlockClip;

    public bool locked = false;

    Vector3 lockedPosition;
    [SerializeField] float minLockDistance = 0;

    Quaternion lockedRotation;
    [SerializeField] float minLockAngle = 5;

    bool doorClosed = true;

    void Start()
    {
        lockedPosition = transform.position;
        lockedRotation = transform.rotation;
        interactable = GetComponent<XRBaseInteractable>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (doorClosed)
        {
            if (Quaternion.Angle(transform.rotation, lockedRotation) > minLockAngle)
            {
                doorClosed = false;
                doorAudio.PlayOneShot(openClip);
            }
        }
        else
        {
            if (Quaternion.Angle(transform.rotation, lockedRotation) <= minLockAngle)
            {
                transform.SetPositionAndRotation(lockedPosition, lockedRotation);
                doorClosed = true;
                doorAudio.PlayOneShot(closeClip);
            }
        }
    }

    public void Lock()
    {
        if ((minLockDistance == 0 || (transform.localPosition - lockedPosition).sqrMagnitude < minLockDistance) && (minLockAngle == 0 || doorClosed))
        {
            locked = true;
            UpdateDoor();
            transform.SetPositionAndRotation(lockedPosition, lockedRotation);
            doorAudio.PlayOneShot(lockClip);
        }
    }

    public void Unlock()
    {
        locked = false;
        UpdateDoor();
        doorAudio.PlayOneShot(unlockClip);
    }

    void UpdateDoor()
    {
        interactable.enabled = !locked;
        _rigidbody.isKinematic = locked;
    }
}
