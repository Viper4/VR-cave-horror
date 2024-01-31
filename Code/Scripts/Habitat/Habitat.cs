using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Habitat : MonoBehaviour
{
    [SerializeField] Slider[] oxygenSliders;
    TextMeshProUGUI[] oxygenTexts;
    [SerializeField] float targetOxygen = 21f;
    float oxygenLevel;

    [SerializeField] Slider[] nitrogenSliders;
    TextMeshProUGUI[] nitrogenTexts;
    [SerializeField] float targetNitrogen = 79f;
    float nitrogenLevel;

    [SerializeField] Slider[] CO2Sliders;
    TextMeshProUGUI[] CO2Texts;
    [SerializeField] float targetCO2 = 0.031f;
    float CO2Level;
    [SerializeField] float CO2Fluctuation = 0.1f;
    [SerializeField] float fluctuationSpeed = 1;
    float perlinX;
    
    void Start()
    {
        perlinX = Random.Range(-10000f, 10000f);

        oxygenTexts = new TextMeshProUGUI[oxygenSliders.Length];
        for(int i = 0; i < oxygenSliders.Length; i++)
        {
            oxygenTexts[i] = oxygenSliders[i].transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }
        nitrogenTexts = new TextMeshProUGUI[nitrogenSliders.Length];
        for (int i = 0; i < nitrogenSliders.Length; i++)
        {
            nitrogenTexts[i] = nitrogenSliders[i].transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }
        CO2Texts = new TextMeshProUGUI[CO2Sliders.Length];
        for (int i = 0; i < nitrogenSliders.Length; i++)
        {
            CO2Texts[i] = CO2Sliders[i].transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        CO2Level = Mathf.Round(Mathf.Clamp(targetCO2 + ((Mathf.PerlinNoise1D(perlinX) - 0.5f) * CO2Fluctuation * 2), 0, 100) * 100) * 0.0001f;
        oxygenLevel = Mathf.Round(Mathf.Clamp(targetOxygen - (CO2Level * 90), 0, 100) * 10) * 0.001f;
        nitrogenLevel = Mathf.Round(Mathf.Clamp(targetNitrogen - (CO2Level * 10), 0, 100) * 10) * 0.001f;

        for (int i = 0; i < oxygenSliders.Length; i++)
        {
            oxygenSliders[i].value = oxygenLevel;
            oxygenTexts[i].text = (oxygenLevel * 100) + "%";
        }

        for (int i = 0; i < nitrogenSliders.Length; i++)
        {
            nitrogenSliders[i].value = nitrogenLevel;
            nitrogenTexts[i].text = (nitrogenLevel * 100) + "%";
        }

        for (int i = 0; i < CO2Sliders.Length; i++)
        {
            CO2Sliders[i].value = CO2Level;
            CO2Texts[i].text = (CO2Level * 100) + "%";
        }

        perlinX += Time.deltaTime * fluctuationSpeed;
    }
}
