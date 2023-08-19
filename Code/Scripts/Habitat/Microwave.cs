using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Microwave : MonoBehaviour
{
    [SerializeField] Transform turnTable;
    [SerializeField] float rotateSpeed = 10;
    [SerializeField] float startupSpeed = 0.75f;
    float rotateT = 0;

    [SerializeField] GameObject _light;
    [SerializeField] AudioSource microwaveAudio;
    [SerializeField] HingeJoint doorHinge;
    [SerializeField] AudioClip doorOpenClip;
    [SerializeField] AudioClip doorCloseClip;
    [SerializeField] float maxAngleDifference = 1;
    [SerializeField] Transform closedDoorTransform;

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] AudioSource screenAudio;

    bool running = false;
    float time = 0;
    bool minutes = true;
    bool doorClosed = true;

    void Start()
    {
        
    }

    void Update()
    {
        if(doorClosed)
        {
            if(Quaternion.Angle(doorHinge.transform.rotation, closedDoorTransform.rotation) > maxAngleDifference)
            {
                doorClosed = false;
                TurnOff();
                microwaveAudio.PlayOneShot(doorOpenClip);
                _light.SetActive(true);
            }
        }
        else
        {
            if(Quaternion.Angle(doorHinge.transform.rotation, closedDoorTransform.rotation) <= maxAngleDifference)
            {
                doorHinge.transform.SetPositionAndRotation(closedDoorTransform.position, closedDoorTransform.rotation);
                doorClosed = true;
                TurnOff();
                microwaveAudio.PlayOneShot(doorCloseClip);
            }
        }

        if (running)
        {
            float lerpedRotateSpeed = Mathf.Lerp(0, rotateSpeed, rotateT);
            rotateT += startupSpeed * Time.deltaTime;
            turnTable.Rotate(transform.up, lerpedRotateSpeed * Time.deltaTime);
            time -= Time.deltaTime;
            UpdateTimer();
            if (time <= 0)
            {
                StartCoroutine(MicrowaveDone());
            }
        }
    }

    public void TimeCook()
    {
        screenAudio.Play();
        minutes = !minutes;
    }

    public void NumberPad(int number)
    {
        screenAudio.Play();
        if (time * 10 > 6039) // 99 minutes 99 seconds
            time = 0;
        else
            time *= 10;

        if(number != 0)
        {
            if (minutes)
                time += number * 60;
            else
                time += number;
        }
        UpdateTimer();
    }

    public void StartButton()
    {
        screenAudio.Play();
        time += 30;
        UpdateTimer();
        if (doorClosed)
            TurnOn();
    }

    public void StopButton()
    {
        screenAudio.Play();
        if (running)
            TurnOff();
        else
            time = 0;
        UpdateTimer();
    }

    void TurnOn()
    {
        running = true;
        rotateT = 0;
        _light.SetActive(true);
        microwaveAudio.Play();
    }

    void TurnOff()
    {
        running = false;
        _light.SetActive(false);
        microwaveAudio.Stop();
        UpdateTimer();
    }

    void UpdateTimer()
    {
        TimeSpan t = TimeSpan.FromSeconds(time);
        timerText.text = t.ToString(@"mm\:ss");
    }

    IEnumerator MicrowaveDone()
    {
        time = 0;
        TurnOff();
        timerText.text = "END";
        screenAudio.Play();
        yield return new WaitForSeconds(1);
        screenAudio.Play();
        yield return new WaitForSeconds(1);
        screenAudio.Play();
    }
}
