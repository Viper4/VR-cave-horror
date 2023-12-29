using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    [SerializeField] bool[] activeLights;

    [SerializeField] GameObject[] lights;

    [SerializeField] MeshRenderer[] renderers;
    [SerializeField] int[] materialIndices;
    [SerializeField] Material onMaterial;
    [SerializeField] Material offMaterial;
    [SerializeField] ReflectionProbe[] reflectionProbes;

    void Start()
    {
        foreach (ReflectionProbe reflectionProbe in reflectionProbes)
        {
            reflectionProbe.RenderProbe();
        }
    }

    public void ToggleLights(int index)
    {
        activeLights[index] = !activeLights[index];
        lights[index].SetActive(activeLights[index]);
        List<Material> materials = new List<Material>();
        renderers[index].GetMaterials(materials);
        materials[materialIndices[index]] = activeLights[index] ? onMaterial : offMaterial;
        renderers[index].SetMaterials(materials);
        foreach(ReflectionProbe reflectionProbe in reflectionProbes)
        {
            reflectionProbe.RenderProbe();
        }
    }
}
