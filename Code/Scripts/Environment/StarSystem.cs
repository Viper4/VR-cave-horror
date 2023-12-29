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
    public float planetRotateSpeed = 1;
    [SerializeField] float planetScaleFactor = 1000;

    [SerializeField] Transform star;
    [SerializeField] Vector3 starOffset;
    [SerializeField] Vector3 starScale;
    [SerializeField] float starOrbitRadius = 100000;
    public float starOrbitSpeed = 1;
    float orbitAngle = 0;
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

    void LateUpdate()
    {
        planet.position = new Vector3((float)((planetPosition.x - cameraTransform.position.x) / planetScaleFactor + cameraTransform.position.x), (float)((planetPosition.y - cameraTransform.position.y) / planetScaleFactor + cameraTransform.position.y), (float)((planetPosition.z - cameraTransform.position.z) / planetScaleFactor + cameraTransform.position.z));

        orbitAngle += starOrbitSpeed * Time.deltaTime;
        if(orbitAngle > 360)
            orbitAngle = 0;
        Vector3 toPlanet = Quaternion.AngleAxis(orbitAngle, Vector3.up) * -Vector3.forward;
        atmosphereMaterial.SetVector("_LightDirection", toPlanet);
        atmosphereMaterial.SetVector("_PlanetPosition", planet.position);
        planet.Rotate(planetRotateSpeed * Time.deltaTime * planetRotationAxis);

        Vector3 starPosition = new Vector3(-toPlanet.x * starOrbitRadius + starOffset.x + planetPosition.x, starOffset.y, -toPlanet.z * starOrbitRadius + starOffset.z + planetPosition.z);

        RenderSettings.skybox.SetFloat("_Rotation", 360 - orbitAngle);

        star.position = new Vector3((float)((starPosition.x - cameraTransform.position.x) / starScaleFactor + cameraTransform.position.x), (float)((starPosition.y + planetPosition.y - cameraTransform.position.y) / starScaleFactor + cameraTransform.position.y), (float)((starPosition.z - cameraTransform.position.z) / starScaleFactor + cameraTransform.position.z));

        directionalLight.rotation = Quaternion.LookRotation(toPlanet);
        foreach(Transform solarArray in solarArrays)
        {
            solarArray.rotation = Quaternion.LookRotation(toPlanet, solarArray.up);
        }
    }
}
