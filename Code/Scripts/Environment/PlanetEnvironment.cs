using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetEnvironment : MonoBehaviour
{
    public static PlanetEnvironment instance;

    [SerializeField] float minFogDensity = 0.0075f;
    [SerializeField] float maxFogDensity = 0.1f;

    [SerializeField] float minAtmosphereThickness = 2;
    [SerializeField] float maxAtmosphereThickness = 4;

    [SerializeField] float startLightIntensity = 1f;
    [SerializeField] float endLightIntensity = 0.1f;

    [SerializeField] float fogTime = 400;
    [SerializeField] float stormTime = 300;
    float timer;

    [SerializeField] Light worldLight;
    [SerializeField] MeshRenderer skyFogRenderer;
    public float fogDensity { get; private set; }

    [SerializeField] Transform player;
    public float minDepth = -8;
    public float maxDepth = 0;

    [SerializeField] ParticleSystem dustStorm;
    [SerializeField] AmbianceControl ambianceControl;

    private void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    void Start()
    {
        if (instance == null)
            instance = this;
        fogDensity = minFogDensity;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float fogT = timer / fogTime;
        fogDensity = Mathf.Lerp(minFogDensity, maxFogDensity, fogT);
        RenderSettings.skybox.SetFloat("_AtmosphereThickness", Mathf.Lerp(minAtmosphereThickness, maxAtmosphereThickness, fogT));
        skyFogRenderer.material.color = new Color(0, 0, 0, fogT);
        worldLight.intensity = Mathf.Lerp(startLightIntensity, endLightIntensity, fogT);

        if (timer > stormTime && !dustStorm.gameObject.activeSelf)
        {
            dustStorm.gameObject.SetActive(true);
            ambianceControl.StartStormAudio();
        }

        float depthT = Mathf.Abs(Mathf.Clamp(player.position.y, minDepth, maxDepth)) / (maxDepth - minDepth);
        RenderSettings.fogDensity = Mathf.Lerp(fogDensity, 0, depthT);
    }
}
