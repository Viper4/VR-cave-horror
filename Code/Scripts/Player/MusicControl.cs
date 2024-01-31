using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip[] musicClips;
    [SerializeField] float minDelay = 8;
    [SerializeField] float maxDelay = 12;
    int previousClipIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(MusicLoop());
    }

    IEnumerator MusicLoop()
    {
        previousClipIndex = Random.Range(0, musicClips.Length);
        audioSource.clip = musicClips[previousClipIndex];
        audioSource.Play();
        yield return new WaitWhile(() => audioSource.isPlaying);
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        StartCoroutine(MusicLoop());
    }
}
