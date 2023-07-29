using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    [SerializeField] bool playOnAwake = true;
    [SerializeField] float loopTime = 2;
    [SerializeField] float onTime = 0.25f;

    Material materialInstance;

    [SerializeField] Color offColor;
    [SerializeField] Color onColor;
    [SerializeField] Light _light;

    void Awake()
    {
        materialInstance = GetComponent<MeshRenderer>().material;
        if (playOnAwake)
            StartCoroutine(BlinkLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator BlinkLoop()
    {
        materialInstance.SetColor("_EmissionColor", onColor);
        if (_light != null)
            _light.enabled = true;

        yield return new WaitForSeconds(onTime);

        materialInstance.SetColor("_EmissionColor", offColor);
        if (_light != null)
            _light.enabled = false;

        yield return new WaitForSeconds(loopTime);
        StartCoroutine(BlinkLoop());
    }
}
