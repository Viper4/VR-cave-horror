using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Habitat : MonoBehaviour
{
    [SerializeField] Slider oxygenSlider;
    TextMeshProUGUI oxygenText;
    [SerializeField] float targetOxygen = 21f;
    float oxygenLevel;

    [SerializeField] Slider nitrogenSlider;
    TextMeshProUGUI nitrogenText;
    float nitrogenLevel;
    [SerializeField] float targetNitrogen = 79f;

    [SerializeField] Slider CO2Slider;
    TextMeshProUGUI CO2Text;
    [SerializeField] float targetCO2 = 0.031f;
    float CO2Level;
    [SerializeField] float CO2Fluctuation = 0.1f;
    [SerializeField] float fluctuationSpeed = 1;
    float perlinX;
    
    void Start()
    {
        perlinX = Random.Range(-10000f, 10000f);

        oxygenText = oxygenSlider.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        nitrogenText = nitrogenSlider.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        CO2Text = CO2Slider.transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        CO2Level = Mathf.Round(Mathf.Clamp(targetCO2 + ((Mathf.PerlinNoise1D(perlinX) - 0.5f) * CO2Fluctuation * 2), 0, 100) * 100) / 10000;
        oxygenLevel = Mathf.Round(Mathf.Clamp(targetOxygen - (CO2Level * 90), 0, 100) * 10) / 1000;
        nitrogenLevel = Mathf.Round(Mathf.Clamp(targetNitrogen - (CO2Level * 10), 0, 100) * 10) / 1000;

        oxygenSlider.value = oxygenLevel;
        oxygenText.text = (oxygenLevel * 100) + "%";

        nitrogenSlider.value = nitrogenLevel;
        nitrogenText.text = (nitrogenLevel * 100) + "%";

        CO2Slider.value = CO2Level;
        CO2Text.text = (CO2Level * 100) + "%";

        perlinX += Time.deltaTime * fluctuationSpeed;
    }
}
