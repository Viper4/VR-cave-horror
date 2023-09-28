using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class StasisPod : MonoBehaviour
{
    [SerializeField] GameObject home;
    [SerializeField] GameObject goodMonitor;
    [SerializeField] GameObject badMonitor;
    [SerializeField] AudioSource monitorAudio;
    [SerializeField] AudioClip heartBeatClip;
    [SerializeField] AudioClip flatlineAudioClip;
    [SerializeField] bool goodStatus;

    [SerializeField] GameObject particles;
    [SerializeField] Light[] lights;

    [SerializeField] bool active = true;
    [SerializeField] TextMeshProUGUI activityText;
    [SerializeField] TextMeshProUGUI vitalsText;

    DateTime elapsedDateTime;
    [SerializeField] int startYear = 5;
    [SerializeField] int startMonth = 5;
    [SerializeField] int startDay = 20;
    [SerializeField] int startHour = 19;
    [SerializeField] int startMinute = 46;
    [SerializeField] int startSecond = 30;

    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] bool openOnStart;
    [SerializeField] Animator leftDoorAnimator;
    [SerializeField] Animator rightDoorAnimator;

    IEnumerator Start()
    {
        elapsedDateTime = new DateTime(startYear, startMonth, startDay, startHour, startMinute, startSecond);
        if (openOnStart)
        {
            active = false;
            foreach (Light light in lights)
            {
                light.color = Color.yellow;
            }
            particles.SetActive(true);
            yield return new WaitForSeconds(3f);
            leftDoorAnimator.SetTrigger("Open");
            rightDoorAnimator.SetTrigger("Open");
            yield return new WaitForSeconds(1.5f);
            foreach (Light light in lights)
            {
                light.color = Color.white;
            }
        }
    }

    void Update()
    {
        if (active)
        {
            elapsedDateTime = elapsedDateTime.AddSeconds(Time.deltaTime);
            timeText.text = $"{elapsedDateTime.Year}y {elapsedDateTime.Month}m {elapsedDateTime.Day}d\n{elapsedDateTime.Hour}hr {elapsedDateTime.Minute}min {elapsedDateTime.Second}sec";
        }
    }

    public void Home()
    {
        monitorAudio.Stop();
        home.SetActive(true);
        goodMonitor.SetActive(false);
        badMonitor.SetActive(false);
    }

    public void Monitor()
    {
        monitorAudio.Stop();
        monitorAudio.clip = goodStatus ? flatlineAudioClip : heartBeatClip;
        monitorAudio.Play();
        goodMonitor.SetActive(goodStatus);
        badMonitor.SetActive(!goodStatus);
        home.SetActive(false);
    }

    public void MuteMonitor()
    {
        monitorAudio.mute = !monitorAudio.mute;
    }

    public void EmergencyStop()
    {
        if(active)
            StartCoroutine(StopRoutine());
    }

    IEnumerator StopRoutine()
    {
        active = false;
        foreach(Light light in lights)
        {
            light.color = Color.yellow;
        }
        activityText.text = "STOPPING";
        activityText.color = Color.yellow;
        particles.SetActive(true);
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 3));
        goodStatus = false;
        if (goodMonitor.activeSelf)
        {
            badMonitor.SetActive(true);
            goodMonitor.SetActive(false);
        }
        activityText.text = "MALFUNCTION";
        activityText.color = Color.red;
        vitalsText.text = "ERR";
        vitalsText.color = Color.red;
    }

    public void ToggleButton(TextMeshProUGUI buttonText)
    {
        buttonText.text = "ERROR";
    }
}
