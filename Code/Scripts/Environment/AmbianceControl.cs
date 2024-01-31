using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceControl : MonoBehaviour
{
    [SerializeField] Player player;
    AudioSource audioSource;
    [SerializeField] AudioReverbFilter reverbFilter;
    [SerializeField] Vector2 reverbLevelRange;

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

    bool fadingVolume = false;
    public bool inShelter = false;
    public bool inCave = false;

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
        audioSource.clip = inShelter ? insideStormClip : outsideStormClip;
        StartCoroutine(VolumeFade(true, stormFadeSpeed));
    }

    public void EnterShelter()
    {
        if (!inShelter)
        {
            inShelter = true;
            if (storm)
                StartCoroutine(FadeToClip(insideStormClip));
        }
    }

    public void ExitShelter()
    {
        if (inShelter)
        {
            inShelter = false;
            if (storm)
                StartCoroutine(FadeToClip(outsideStormClip));
        }
    }

    public void EnterCave()
    {
        if (!inCave)
        {
            inCave = true;
            perlinScale = caveVolumeScale;
            perlinSpeed = caveVolumeSpeed;
            if (audioSource.clip != caveClip)
            {
                StartCoroutine(FadeToClip(caveClip));
                StartCoroutine(ReverbFade(true, clipFadeSpeed));
            }
        }
    }

    public void ExitCave()
    {
        if (inCave)
        {
            inCave = false;
            perlinScale = stormVolumeScale;
            perlinSpeed = stormVolumeSpeed;
            ExitShelter();
            StartCoroutine(ReverbFade(false, clipFadeSpeed));
        }
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
        float lerpT = 0;

        if (fadeIn)
        {
            audioSource.Play();
            while (lerpedVolume < targetVolume)
            {
                lerpedVolume = Mathf.Lerp(0, targetVolume, lerpT);
                lerpT += Time.deltaTime * fadeSpeed;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (lerpedVolume > 0)
            {
                lerpedVolume = Mathf.Lerp(targetVolume, 0, lerpT);
                lerpT += Time.deltaTime * fadeSpeed;
                yield return new WaitForEndOfFrame();
            }
            audioSource.Stop();
        }
        fadingVolume = false;
    }

    IEnumerator ReverbFade(bool fadeIn, float fadeSpeed)
    {
        float lerpT = 0;

        if (fadeIn)
        {
            audioSource.Play();
            while (lerpedVolume < targetVolume)
            {
                reverbFilter.reverbLevel = Mathf.Lerp(reverbLevelRange.x, reverbLevelRange.y, lerpT);
                lerpT += Time.deltaTime * fadeSpeed;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (lerpedVolume > 0)
            {
                reverbFilter.reverbLevel = Mathf.Lerp(reverbLevelRange.y, reverbLevelRange.x, lerpT);
                lerpT += Time.deltaTime * fadeSpeed;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
