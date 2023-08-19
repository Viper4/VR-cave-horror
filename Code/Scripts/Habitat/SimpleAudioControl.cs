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

    IEnumerator Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.volume = Random.Range(minVolume, maxVolume);
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        audioSource.Play();
    }
}
