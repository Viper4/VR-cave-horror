using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSystem : MonoBehaviour
{
    [SerializeField] float loopTime = 2;
    [SerializeField] float onTime = 0.25f;

    [SerializeField] Light[] lights;
    [SerializeField] Color offColor;
    [SerializeField] Color onColor;

    AudioSource _audio;
    [SerializeField] AudioClip beepClip;

    int lightIndex = 0;
    [SerializeField] bool[] lightPattern;
    int audioIndex = 0;
    [SerializeField] bool[] audioPattern;

    [SerializeField] float audioDelay = 1;

    IEnumerator Start()
    {
        _audio = GetComponent<AudioSource>();
        if (onTime > loopTime)
            onTime = loopTime;
        yield return new WaitForSeconds(audioDelay);
        _audio.Play();
        yield return new WaitForSeconds(1);
        StartCoroutine(AlarmLoop());
    }

    IEnumerator AlarmLoop()
    {
        if(lightPattern.Length == 0 || lightPattern[lightIndex])
        {
            foreach(Light light in lights)
            {
                light.color = onColor;
            }
        }
        if(audioPattern.Length == 0 || audioPattern[audioIndex])
        {
            _audio.PlayOneShot(beepClip);
        }

        yield return new WaitForSeconds(onTime);

        foreach (Light light in lights)
        {
            light.color = offColor;
        }

        yield return new WaitForSeconds(loopTime - onTime);
        lightIndex++;
        if (lightIndex > lightPattern.Length - 1)
            lightIndex = 0;
        audioIndex++;
        if (audioIndex > audioPattern.Length - 1)
            audioIndex = 0;
        StartCoroutine(AlarmLoop());
    }
}
