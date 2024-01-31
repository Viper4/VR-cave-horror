using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TVRemote : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] Canvas screenCanvas;
    [SerializeField] GameObject screenLight;
    [SerializeField] GameObject volumeImage;
    [SerializeField] TextMeshProUGUI volumeText;
    [SerializeField] Slider volumeSlider; 
    [SerializeField] AudioSource TVAudioSource;
    [SerializeField] AudioClip TVOnClip;
    [SerializeField] AudioClip TVOffClip;
    [SerializeField] AudioClip[] TVChannelClips;

    [SerializeField] GameObject[] channels;
    int currentChannel = 0;

    [SerializeField] float volumeAdjustRate = 0.1f;
    bool adjustVolume = false;
    Coroutine hideVolumeRoutine;

    void Update()
    {
        if (adjustVolume)
        {
            TVAudioSource.volume += volumeAdjustRate * Time.deltaTime;
            if (TVAudioSource.volume >= 1 || TVAudioSource.volume <= 0)
                volumeAdjustRate *= -1;
            volumeSlider.value = TVAudioSource.volume;
            volumeText.text = Mathf.Round(TVAudioSource.volume * 100) + "%";
        }
    }

    public void TogglePower()
    {
        active = !active;
        screenCanvas.gameObject.SetActive(active);
        screenLight.SetActive(active);
        if (active)
        {
            TVAudioSource.PlayOneShot(TVOnClip);
        }
        else
        {
            TVAudioSource.Stop();
            TVAudioSource.PlayOneShot(TVOffClip);
        }
    }

    public void StartAdjustVolume()
    {
        if(hideVolumeRoutine != null)
            StopCoroutine(hideVolumeRoutine);
        volumeImage.SetActive(true);
        adjustVolume = true;
    }

    public void EndAdjustVolume()
    {
        hideVolumeRoutine = StartCoroutine(HideVolumeImage());
        adjustVolume = false;
    }

    public void ToggleMute()
    {
        TVAudioSource.mute = !TVAudioSource.mute;
    }

    public void SwitchChannel()
    {
        channels[currentChannel].SetActive(false);
        currentChannel++;
        if (currentChannel >= channels.Length)
            currentChannel = 0;
        channels[currentChannel].SetActive(true);
        TVAudioSource.clip = TVChannelClips[currentChannel];
        TVAudioSource.Play();
    }

    IEnumerator HideVolumeImage()
    {
        yield return new WaitForSeconds(1);
        volumeImage.SetActive(false);
    }
}
