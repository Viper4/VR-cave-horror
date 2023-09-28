using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystem : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform planet;
    [SerializeField] Vector3 planetPosition;
    [SerializeField] Vector3 planetScale;
    [SerializeField] Transform atmosphere;
    Material atmosphereMaterial;
    [SerializeField] Vector3 planetRotationAxis = Vector3.up;
    [SerializeField] float planetRotateSpeed = 1;
    [SerializeField] float planetScaleFactor = 1000;

    [SerializeField] Transform star;
    [SerializeField] Vector3 starOffset;
    [SerializeField] Vector3 starScale;
    [SerializeField] float starOrbitRadius = 100000;
    [SerializeField] float starOrbitSpeed = 1;
    float orbitTimer = 0;
    [SerializeField] float starScaleFactor = 1000;

    [SerializeField] Transform directionalLight;

    [SerializeField] Transform[] solarArrays;

    void Start()
    {
        planet.localScale = new Vector3((float)(planetScale.x / planetScaleFactor), (float)(planetScale.y / planetScaleFactor), (float)(planetScale.z / planetScaleFactor));
        atmosphereMaterial = atmosphere.GetComponent<MeshRenderer>().sharedMaterial;
        atmosphereMaterial.SetFloat("_PlanetRadius", planet.localScale.x + 1);
        atmosphereMaterial.SetFloat("_AtmosphereRadius", planet.localScale.x + 8);

        star.localScale = new Vector3((float)(starScale.x / starScaleFactor), (float)(starScale.y / starScaleFactor), (float)(starScale.z / starScaleFactor));
    }

    void Update()
    {
        planet.position = new Vector3((float)((planetPosition.x - cameraTransform.position.x) / planetScaleFactor + cameraTransform.position.x), (float)((planetPosition.y - cameraTransform.position.y) / planetScaleFactor + cameraTransform.position.y), (float)((planetPosition.z - cameraTransform.position.z) / planetScaleFactor + cameraTransform.position.z));

        float x = Mathf.Cos(orbitTimer) * starOrbitRadius + starOffset.x + planetPosition.x;
        float z = Mathf.Sin(orbitTimer) * starOrbitRadius + starOffset.z + planetPosition.z;
        Vector3 toPlanet = (planet.position - new Vector3(x, starOffset.y + planetPosition.y, z)).normalized;
        atmosphereMaterial.SetVector("_LightDirection", toPlanet);
        atmosphereMaterial.SetVector("_PlanetPosition", planet.position);
        planet.Rotate(planetRotateSpeed * Time.deltaTime * planetRotationAxis);

        orbitTimer += starOrbitSpeed * Time.deltaTime;
        float signedAngle = -Vector3.SignedAngle(Vector3.forward, toPlanet, Vector3.up);
        if (signedAngle < 0)
            signedAngle += 360;
        RenderSettings.skybox.SetFloat("_Rotation", signedAngle);

        star.position = new Vector3((float)((x - cameraTransform.position.x) / starScaleFactor + cameraTransform.position.x), (float)((starOffset.y + planetPosition.y - cameraTransform.position.y) / starScaleFactor + cameraTransform.position.y), (float)((z - cameraTransform.position.z) / starScaleFactor + cameraTransform.position.z));

        directionalLight.rotation = Quaternion.LookRotation(toPlanet);
        foreach (Transform solarArray in solarArrays)
        {
            solarArray.rotation = Quaternion.LookRotation(toPlanet, solarArray.up);
        }
    }
}
