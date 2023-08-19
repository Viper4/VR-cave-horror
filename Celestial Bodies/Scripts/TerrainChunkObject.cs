using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TerrainChunkObject : MonoBehaviour
{
    TerrainChunk terrainChunk;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    [SerializeField] float visibleAngle;

    private void Update()
    {
        UpdateMeshRenderer();
    } 

    public void Init(TerrainChunk chunk, float visibleAngle)
    {
        terrainChunk = chunk;
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        this.visibleAngle = visibleAngle;
    }

    private bool IsVisibleFrom(Camera camera)
    {
        Vector3 meshMidPoint = transform.TransformPoint(terrainChunk.GetPointOnCubeSphere(new Vector2(0.5f, 0.5f)));
        Vector3 normal = meshMidPoint - transform.position;
        Vector3 toCamera = camera.transform.position - meshMidPoint;
        if(Vector3.Angle(normal, toCamera) < visibleAngle)
        {
            if (meshRenderer.enabled)
            {
                return meshRenderer.isVisible;
            }
            else
            {
                foreach (Camera cameraInStack in camera.GetUniversalAdditionalCameraData().cameraStack)
                {
                    Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cameraInStack);
                    if (GeometryUtility.TestPlanesAABB(frustumPlanes, meshRenderer.bounds))
                        return true;
                }
            }
        }
        return false;
    }

    private void UpdateMeshRenderer()
    {
        meshRenderer.enabled = IsVisibleFrom(Camera.main);
    }
}
