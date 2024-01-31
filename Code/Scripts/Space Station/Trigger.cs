using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] bool useTags;
    [SerializeField] List<string> triggerTags;

    [SerializeField] UnityEvent enterEvent;
    [SerializeField] UnityEvent exitEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (useTags)
        {
            for (int i = 0; i < triggerTags.Count; i++)
            {
                if (other.CompareTag(triggerTags[i]))
                {
                    enterEvent?.Invoke();
                }
            }
        }
        else
        {
            enterEvent?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (useTags)
        {
            for (int i = 0; i < triggerTags.Count; i++)
            {
                if (other.CompareTag(triggerTags[i]))
                {
                    exitEvent?.Invoke();
                }
            }
        }
        else
        {
            exitEvent?.Invoke();
        }
    }
}
