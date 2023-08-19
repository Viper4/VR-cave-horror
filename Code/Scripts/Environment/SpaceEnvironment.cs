using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEnvironment : MonoBehaviour
{
    public static SpaceEnvironment instance;

    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform planet;
    [SerializeField] Transform atmosphere;
    Material atmosphereMaterial;
    [SerializeField] Vector3 planetRotationAxis = Vector3.up;
    [SerializeField] float planetRotateSpeed = 1;

    [SerializeField] Transform star;
    [SerializeField] Vector3 starOffset;
    [SerializeField] Vector3 starRealScale;
    [SerializeField] float starOrbitRadius = 100000;
    [SerializeField] float scaleFactor = 1000;
    [SerializeField] float starOrbitSpeed = 1;
    float orbitTimer = 0;

    [SerializeField] Transform directionalLight;

    [SerializeField] Transform[] solarArrays;

    void Start()
    {
        if (instance == null)
            instance = this;
        star.localScale = new Vector3((float)(starRealScale.x / scaleFactor), (float)(starRealScale.y / scaleFactor), (float)(starRealScale.z / scaleFactor));
        atmosphereMaterial = atmosphere.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void Update()
    {
        float x = Mathf.Cos(orbitTimer) * starOrbitRadius + starOffset.x + planet.position.x;
        float z = Mathf.Sin(orbitTimer) * starOrbitRadius + starOffset.z + planet.position.z;
        Vector3 toPlanet = (planet.position - new Vector3(x, starOffset.y + planet.position.y, z)).normalized;
        atmosphereMaterial.SetVector("_LightDirection", toPlanet);
        atmosphereMaterial.SetVector("_PlanetPosition", planet.position);
        planet.Rotate(planetRotateSpeed * Time.deltaTime * planetRotationAxis);

        orbitTimer += starOrbitSpeed * Time.deltaTime;
        float signedAngle = -Vector3.SignedAngle(Vector3.forward, toPlanet, Vector3.up);
        if (signedAngle < 0)
            signedAngle += 360;
        RenderSettings.skybox.SetFloat("_Rotation", signedAngle);

        star.position = new Vector3((float)((x - cameraTransform.position.x) / scaleFactor + cameraTransform.position.x), (float)((starOffset.y + planet.position.y - cameraTransform.position.y) / scaleFactor + cameraTransform.position.y), (float)((z - cameraTransform.position.z) / scaleFactor + cameraTransform.position.z));

        directionalLight.rotation = Quaternion.LookRotation(toPlanet);
        foreach(Transform solarArray in solarArrays)
        {
            solarArray.rotation = Quaternion.LookRotation(toPlanet, solarArray.up);
        }
    }
}
