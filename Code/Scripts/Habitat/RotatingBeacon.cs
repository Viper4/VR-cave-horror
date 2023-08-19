using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBeacon : MonoBehaviour
{
    [SerializeField] bool active = true;
    [SerializeField] float rotateSpeed = 1;
    [SerializeField] float startupSpeed = 0.75f;
    float rotateT = 0;

    MeshRenderer meshRenderer;
    [SerializeField] Material onMaterial;
    [SerializeField] Material offMaterial;

    [SerializeField] GameObject _light;

    [SerializeField] float fogTrigger = 0.025f;


    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            float lerpedRotateSpeed = Mathf.Lerp(0, rotateSpeed, rotateT);
            rotateT += startupSpeed * Time.deltaTime;
            transform.Rotate(transform.up, lerpedRotateSpeed * Time.deltaTime);
            if (PlanetEnvironment.instance.fogDensity < fogTrigger)
                SetActive(false);
        }
        else
        {
            if (PlanetEnvironment.instance.fogDensity >= fogTrigger)
                SetActive(true);
        }
    }

    public void SetActive(bool value)
    {
        active = value;
        rotateT = 0;
        List<Material> materials = new List<Material>();
        meshRenderer.GetMaterials(materials);
        materials[1] = value ? onMaterial : offMaterial;
        meshRenderer.SetMaterials(materials);
        _light.SetActive(value);
    }
}
