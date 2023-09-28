using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceControl : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] AudioReverbFilter reverbFilter;
    AudioSource audioSource;
    bool storm = false;
    [SerializeField] AudioClip outsideStormClip;
    [SerializeField] AudioClip insideStormClip;
    [SerializeField] AudioClip caveClip;
    [SerializeField] float stormFadeSpeed = 0.5f;
    [SerializeField] float clipFadeSpeed = 0.75f;
    [SerializeField] float targetVolume = 1;
    float lerpedVolume;
    [SerializeField] float stormVolumeScale = 1;
    [SerializeField] float stormVolumeSpeed = 1;
    [SerializeField] float caveVolumeScale = 1;
    [SerializeField] float caveVolumeSpeed = 1;
    float perlinX;
    float perlinScale;
    float perlinSpeed;

    float volumeT;
    bool fadingVolume = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        perlinX = Random.Range(-10000f, 10000f);
    }

    void Update()
    {
        audioSource.volume = lerpedVolume + (Mathf.PerlinNoise1D(perlinX) - 0.5f) * perlinScale * 2;
        perlinX += Time.deltaTime * perlinSpeed;
    }

    public void StartStormAudio()
    {
        storm = true;
        perlinScale = stormVolumeScale;
        perlinSpeed = stormVolumeSpeed;
        audioSource.clip = player.inShelter ? insideStormClip : outsideStormClip;
        StartCoroutine(VolumeFade(true, stormFadeSpeed));
    }

    public void EnterShelter()
    {
        if(storm && audioSource.clip != insideStormClip)
            StartCoroutine(FadeToClip(insideStormClip));
    }

    public void ExitShelter()
    {
        if(storm && audioSource.clip != outsideStormClip)
            StartCoroutine(FadeToClip(outsideStormClip));
    }

    public void EnterCave()
    {
        perlinScale = caveVolumeScale;
        perlinSpeed = caveVolumeSpeed;
        if(audioSource.clip != caveClip)
            StartCoroutine(FadeToClip(caveClip));
    }

    public void ExitCave()
    {
        perlinScale = stormVolumeScale;
        perlinSpeed = stormVolumeSpeed;
        ExitShelter();
    }

    IEnumerator FadeToClip(AudioClip newClip)
    {
        StartCoroutine(VolumeFade(false, clipFadeSpeed));
        yield return new WaitWhile(() => fadingVolume);
        audioSource.clip = newClip;
        StartCoroutine(VolumeFade(true, clipFadeSpeed));
    }

    IEnumerator VolumeFade(bool fadeIn, float fadeSpeed)
    {
        fadingVolume = true;
        volumeT = 0;

        if (fadeIn)
        {
            audioSource.Play();
            while (lerpedVolume < targetVolume)
            {
                lerpedVolume = Mathf.Lerp(0, targetVolume, volumeT);
                volumeT += Time.deltaTime * fadeSpeed;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (lerpedVolume > 0)
            {
                lerpedVolume = Mathf.Lerp(targetVolume, 0, volumeT);
                volumeT += Time.deltaTime * fadeSpeed;
                yield return new WaitForEndOfFrame();
            }
            audioSource.Stop();
        }
        fadingVolume = false;
    }
}
