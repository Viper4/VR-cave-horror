using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAudioControl : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField] float minPitch = 1;
    [SerializeField] float maxPitch = 1;
    [SerializeField] float minVolume = 1;
    [SerializeField] float maxVolume = 1;
    [SerializeField] float minDelay = 0;
    [SerializeField] float maxDelay = 0;

    [SerializeField] AudioClip loopClip;
    [SerializeField] float loopTime = 1;

    int audioIndex = 0;
    [SerializeField] bool[] audioPattern;

    IEnumerator Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.volume = Random.Range(minVolume, maxVolume);
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        if (loopClip != null)
        {
            StartCoroutine(ClipLoop());
        }
        else
        {
            audioSource.Play();
        }
    }

    IEnumerator ClipLoop()
    {
        if (audioPattern.Length == 0 || audioPattern[audioIndex])
            audioSource.PlayOneShot(loopClip);
        yield return new WaitForSeconds(loopTime);
        audioIndex++;
        if (audioIndex > audioPattern.Length - 1)
            audioIndex = 0;
        StartCoroutine(ClipLoop());
    }
}
