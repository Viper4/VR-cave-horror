using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AirlockControl : MonoBehaviour
{
    bool pressurized = true;

    [SerializeField] Image[] statusImages;
    [SerializeField] Color pressurizedImageColor = Color.green;
    [SerializeField] Color depressurizedImageColor = Color.red;
    [SerializeField] Color waitImageColor = Color.yellow;
    [SerializeField] Color pressurizedTextColor = Color.green;
    [SerializeField] Color depressurizedTextColor = Color.red;
    [SerializeField] Color waitTextColor = Color.yellow;

    [SerializeField] Slider[] pressureSliders;
    [SerializeField] Button[] buttons;
    [SerializeField] AudioSource panelAudio;
    [SerializeField] AudioClip secureDoorsClip;
    [SerializeField] AudioClip pressurizingClip;
    [SerializeField] AudioClip depressurizingClip;
    [SerializeField] AudioClip airlockDoneClip;

    [SerializeField] float minPressurizeTime = 7.5f;
    [SerializeField] float maxPressurizeTime = 8.5f;
    [SerializeField] ParticleSystem airlockParticles;
    [SerializeField] AudioSource pressureAudio;

    Color buttonColor;

    [SerializeField] Door innerDoor;
    [SerializeField] LeverInteractable innerLever;
    [SerializeField] Door outerDoor;
    [SerializeField] LeverInteractable outerLever;

    // Start is called before the first frame update
    void Start()
    {
        if(buttons.Length > 0)
            buttonColor = buttons[0].image.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePressure()
    {
        StartCoroutine(PressureRoutine());
    }

    IEnumerator PressureRoutine()
    {
        if (!outerDoor.locked || !innerDoor.locked)
        {
            foreach(Button button in buttons)
            {
                button.image.color = Color.red;
                button.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "SECURE DOORS";
                panelAudio.PlayOneShot(secureDoorsClip);
            }
            yield return new WaitForSeconds(2);
            foreach (Button button in buttons)
            {
                button.image.color = buttonColor;
                button.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = pressurized ? "DEPRESSURIZE" : "PRESSURIZE";
            }
            yield break;
        }

        foreach(Image statusImage in statusImages)
        {
            statusImage.color = waitImageColor;
            TextMeshProUGUI statusText = statusImage.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            statusText.color = waitTextColor;
            statusText.text = pressurized ? "DEPRESSURIZING" : "PRESSURIZING";
        }

        if (pressurized)
            panelAudio.PlayOneShot(depressurizingClip);
        else
            panelAudio.PlayOneShot(pressurizingClip);

        innerLever.enabled = false;
        outerLever.enabled = false;
        foreach (Button button in buttons) 
        {
            button.interactable = false;
            button.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "WAIT...";
        }

        airlockParticles.Play();
        pressureAudio.Play();

        float time = 0;
        float t = 0;
        float pressurizeTime = Random.Range(minPressurizeTime, maxPressurizeTime);
        while (time < pressurizeTime)
        {
            time += Time.deltaTime;
            t = time / pressurizeTime;
            if (pressurized)
                t = 1 - t;
            foreach(Slider pressureSlider in pressureSliders)
            {
                pressureSlider.value = t;
                pressureSlider.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = Mathf.Round(t * 100) + "%";
            }
            yield return null;
        }

        pressurized = !pressurized;
        panelAudio.PlayOneShot(airlockDoneClip);
        foreach (Image statusImage in statusImages)
        {
            TextMeshProUGUI statusText = statusImage.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            if (pressurized)
            {
                statusImage.color = pressurizedImageColor;
                statusText.color = pressurizedTextColor;
                statusText.text = "PRESSURIZED";
            }
            else
            {
                statusImage.color = depressurizedImageColor;
                statusText.color = depressurizedTextColor;
                statusText.text = "DEPRESSURIZED";
            }
        }
        innerLever.enabled = pressurized;
        outerLever.enabled = !pressurized;
        foreach (Button button in buttons)
        {
            button.interactable = true;
            button.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = pressurized ? "DEPRESSURIZE" : "PRESSURIZE";
        }

        if (pressurized)
        {
            if(innerLever.TryGetComponent<AudioSource>(out var leverAudio))
            {
                leverAudio.Play();
            }
        }
        else
        {
            if (outerLever.TryGetComponent<AudioSource>(out var leverAudio))
            {
                leverAudio.Play();
            }
        }
    }
}
