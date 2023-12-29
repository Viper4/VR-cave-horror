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
    int digit = 0;
    bool minutes = true;
    bool doorClosed = true;

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
        time *= 10;

        if(number != 0)
        {
            if (minutes)
            {
                if(digit > 1)
                {
                    time = 0;
                    digit = 0;
                }
                else
                {
                    time += number * 60;
                }
            }
            else
            {
                if(digit > 1)
                {
                    time = 0;
                    digit = 0;
                }
                else
                {
                    time += number;
                }
            }
        }
        if(time != 0)
            digit++;
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
        digit = 0;
        screenAudio.Play();
        if (running)
            TurnOff();
        else
            time = 0;
        UpdateTimer();
    }

    void TurnOn()
    {
        digit = 0;
        running = true;
        rotateT = 0;
        _light.SetActive(true);
        microwaveAudio.Play();
    }

    void TurnOff()
    {
        digit = 0;
        running = false;
        _light.SetActive(false);
        microwaveAudio.Stop();
        UpdateTimer();
    }

    void UpdateTimer()
    {
        timerText.text = TimeSpan.FromSeconds(time).ToString(@"mm\:ss");
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
