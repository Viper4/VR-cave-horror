using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] UnityEvent enterEvent;
    [SerializeField] UnityEvent exitEvent;

    private void OnTriggerEnter(Collider other)
    {
        enterEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        exitEvent?.Invoke();
    }
}
