using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogFade : MonoBehaviour
{
    [SerializeField] float minDepth = 8;

    float initialFogDensity;

    // Start is called before the first frame update
    void Start()
    {
        initialFogDensity = RenderSettings.fogDensity;
    }

    private void Update()
    {
        RenderSettings.fogDensity = Mathf.Lerp(initialFogDensity, 0, transform.position.y / minDepth);
    }
}
